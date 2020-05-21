using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR.SG
{
    [DisplayName("Столбец"), Browsable(true), De]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ColumnPars: ParBase
    {
        [DisplayName("Наименование"), Browsable(true), De]
        public string Name { get; set; }

        [DisplayName("Ширина"), Browsable(true), De]
        public int Width { get; set; }

        public override string ToString() { return (Name); }
    }
}
