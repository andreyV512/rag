using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using PARLIB;

namespace UPAR.Def

{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DefLineCh : ParBase
    {
        [DisplayName("Датчик 0"), Browsable(true), De]
        public L502Ch L502_0 { get; set; }
        [DisplayName("Датчик 1"), Browsable(true), De]
        public L502Ch L502_1 { get; set; }
        [DisplayName("Датчик 2"), Browsable(true), De]
        public L502Ch L502_2 { get; set; }
        [DisplayName("Датчик 3"), Browsable(true), De]
        public L502Ch L502_3 { get; set; }

        public override string ToString() { return (null); }
    }
}
