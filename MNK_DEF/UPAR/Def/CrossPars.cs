using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PARLIB;
using UPAR_common;
using UPAR.SG;

namespace UPAR.Def
{
    [DisplayName("Поперечный"), TypeConverter(typeof(ExpandableObjectConverter))]
    public class CrossPars : ParBase
    {
        [DisplayName("Работа"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsWork { get; set; }

        [DisplayName("Фильтр"), Browsable(true), De]
        public FilterPars Filter { get; set; }

        [DisplayName("Размер буфера сбора, мб"), Browsable(true), De]
        public int Buffer { get; set; }

        [DisplayName("Концевые корректировки"), Browsable(true), De]
        public TailPars Tails { get; set; }

        [DisplayName("Выпрямитель"), Browsable(true), De]
        public RectifiersPars Rectifiers { get; set; }

        [DisplayName("LCard502"), Browsable(true), De]
        public LCard502Pars L502 { get; set; }

        [DisplayName("Группа прочности"), Browsable(true), De]
        public SolidGroupPars SolidGroup { get; set; }

    }
}
