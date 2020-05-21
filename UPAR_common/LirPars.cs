using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR_common
{
    public class LirPars : ParBase
    {
        [DisplayName("Плотность импульсов, 1/мм"), DefaultValue(10), Browsable(true), De]
        public double Density { get; set; }

        public override string ToString() { return (PropertyIndex.ToString()); }
    }
}
