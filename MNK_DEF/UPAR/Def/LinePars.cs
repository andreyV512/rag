using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PARLIB;
using UPAR_common;

namespace UPAR.Def
{
    [DisplayName("Продольный"), TypeConverter(typeof(ExpandableObjectConverter))]
    public class LinePars : ParBase
    {
        [DisplayName("Работа"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsWork { get; set; }

        [DisplayName("Фильтр внешний"), Browsable(true), De]
        public FilterPars Filter { get; set; }

        [DisplayName("Фильтр внутренний"), Browsable(true), De]
        public FilterPars FilterIn { get; set; }

        [DisplayName("Выпрямитель"), Browsable(true), De]
        public RectifiersPars Rectifiers { get; set; }

        [DisplayName("Размер буфера сбора, мб"), Browsable(true), De]
        public int Buffer { get; set; }

        [DisplayName("Концевые корректировки"), Browsable(true), De]
        public TailPars Tails { get; set; }

        [DisplayName("LCard502E"), Browsable(true), De]
        public LCard502Pars L502 { get; set; }

        [DisplayName("COM порт частотных преобразователей"), Browsable(true), De]
        public ComPortPars ComPortConverters { get; set; }

        [Browsable(true), De]
        public ConverterPars Converter { get; set; }
    }
}
