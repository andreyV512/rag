using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using UPAR;
using Share;

namespace ResultLib.Def
{
    public class Meas
    {
        public double Source = 0;
        public double Median = 0;
        public double Filter = 0;
        public double FilterIn = 0;
        public double FilterABC { get { return (Filter >= 0 ? Filter : -Filter); } }
        public double FilterInABC { get { return (FilterIn >= 0 ? FilterIn : -FilterIn); } }
        public EClass Class = EClass.None;
        public EClass ClassIn = EClass.None;
        public bool Dead = false;
    }
}
