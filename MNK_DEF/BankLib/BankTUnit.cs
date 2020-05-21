using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UPAR;
using Protocol;
//using ResultLib.Thick;

namespace BankLib
{
    public class BankTUnit : BankUnit, ILoadSettings
    {
        public BankTUnit()
            : base(BankZone.EType.THICK)
        {
            LoadSettings();
            GotZones = false;
        }
        public RawStrobes rawStrobes;
        public int Count { get { return (rawStrobes.Count); } }
        List<ZoneThick> ZonesAll = new List<ZoneThick>(100);
        List<ZoneThick> ZonesGot = new List<ZoneThick>(100);
        List<ZoneThick> ZonesCalced = new List<ZoneThick>(100);
        List<ZoneThick> ZonesComputed = new List<ZoneThick>(100);
        int ZoneLength = 0;
        public bool GotZones { get; private set; }

        public void LoadSettings()
        {
            rawStrobes = new RawStrobes();
            SensorsPosition = ParAll.ST.Dimensions.TSensors;
            ZoneLength = ParAll.ST.ZoneSize;
        }
        public new void Clear()
        {
            rawStrobes.Clear();
            ZonesAll.Clear();
            ZonesGot.Clear();
            ZonesCalced.Clear();
            ZonesComputed.Clear();
            GotZones = false;
            base.Clear();
        }
        bool Half(double _tick, ref int _left, ref int _right, ref int? result)
        {
            result = null;
            if (_left >= _right)
                return (false);
            double left_tick = rawStrobes[_left].tick;
            double right_tick = rawStrobes[_right].tick;

            if (_tick < left_tick || _tick > right_tick)
                return (false);
            if (_tick == left_tick)
            {
                result = _left;
                return (true);
            }
            if (_tick == right_tick)
            {
                result = _right;
                return (true);
            }
            if (_right - _left == 1)
            {
                result = _left;
                return (true);
            }
            int middle = (_left + _right) / 2;
            if (_tick <= rawStrobes[middle].tick)
                _right = middle;
            else
                _left = middle;
            return (true);
        }
        int? IndexByTick(double? _tick)
        {
            if (_tick == null)
                return (null);
            if (Count < 2)
                return (null);
            int left = 0;
            int right = Count - 1;
            int? result = null;
            for (; ; )
            {
                if (!Half(_tick.Value, ref left, ref right, ref result))
                    return (null);
                if (result != null)
                    return (result.Value);
            }
        }
        void NextZone(int? _TubeLength, L_TickPosition _MTP)
        {
            ZoneThick PrevZone = ZonesAll.LastOrDefault();
            if (PrevZone == null)
            {
                int? idata0 = IndexByTick(_MTP.TickByPosition(SensorsPosition));
                if (idata0 == null)
                    return;

                int? idata1 = IndexByTick(_MTP.TickByPosition(SensorsPosition + ZoneLength));
                if (idata1 == null)
                    return;
                if (idata1.Value == idata0.Value)
                    return;

                int zsize = idata1.Value - idata0.Value;
                if (zsize <= 0)
                    return;

                ZoneThick z = new ZoneThick(rawStrobes, idata0.Value, zsize, 0, ZoneLength, false);
                ZonesAll.Add(z);
                ZonesGot.Add(z);
                pr(z.ToString());
            }
            else
            {
                if (PrevZone.Last)
                    return;
                int idata = PrevZone.rawstrobes_index + PrevZone.Size;
                int length = ZoneLength;
                bool last = false;

                double pos = ZoneLength * (PrevZone.Index + 2);
                if (_TubeLength != null)
                {
                    if (pos >= _TubeLength.Value)
                    {
                        last = true;
                        length = Convert.ToInt32(ZoneLength - (pos - _TubeLength.Value));
                        pos = _TubeLength.Value;
                    }
                }
                int? idata1 = IndexByTick(_MTP.TickByPosition(SensorsPosition + pos));
                if (idata1 == null)
                    return;

                int size = idata1.Value - idata;
                if (size <= 0)
                    return;
                ZoneThick z = new ZoneThick(rawStrobes, idata, size, PrevZone.Index + 1, length, last);
                ZonesAll.Add(z);
                ZonesGot.Add(z);
                pr(z.ToString());
            }
        }
        public SensorThick GetToCalc()
        {
            //            pr("GetToCalc: " + Zones.Count.ToString());
            foreach (ZoneThick z in ZonesGot)
            {
                foreach (SensorThick s in z.MSensor)
                {
                    if (!s.calcing && !s.calced)
                    {
                        s.calcing = true;
                        return (s);
                    }
                }
            }
            return (null);
        }
        public void SetCalced(SensorThick _s)
        {
            _s.calced = true;
            ZoneThick z = ZonesGot[0];
            foreach (SensorThick s in z.MSensor)
            {
                if (!s.calced)
                    return;
            }
            ZonesGot.RemoveAt(0);
            ZonesCalced.Add(z);
        }
        int CalcBeginDeadZones()
        {
            double v = ParAll.ST.TSSet.Current.TSThick.DeadZoneBegin;
            if (v == 0)
                return (0);
            v /= ParAll.ST.ZoneSize;
            return (Convert.ToInt32(Math.Ceiling(v)));
        }
        int CalcEndDeadZones()
        {
            double v = ParAll.ST.TSSet.Current.TSThick.DeadZoneEnd;
            if (v == 0)
                return (0);
            v /= ParAll.ST.ZoneSize;
            return (Convert.ToInt32(Math.Ceiling(v)));
        }
        void ComputeZones()
        {
            if (ZonesCalced.Count == 0)
                return;
            ZoneThick zLast = ZonesAll.LastOrDefault();
            bool IsLast = zLast == null ? false : zLast.Last;
            if (!IsLast)
            {
                ZoneThick Z = ZonesCalced[0];
                // Зона не должна попасть на последние мертвые зоны
                if (Z.Index + 1 + CalcEndDeadZones() >= ZonesAll.Count)
                    return;
                // Зона должна иметь следующую зону 
                if (Z.Index + 1 + 1 >= ZonesAll.Count)
                    return;
                Z.ComputeMedian(Z.Index == 0 ? null : ZonesAll[Z.Index - 1], ZonesAll[Z.Index + 1]);
                Z.ComputeDead();
                ZonesCalced.RemoveAt(0);
                ZonesComputed.Add(Z);
            }
            else
            {
                // Считаем в любом случае - дополнительных зон уже не будет никогда
                ZoneThick Z = ZonesCalced[0];
                ZoneThick prev = Z.Index == 0 ? null : ZonesAll[Z.Index - 1];
                ZoneThick next = Z.Index + 1 < ZonesAll.Count() ? ZonesAll[Z.Index + 1] : null;
                Z.ComputeMedian(prev,next);
                Z.ComputeDead(ZonesAll.Count());
                ZonesCalced.RemoveAt(0);
                ZonesComputed.Add(Z);
            }
        }

        public ZoneThick GetNextDataUnitZone(bool _check, int? _TubeLength, L_TickPosition _MTP)
        {
            if (IsComplete())
                return (null);
            NextZone(_TubeLength, _MTP);
            ComputeZones();
            ZoneThick PrevZone = ZonesAll.LastOrDefault();
            if (PrevZone != null)
            {
                if (PrevZone.Last)
                    GotZones = true;
            }
            if (ZonesComputed.Count == 0)
                return (null);
            ZoneThick z = ZonesComputed[0];
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
                if (z.Last)
                    complete = true;
                ZonesComputed.RemoveAt(0);
            }
            return (z);
        }
        void pr(string _msg)
        {
            ProtocolST.pr("BankTUnit: " + _msg);
        }
    }
}
