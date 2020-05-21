#if !LCARD_VIRTUAL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using l502api;
using lpcieapi;

using UPAR;
using Share;

namespace Defect.LCard
{
    public class LCard502 : ILCard502, IDisposable
    {
        L502 hnd;
        public event DOnPr OnPr = null;
        double[] VoltPercent = null;
        bool IsStarted = false;
        uint CurrentSensors = 0;
        uint[] rawi = null;
        double[] raw = null;
        public string LastError { get; private set; }
        double f_acq = 0;
        double f_lch = 0;
        public double F_ACQ { get { return (f_acq); } }
//        static int allCount=0;
//        int curr_test = 0;
//        int defect = 50000;

        const uint RECV_TOUT = 250;
        int first_tick = 0;

        public LCard502(LCard502Pars _pars, DOnPr _OnPr = null)
        {
            if (_OnPr != null)
                OnPr += _OnPr;

            LastError = null;
            /* создаем описатель модуля */
            hnd = new L502();

            string serial = FindSerial(_pars.DevNum);
            if (serial == null)
                throw (new Exception("LCard502.LCard502: Ошибка: Устройство не найдено: " + _pars.DevNum.ToString()));
            pr("LCard502 create");
            //            lpcie.Errs res;
            /* устанавливаем связь по выбранному серийному номеру */
            lpcie.Errs res = hnd.Open(serial);
            if (res != 0)
                throw (new Exception(L502.GetErrorString(res) + " Ошибка открытия модуля"));
            /* получаем информацию о модуле */
            L502.Info devinfo = hnd.DevInfo;
        }
        public void Dispose()
        {
            if (hnd == null)
                return;
            hnd.Close();
            // память освободится диспетчером мусора, т.к. нет больше ссылок
            hnd = null;
        }
        void LoadSettings(LCard502Pars _pars, L502Ch[] _channels)
        {
            CurrentSensors = Convert.ToUInt32(_channels.Length);
            string a = "LCard502.LoadSettings: Не удалось задать параметры";
            hnd.LChannelCount = (uint)_channels.Length;
            VoltPercent = new double[_channels.Length];
            double[] K = { 10, 20, 50, 100, 200, 500 };
            for (int i = 0; i < _channels.Length; i++)
            {
                string aa = string.Format("LCard502.LoadSettings: Канал[{0}]: {1}", i, _channels[i].ToString());
                pr(aa);
                AdapterPars p = new AdapterPars(_channels[i]);
                LFATAL(aa, hnd.SetLChannel((uint)i, p.phy_ch, p.mode, p.range, p.avg));
                if(p.IsPercent)
                    VoltPercent[i] = K[_channels[i].Range];
                else
                    VoltPercent[i] = 1;
            }
            // Настраиваем источник частоты синхронизации
            L502.Sync[] f_sync_mode = { L502.Sync.INTERNAL, L502.Sync.EXTERNAL_MASTER };
            hnd.SyncMode = f_sync_mode[_pars.SyncMode];

            // RAG Чтобы не поставил - не работает - ихний баг.
            //            L502.Sync[] f_sync_start_mode = { L502.Sync.DI_SYN1_RISE, L502.Sync.DI_SYN2_RISE, L502.Sync.DI_SYN1_FALL, L502.Sync.DI_SYN2_FALL };
            //            hnd.SyncStartMode = f_sync_start_mode[_pars.SyncStartMode];

            f_acq = _pars.FrequencyPerChannel * _channels.Length * 4;
            double f_acq_buf = f_acq;
            f_lch = _pars.FrequencyPerChannel;
            // настраиваем частоту сбора с АЦП
            LFATAL(a, hnd.SetAdcFreq(ref f_acq, ref f_lch));
            // Parameters.frequencyCollect = f_acq;
            // Parameters.frequencyPerChannel = f_lch;
            // Записываем настройки в модуль

            double f_din_save = _pars.TTL.Frequency;
            LFATAL(a, hnd.SetDinFreq(ref f_din_save));
            LFATAL(a, hnd.Configure(0));
        }
        public void Start(LCard502Pars _pars, L502Ch[] _channels)
        {
            if (IsStarted)
                return;
            LoadSettings(_pars, _channels);
#if TTL_SIGNALS
	        LFATAL("LCard502::Start: не смогли разрешить потоки: ", hnd.StreamsEnable(L502.Streams.ADC|L502.Streams.DIN));
#else
            LFATAL("LCard502::Start: не смогли разрешить потоки: ", hnd.StreamsEnable(L502.Streams.ADC));
#endif
            first_tick = Environment.TickCount;
            LFATAL("LCard502::Start: не смогли стартовать: ", hnd.StreamsStart());
            IsStarted = true;
        }
        public void Stop()
        {
            if (!IsStarted)
                return;
            IsStarted = false;
            LFATAL("LCard502::Start: не смогли остановиться: ", hnd.StreamsStop());
        }
        public double[] Read()
        {
            uint count = hnd.RecvReadyCount;
            count /= CurrentSensors;
            count *= CurrentSensors;
            SetRawSize(count);
            int rcv_size = hnd.Recv(rawi, count, RECV_TOUT);
            if (rcv_size < 0)
            {
                if (CheckError((lpcie.Errs)rcv_size))
                    return (null);
            }
            if (rcv_size != (int)count)
            {
                LastError = "Размер полученный не равен размеру запрошенному";
                return (null);
            }
            // переводим АЦП в Вольты
            uint count1 = count;
            if (CheckError(hnd.ProcessAdcData(rawi, raw, ref count1, L502.ProcFlags.VOLT)))
                return (null);
            if (count != count1)
            {
                LastError = "Размер преобразование полученный не равен размеру запрошенному";
                return (null);
            }
            int sensor = 0;
            double[] values = new double[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = raw[i] * VoltPercent[sensor];
                //if (curr_test>=1000000)
                //    values[i] = 100;
                //else
                //    values[i] = 0;
                //curr_test++;
                //values[i] = curr_test;
                //curr_test += 10;
                //if (curr_test > 100)
                //    curr_test = 10;
                sensor++;
                if (sensor == CurrentSensors)
                    sensor = 0;
            }
            //int tick=Environment.TickCount;
            //int delta=tick-first_tick;
            //allCount += Convert.ToInt32(count);
            //double fre=allCount;
            //fre/=delta;
            //pr(string.Format("delta={0} allCount={1} fre,byte/ms={2} f_acq={3} f_lch={4}",
            //    delta.ToString(),allCount.ToString(),fre.ToString(),f_acq.ToString(), f_lch.ToString()));
            //tick_prev=tick;
            return (values);
        }

        void SetRawSize(uint _size)
        {
            if (rawi == null)
            {
                rawi = new uint[_size];
                raw = new double[_size];
            }
            else
            {
                if (rawi.Length < _size)
                {
                    rawi = new uint[_size];
                    raw = new double[_size];
                }
            }
        }
        bool CheckError(lpcie.Errs _err)
        {
            if (_err == 0)
                return (false);
            LastError = L502.GetErrorString(_err);
            return (true);
        }

        void LFATAL(string _msg, lpcie.Errs _err)
        {
            if (_err != 0)
                throw (new Exception(_msg + " " + L502.GetErrorString(_err)));
        }

        class AdapterPars
        {
            public AdapterPars(L502Ch _pars)
            {
                phy_ch = Convert.ToUInt32(_pars.ChPhisical);
                L502.LchMode[] f_mode_tbl = { L502.LchMode.COMM, L502.LchMode.DIFF, L502.LchMode.ZERO };
                mode = f_mode_tbl[_pars.Mode];
                L502.AdcRange[] f_range_tbl = {L502.AdcRange.RANGE_10, L502.AdcRange.RANGE_5, L502.AdcRange.RANGE_2,
									    L502.AdcRange.RANGE_1, L502.AdcRange.RANGE_05, L502.AdcRange.RANGE_02};
                range = f_range_tbl[_pars.Range];
                avg = Convert.ToUInt32(_pars.Avg);
                IsPercent = _pars.IsPersent;
            }
            public uint phy_ch;
            public L502.LchMode mode;
            public L502.AdcRange range;
            public uint avg;
            public bool IsPercent;
        }

        void pr(string _msg)
        {
            if (OnPr != null)
                OnPr(_msg);
        }
        string FindSerial(int _devNum)
        {
            String[] serials;
            Int32 res = L502.GetSerialList(out serials, 0);
            if (res <= 0)
                throw (new Exception("LCard502.FindSerial: Ошибка: не могу получить список плат"));
            if (_devNum >= serials.Length)
                return (null);
            return (serials[_devNum]);
        }
        public double GetValueV(L502Ch _channel, LCard502Pars _parsLCard502Pars, ref bool _ret)
        {
            if (IsStarted)
            {
                _ret = false;
                return (0);
            }
            double[] buf = new double[1];
            LoadSettings(_parsLCard502Pars, new L502Ch[] { _channel });
            _ret = !CheckError(hnd.AsyncGetAdcFrame(L502.ProcFlags.VOLT, 1000, buf));
            pr("GetValueV=" + buf[0].ToString());
            return (buf[0]);
        }
        public double GetValueP(L502Ch _channel, LCard502Pars _parsLCard502Pars, ref bool _ret)
        {
            return (GetValueP(GetValueV(_channel, _parsLCard502Pars, ref _ret)));
        }
        public double GetValueP(double _val)
        {
            return (_val * VoltPercent[0]);
        }
    }
}
#endif
