using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using PARLIB;

namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SignalViewPars : ParBase
    {
        [DisplayName("Показывать"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool View { get; set; }

        [DisplayName("Цвет"), Browsable(true), De]
        public Color SColor { get; set; }
    }
}
