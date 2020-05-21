using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UPAR
{
    public class cIW
    {
        public bool Cross { get; set; }
        public bool Line { get; set; }
        public bool Thick { get; set; }
        public bool SG { get; set; }

        public cIW(bool _load=false)
        {
            if (_load)
            {
                Cross = ParAll.ST.Defect.Cross.IsWork;
                Line = ParAll.ST.Defect.Line.IsWork;
                Thick = ParAll.ST.Defect.Thick.IsWork;
                SG = ParAll.ST.Defect.Cross.SolidGroup.IsWork;
            }
        }
    }
}
