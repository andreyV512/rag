using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR.SG
{
    [DisplayName("Таблица"), Browsable(true), De]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class GridPars : ParBase
    {
        [DisplayName("Наименование"), Browsable(true), De]
        public string Name { get; set; }

        [DisplayName("Столбцы"), Browsable(true), De]
        public L_ColumnPars Columns { get; set; }

        public override string ToString() { return (Name); }
    }
}
