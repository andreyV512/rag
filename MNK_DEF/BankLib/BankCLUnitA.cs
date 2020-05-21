using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Protocol;
using UPAR;
using UPAR.Def;
using UPAR_common;
using Share;

namespace BankLib
{
    public class BankCLUnitA : BankDataUnit, ILoadSettings
    {
        BankZoneDataA prevZone = null;
        int ZoneLength = 0;
        public bool GotZones = false;
        double[] MOffset;
        double SensorsPosition = 0;
        public double f_acq { get; private set; }
        public int GaveZones { get { return (prevZone == null ? 0 : prevZone.index + 1); } }

        public BankCLUnitA(EUnit _type, cIW _IW)
            : base(_type)
        {
            Clear();
            DefCL dcl = new DefCL(Tp);
            //            f_acq = (dcl.LCh.Count + (_IW.SG ? 2 : 0)) * dcl.L502.FrequencyPerChannel;
            f_acq = dcl.LCh.Count * dcl.L502.FrequencyPerChannel;
        }

        public new void Clear()
        {
            base.Clear();
            prevZone = null;
            GotZones = false;
        }
        public void LoadSettings()
        {
            DefCL dcl = new DefCL(Tp);
            SensorsPosition = dcl.SensorsPosition;
            Sensors = dcl.LCh.Count;
            MOffset = new double[Sensors];
            for (int i = 0; i < Sensors; i++)
                MOffset[i] = dcl.LCh[i].IOffset;
            ReSize(dcl.Buffer * 1024 * 1024 / sizeof(double));
            ZoneLength = ParAll.ST.ZoneSize;
            SetDeadEnd(ParAll.ST.ZoneSize, dcl.DeadZoneFinish);
            SetDeadEnd(ParAll.ST.ZoneSize, Convert.ToInt32(Math.Ceiling(dcl.MultEnd)));

            Clear();
        }
        public int Zones
        {
            get
            {
                if (!isStarted)
                    return (0);
                if (prevZone == null)
                    return (0);
                return (prevZone.index + 1);
            }
        }
        int? IndexByTick(double? _tick)
        {
            if (_tick == null)
            {
                //pr("_tick == null");
                return (null);
            }
            if (firstTick == null)
            {
                //pr("firstTick == null");
                return (null);
            }
            if (f_acq == 0)
            {
                //pr("f_acq == 0");
                return (null);
            }
            if (Count == 0)
            {
                //pr("Count == 0");
                return (null);
            }
            //pr("_tick=" + _tick.Value.ToString());
            //pr("f_acq=" + f_acq.ToString());
            double dindex = f_acq;
            dindex /= 1000;
            dindex *= _tick.Value;
            dindex /= Sensors;
            int index = Convert.ToInt32(dindex);
            index *= Sensors;
            //pr("index=" + index.ToString());
            //pr("Count=" + Count.ToString());

            //string a = string.Format("c={0} ftl={1}.{2}.{3} ind={4}",
            //    Count.ToString(),
            //    firstTick.Value.ToString(),
            //    _tick.Value.ToString(),
            //    lastTick.ToString(),
            //    index.ToString()
            // );
            //pr(a);
            if (index < 0)
            {
                //pr("index < 0");
                return (null);
            }
            if (index >= Count)
            {
                //string a = string.Format("index >= Count index={0} Count={1}",
                //index.ToString(),
                //Count.ToString());
                return (null);
            }
            //pr("OKindex" + index.ToString());
            return (index);
        }
        BankZoneData GetNextZone(int _sensor, int? _TubeLength, L_TickPosition _MTP)
        {
            double soffset = SensorsPosition + MOffset[_sensor];
            if (prevZone == null)
            {
                //                string sss = _second ? " 2" : " 1";
                double? tbp = _MTP.TickByPosition(soffset);
                //pr("soffset=" + soffset.ToString());
                //pr("TickByPosition=" + tbp.ToString());
                int? idata0 = IndexByTick(tbp);
                //pr("idata0="+idata0.ToString());
                if (idata0 == null)
                {
                    //                    pr("idata0"+sss);
                    return (null);
                }

                int? idata1 = IndexByTick(_MTP.TickByPosition(soffset + ZoneLength));
                if (idata1 == null)
                {
                    //                    pr("idata1" + sss);
                    return (null);
                }
                if (idata1.Value == idata0.Value)
                {
                    //                    pr("idata==idata1" + sss);
                    return (null);
                }

                int zsize = idata1.Value - idata0.Value;
                zsize /= Sensors;
                zsize *= Sensors;
                if (zsize <= 0)
                {
                    //                    pr("zsize <= 0" + sss);
                    return (null);
                }

                BankZoneData z = new BankZoneData(BankZone.EUnitToEType(Tp));
                z.length = ZoneLength;
                z.index = 0;
                z.idata = idata0.Value;
                z.size = zsize;
                //                pr("------ " + _sensor.ToString() + " --------" + z.ToString());
                return (z);
            }
            else
            {
                if (prevZone.last)
                    return (null);
                BankZoneData z = new BankZoneData(BankZone.EUnitToEType(Tp));
                BankZoneData pz = prevZone.MZones[_sensor];
                z.idata = pz.idata + pz.size;
                z.length = ZoneLength;
                z.index = prevZone.index + 1;

                double pos = ZoneLength * (z.index + 1);
                if (_TubeLength != null)
                {
                    if (pos >= _TubeLength.Value)
                    {
                        z.last = true;
                        z.length = Convert.ToInt32(ZoneLength - (pos - _TubeLength.Value));
                        pos = _TubeLength.Value;
                    }
                }
                pos += soffset;
                //pr("pos=" + pos.ToString());
                //if (_MTP.Count != 0)
                //{
                //    pr("_MTP[0].position=" + _MTP[0].position.ToString());
                //    pr(string.Format("_MTP[{0}].position={1}", (_MTP.Count - 1).ToString(), _MTP[_MTP.Count - 1].position.ToString()));
                //}

                //int lcount = _MTP.Count;

                double? tbp = _MTP.TickByPosition(pos);
                int? idata1 = IndexByTick(tbp);
                if (idata1 == null)
                    return (null);

                z.size = idata1.Value - z.idata;
                z.size /= Sensors;
                z.size *= Sensors;
                if (z.size <= 0)
                    return (null);
                //pr(string.Format("pos: {0} + soffset: {1} = {2}", pos.ToString(), soffset.ToString(), (pos + soffset).ToString()));
                //pr(string.Format("tbp={0}", tbp.ToString()));
                //pr(string.Format("idata={0}", idata1.ToString()));
                return (z);
            }
        }
        public BankZoneDataA GetNextDataUnitZoneA(int? _TubeLength, L_TickPosition _MTP)
        {
            if (IsComplete())
                return (null);
            BankZoneDataA z = new BankZoneDataA(BankZone.EUnitToEType(Tp), Sensors, prevZone == null ? 0 : prevZone.index + 1);
            for (int i = 0; i < Sensors; i++)
            {
                BankZoneData zi = GetNextZone(i, _TubeLength, _MTP);
                if (zi == null)
                    return (null);
                z.Add(i, zi);
            }
            //            BankZoneData2 z = BankZoneData2.Create(GetNextZone02(false, _TubeLength, _MTP), GetNextZone02(true, _TubeLength, _MTP));
            if (z == null)
                return (null);
            if (z.last)
                GotZones = true;
            //				pr(AnsiString("z.index=") + z.index);
            //				pr(AnsiString("z1.data=") + (unsigned long)(z1.data));
            //				pr(AnsiString("z1.data2=") + (unsigned long)
            //					(z1.data + z1.size));
            //				pr(AnsiString("z2.data=") + (unsigned long)(z2.data));
            //				pr(AnsiString("z2.data2=") + (unsigned long)
            //					(z2.data + z2.size));
            //				pr(AnsiString("deltaZ2Z1=") + ((int)(z2.data - z1.data)));
            //				pr(AnsiString("SensorsPosition=") + _dataUnit->SensorsPosition);
            //				pr(AnsiString("SensorsPosition2=") +
            //					_dataUnit->SensorsPosition2);
            //				pr(z.ToString());
            prevZone = z;
            if (z.last)
                complete = true;
            return (z);
        }
        public BankZoneDataA GetDataUnitZoneACalibr(bool _check)
        {
            if (IsComplete())
                return (null);
            BankZoneDataA z = new BankZoneDataA(BankZone.EUnitToEType(Tp), Sensors, prevZone == null ? 0 : prevZone.index + 1);
            for (int i = 0; i < Sensors; i++)
            {
                BankZoneData zi = new BankZoneData(BankZone.EType.LINE);
                zi.index = 0;
                zi.idata = 0;
                zi.size = Count;
                zi.size /= Sensors;
                zi.size *= Sensors;
                zi.length = ZoneLength;
                zi.last = true;
                z.Add(i, zi);
            }
            GotZones = true;
            if (!_check)
            {
                //				pr(AnsiString("z.index=") + z.index);
                //				pr(AnsiString("z1.data=") + (unsigned long)(z1.data));
                //				pr(AnsiString("z1.data2=") + (unsigned long)
                //					(z1.data + z1.size));
                //				pr(AnsiString("z2.data=") + (unsigned long)(z2.data));
                //				pr(AnsiString("z2.data2=") + (unsigned long)
                //					(z2.data + z2.size));
                //				pr(AnsiString("deltaZ2Z1=") + ((int)(z2.data - z1.data)));
                //				pr(AnsiString("SensorsPosition=") + _dataUnit->SensorsPosition);
                //				pr(AnsiString("SensorsPosition2=") +
                //					_dataUnit->SensorsPosition2);
                //				pr(z.ToString());
                if (z.last)
                    complete = true;
            }
            return (z);
        }
        void pr(string _msg)
        {
            ProtocolST.pr("BankCLUnitA: " + _msg);
        }
        public new void Add(double[] _data, int _offset, int _size)
        {
            if (GotZones)
                return;
            if (!isStarted)
                return;
            int packets = _data.Length / _size;
            if (packets * _size != _data.Length)
                throw (new Exception(string.Format("Bank::AddGroup: Размер пакета не кратен количеству датчиков: Cross={0} size={1}", Sensors.ToString(), _data.Length.ToString())));
            base.Add(_data, _offset, _size);
        }
        public int MaxCount
        {
            get
            {
                if (prevZone == null)
                    return (0);
                return (prevZone.index + 1);
            }
        }
        public override string ToString()
        {
            return (base.ToString() + " f_acq=" + f_acq.ToString());
        }
    }
}
