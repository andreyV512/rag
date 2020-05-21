using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PARLIB;

namespace UPAR.TS.TSDef
{
    [DisplayName("Датчики признака типоразмера")]
    [Browsable(true), De]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TSSGSensors : ParBase
    {
        [DisplayName("Датчик 0"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool D0 { get; set; }

        [DisplayName("Датчик 1"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool D1 { get; set; }

        [DisplayName("Датчик 2"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool D2 { get; set; }

        public override string ToString()
        {
            return string.Format("{0}{1}{2}",
                D0 ? "1" : ".",
                D1 ? "1" : ".",
                D2 ? "1" : ".");
        }
        [Browsable(false)]
        public bool[] Sensors { get { return (new bool[3] { D0, D1, D2 }); } }
    }
}
