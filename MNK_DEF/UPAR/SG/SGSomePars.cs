using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR.SG
{
    [DisplayName("Разное"),  Browsable(true), De]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SGSomePars : ParBase
    {
        [DisplayName("Разделитель главной формы"), Browsable(true), De]
        public int FMain_SplitterDistance { get; set; }

        [DisplayName("Таблицы"), Browsable(true), De]
        public L_GridPars Grids { get; set; }
    }
}
