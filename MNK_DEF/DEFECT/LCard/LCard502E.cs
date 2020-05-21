#if !LCARD_VIRTUAL
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

using x502api;


//using l502api;
using lpcieapi;
using UPAR;
using System.Threading;

namespace Defect.LCard
{
    class LCard502E : ILCard502, IDisposable
    {
        X502 hnd;
        public delegate void DOnPr(string _msg);
        public event DOnPr OnPr = null;
        double[] VoltPercent = null;
        bool IsStarted = false;
        uint CurrentSensors = 0;
        uint[] rawi = null;
        double[] raw = null;
//        double[] values = null;
        public string LastError { get; private set; }
        double f_acq = 0;
        public double F_ACQ { get { return (f_acq); } }
//        int curr_test = 0;
//        int defect = 120000;

        const uint RECV_TOUT = 250;

        public LCard502E(LCard502Pars _pars, DOnPr _OnPr = null)
        {
            if (_OnPr != null)
                OnPr += _OnPr;

            LastError = null;
            IPAddress addr = FirstIp();
            if (addr == null)
                throw (new Exception("LCard502E.LCard502E: Ошибка: Устройство не найдено"));
            /* создаем запись, соответствующую заданному адресу */
            X502.DevRec rec = E502.MakeDevRecordByIpAddr(addr, 0, 5000);
            /* создание объекта */
            hnd = X502.Create(rec.DevName);
            /* станавливаем связь устанавливаем связь по созданной записи */
            lpcie.Errs res = hnd.Open(rec);
            if (res != lpcie.Errs.OK)
                throw (new Exception(string.Format("Ошибка открытия модуля: {0}", X502.GetErrorString(res))));

            //return;
            ///* создаем описатель модуля */
            //hnd = new L502();

            //string serial = FindSerial(_pars.DevNum);
            //if (serial == null)
            //    throw (new Exception("LCard502.LCard502: Ошибка: Устройство не найдено: " + _pars.DevNum.ToString()));
            //pr("LCard502 create");
            ////            lpcie.Errs res;
            ///* устанавливаем связь по выбранному серийному номеру */
            //lpcie.Errs res = hnd.Open(serial);
            //if (res != 0)
            //    throw (new Exception(L502.GetErrorString(res) + " Ошибка открытия модуля"));
            ///* получаем информацию о модуле */
            //L502.Info devinfo = hnd.DevInfo;
        }
        public void Dispose()
        {
            if (hnd == null)
                return;
            hnd.Close();
            // память освободится диспетчером мусора, т.к. нет больше ссылок
            hnd = null;
        }
        IPAddress FirstIp()
        {
            E502.EthSvcBrowser sb = new E502.EthSvcBrowser();
            lpcie.Errs err = sb.Start();
            if (err != lpcie.Errs.OK)
                throw (new Exception(string.Format("Ошибка запуска поиска устройств в сети {0}: {1}", err, X502.GetErrorString(err))));
            IPAddress addr = null;
            for (int i = 0; i < 10; i++)
            {
                //while (err == lpcie.Errs.OK)
                //{
                E502.EthSvcEvent svc_evt;
                /* Метод EthSvcRecord предоставляет доступ ко функциям для работы с описателем сервиса.
                 * В отличие от С не нужно освобождать память вручную, т.к. освобождение выполняется
                 * в деструкторе */
                E502.EthSvcRecord svc_rec;
                err = sb.GetEvent(out svc_rec, out svc_evt, 300);
                if (err != lpcie.Errs.OK)
                    throw (new Exception(string.Format("Ошибка получения записи о найденном устройстве {0}: {1}", err, X502.GetErrorString(err))));
                if (svc_evt == E502.EthSvcEvent.NONE)
                    continue;
                if (svc_evt == E502.EthSvcEvent.REMOVE)
                {
                    pr(string.Format("Устройство отключено: {0}, S/N: {1}", svc_rec.InstanceName, svc_rec.DevSerial));
                    continue;
                }
                /* Адрес мы можем получить только для присутствующего устройства */
                if ((svc_evt == E502.EthSvcEvent.ADD) || (svc_evt == E502.EthSvcEvent.CHANGED))
                {
                    lpcie.Errs cur_err = svc_rec.ResolveIPv4Addr(out addr, 2000);
                    if (cur_err != lpcie.Errs.OK)
                        throw (new Exception(string.Format("Ошибка получения IP-адреса устройтсва {0}: {1}", err, X502.GetErrorString(err))));
                    pr(string.Format("{0}: {1}, S/N: {2}, Адрес = {3}",
                        svc_evt == E502.EthSvcEvent.ADD ? "Плата" : "Изм. параметров",
                        svc_rec.InstanceName, svc_rec.DevSerial,
                        addr.ToString()));
                    break;
                }
            }
            lpcie.Errs stop_err = sb.Stop();
            if (stop_err != lpcie.Errs.OK)
                throw (new Exception(string.Format("Ошибка останова поиска сервисов {0}: {1}", stop_err, X502.GetErrorString(stop_err))));
            return (addr);
        }

        public void LoadSettings(LCard502Pars _pars, L502Ch[] _channels)
        {
            CurrentSensors = Convert.ToUInt32(_channels.Length);
            string a = "LCard502.LoadSettings: Не удалось задать параметры";
            hnd.LChannelCount = (uint)_channels.Length;
            VoltPercent = new double[_channels.Length];
            double[] K = { 10, 20, 50, 100, 200, 500 };
            for (int i = 0; i < _channels.Length; i++)
            {
                string aa = string.Format("LCard502E.LoadSettings: Канал[{0}]: {1}", i, _channels[i].ToString());
                pr(aa);
                AdapterPars p = new AdapterPars(_channels[i]);
                LFATAL(aa, hnd.SetLChannel((uint)i, p.phy_ch, p.mode, p.range, p.avg));
                VoltPercent[i] = K[_channels[i].Range];
            }
            // Настраиваем источник частоты синхронизации
            L502.Sync[] f_sync_mode = { L502.Sync.INTERNAL, L502.Sync.EXTERNAL_MASTER };
            hnd.SyncMode = f_sync_mode[_pars.SyncMode];
//            hnd.SyncMode = L502.Sync.EXTERNAL_MASTER;

            // RAG Чтобы не поставил - не работает - ихний баг.
            //            L502.Sync[] f_sync_start_mode = { L502.Sync.DI_SYN1_RISE, L502.Sync.DI_SYN2_RISE, L502.Sync.DI_SYN1_FALL, L502.Sync.DI_SYN2_FALL };
            //            hnd.SyncStartMode = f_sync_start_mode[_pars.SyncStartMode];

            f_acq = _pars.FrequencyPerChannel * _channels.Length * 4;
            double f_lch = _pars.FrequencyPerChannel;
            // настраиваем частоту сбора с АЦП
            LFATAL(a, hnd.SetAdcFreq(ref f_acq, ref f_lch));
            // Parameters.frequencyCollect = f_acq;
            // Parameters.frequencyPerChannel = f_lch;
            // Записываем настройки в модуль

            double f_din_save = _pars.TTL.Frequency;
            LFATAL(a, hnd.SetDinFreq(ref f_din_save));
            LFATAL(a, hnd.Configure(0));
#if TTL_SIGNALS
	        LFATAL("LCard502::Start: не смогли разрешить потоки: ", hnd.StreamsEnable(L502.Streams.ADC|L502.Streams.DIN));
#else
            LFATAL("LCard502::Start: не смогли разрешить потоки: ", hnd.StreamsEnable(L502.Streams.ADC));
#endif

        }
        public void Start(LCard502Pars _pars, L502Ch[] _channels)
        {
            if (IsStarted)
                return;
            LoadSettings(_pars, _channels);
            LFATAL("LCard502::Start: не смогли стартовать: ", hnd.StreamsStart());
            IsStarted = true;
        }
        public void StartTest()
        {
            if (IsStarted)
                return;
            IsStarted = true;
            for (int iter = 0; iter < 100; iter++)
            {
                int tick0 = Environment.TickCount;
                LFATAL("LCard502::Start: не смогли стартовать: ", hnd.StreamsStart());
                int tick1 = Environment.TickCount;
                uint count;
                for (; ; )
                {
                    count = hnd.RecvReadyCount;
                    if (count != 0)
                        break;
                }
                int tick2 = Environment.TickCount;
                LFATAL("LCard502::Start: не смогли остановиться: ", hnd.StreamsStop());
                int tick3 = Environment.TickCount;
                int dtick = tick2 - tick0;
                int dtickAll = tick3 - tick0;
                pr(string.Format("dtick[{0}]: count={1} start->byte={2} cycle={3}",
                    iter.ToString(),
                    count.ToString(),
                    dtick.ToString(),
                    dtickAll.ToString()
                    ));
                Thread.Sleep(1000);
            }
            IsStarted = false;
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
                //if (curr_test >= 300000 && curr_test < 300000+40)
                //    values[i] = 100;
                //else
                //    values[i] = 0;
                //values[i] = curr_test / 10000;
                //curr_test++;

                sensor++;
                if (sensor == CurrentSensors)
                    sensor = 0;
            }
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
            }
            public uint phy_ch;
            public L502.LchMode mode;
            public L502.AdcRange range;
            public uint avg;
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
            pr("GetValueV=" + buf.ToString());
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