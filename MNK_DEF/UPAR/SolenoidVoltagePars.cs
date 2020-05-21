using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR
{
    [DisplayName("Параметры контроля по напряжению")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SolenoidVoltagePars : ParBase
    {
        [DisplayName("Сопротивление, Ом"), DefaultValue(100), Browsable(true), De]
        public int ResistSolenoid { get; set; }

        [DisplayName("Напряжение, В"), DefaultValue(260), Browsable(true), De]
        public int VoltageSolenoid { get; set; }

        [DisplayName("+/- напряжения, В"), DefaultValue(100), Browsable(true), De]
        public int DifVoltageSolenoid { get; set; }

        public override string ToString() { return (""); }
    }
}
