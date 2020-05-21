using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RectifierNS
{
    public class UIT
    {
        public double U = 0;
        public double I = 0;
        public int Sec = 0;
        public int Min = 0;
        public int Hour = 0;
        public double R { get { return (I == 0 ? 0 : (U / I)); } }
        public TimeSpan Ts { get { return (new TimeSpan(Hour, Min, Sec)); } }
        public string error = null;
        public UIT() { }
        public UIT(UIT _src)
        {
            if (_src == null)
                return;
            U = _src.U;
            I = _src.I;
            Sec = _src.Sec;
            Min = _src.Min;
            Hour = _src.Hour;
            error = _src.error;
        }
        public override string ToString()
        {
            return (string.Format("U={0} I={1} R={2} T={3}:{4}:{5}",
                U.ToString("F3"),
                I.ToString("F3"),
                R.ToString("F3"),
                Hour.ToString("00"),
                Min.ToString("00"),
                Sec.ToString("00")
                ));
        }
    }
    public class UITH : UIT
    {
        public bool OverHeat { get; private set; }
        public UITH() { }
        public UITH(UIT _src, double _RMax)
            : base(_src)
        {
            OverHeat = R > _RMax;
        }
        public UITH(UITH _src)
            : base(_src)
        {
            OverHeat = _src.OverHeat;
        }
        public bool IsOk
        {
            get
            {
                if (U == 0 && I == 0)
                    return (false);
                return (!OverHeat);
            }
        }
        public override string ToString()
        {
            return(base.ToString()+" OK="+(IsOk?"true":"false"));
        }
    }
}
