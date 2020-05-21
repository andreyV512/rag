using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using UPAR.Def;

namespace BankLib
{
    public class BankUnit
    {
        public EUnit Tp = EUnit.None;
        public bool isStarted = false;
        public bool complete = false;

        public BankUnit(EUnit _Tp)
        {
            Tp = _Tp;

            Clear();
        }

        protected void Clear()
        {
            isStarted = false;
            complete = false;
        }

        public bool IsComplete()
        {
            if (!isStarted)
                return (true);
            return (complete);
        }

        public override string ToString()
        {
            return ("BankUnit[" + Current.EUnitToString(Tp) + "]");
        }
    }
    public class BankDataUnit : BankUnit
    {
        public int deadEnd = 0;
        public int Count { get; private set; }
        public double[] data = null;
        public double? firstTick = null;
        public int Sensors = 0;
        public BankDataUnit(EUnit _type)
            : base(_type)
        {
            Clear();
        }

        new void Clear()
        {
            base.Clear();
            firstTick = 0;
            Count = 0;
        }

        public void ReSize(int _size)
        {
            if (data == null)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                data = new double[_size];
            }
            else
            {
                if (data.Length != _size)
                {
                    data = null;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    data = new double[_size];
                }
            }
            Clear();
        }
        public void Add(double[] _data, int _offset, int _size)
        {
            if (!isStarted)
                return;
            if (_data.Length <= 0)
                return;
            int packets = _data.Length / _size;
            int lLength = packets * Sensors;

            if (Count + lLength > data.Length)
                throw (new Exception(string.Format("BankDataUnit.Add: Превышен размер буфера: {0}: count={1} + _size={2} > size={3}",
                    Current.EUnitToString(Tp),
                    Count.ToString(),
                    lLength.ToString(),
                    data.Length.ToString())));
            int i = 0;
            for (int p = 0; p < packets; p++)
            {
                for (int s = 0; s < Sensors; s++)
                {
                    data[Count + i] = _data[p*_size+_offset+s];
                    i++;
                }
            }
            Count += lLength;
        }
        public void SetDeadEnd(int _ZoneLength, int _dead)
        {
            int ldeadSize = _dead + _ZoneLength;
            int ldeadEnd = ldeadSize / _ZoneLength;
            if (ldeadEnd * _ZoneLength < ldeadSize)
                ldeadEnd++;
            if (deadEnd < ldeadEnd)
                deadEnd = ldeadEnd;
        }
        public override string ToString()
        {
            return (base.ToString() + "count=" + Count);
        }
        public int GetRealSize()
        {
            return (Count * sizeof(double));
        }
        //public double BtCalc()
        //{
        //    get
        //    {
        //        double ret = lastTick - firstTick.Value;
        //        ret *= Sensors;
        //        return (ret);
        //    }
        //}
    }
}
