using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;


namespace UPAR.Def
{
    [DisplayName("Концевые корректировки"), Browsable(true), De]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TailPars : ParBase
    {
        [DisplayName("Множитель в начале"), Browsable(true), De]
        public double MultStart { get; set; }

        [DisplayName("Длина в начале, mm"), Browsable(true), De]
        public int LenghtStart { get; set; }

        [DisplayName("Множитель в конце"), Browsable(true), De]
        public double MultEnd { get; set; }

        [DisplayName("Длина в конце, mm"), Browsable(true), De]
        public int LenghtEnd { get; set; }
    }
}
