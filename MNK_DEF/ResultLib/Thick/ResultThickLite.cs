using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Protocol;
using BankLib;

namespace ResultLib.Thick
{
    public class ResultThickLite
    {
        //zone 
        public List<BankZoneThick> MZone = new List<BankZoneThick>();
        public double MaxThickness = 16;
        public double Border1 = 0;
        public double Border2 = 0;

        void pr(string _msg)
        {
            ProtocolST.pr("ThickResultLite: " + _msg);
        }
        public double? MinThickness
        {
            get
            {
                if (MZone.Count == 0)
                    return (null);
                double? ret = null;
                foreach (BankZoneThick bzt in MZone)
                {
                    if (ret == null)
                        ret = bzt.Level;
                    else if (bzt.Level != null && ret.Value > bzt.Level.Value)
                        ret = bzt.Level.Value;
                }
                return (ret);
            }
        }

    }
}
