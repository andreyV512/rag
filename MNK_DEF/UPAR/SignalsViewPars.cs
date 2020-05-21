using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using PARLIB;

namespace UPAR
{
    [DisplayName("Графики сигналов")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SignalsViewPars : ParBase
    {
        [DisplayName("Исходный"), Browsable(true), De]
        public SignalViewPars Source { get; set; }

        [DisplayName("Фильтрованный"), Browsable(true), De]
        public SignalViewPars Filter { get; set; }

        [DisplayName("Фильтрованный внутренний"), Browsable(true), De]
        public SignalViewPars FilterIn { get; set; }

        [DisplayName("Медианный"), Browsable(true), De]
        public SignalViewPars Median { get; set; }

        [DisplayName("Цвет разделителя зон"), Browsable(true), De]
        public Color DeviderColor { get; set; }

    }
}
