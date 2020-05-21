using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR
{
    [DisplayName("Параметры контроля по температуре")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SolenoidTemperaturePars : ParBase
    {
        [DisplayName("Начальная температура, °С"), DefaultValue(20.0), Browsable(true), De]
        public double TempStart { get; set; }

        [DisplayName("Начальное сопротивление"), DefaultValue(68.4), Browsable(true), De]
        public double ResistStart { get; set; }

        [DisplayName("Коэффициент TR"), DefaultValue(0.0038), Browsable(true), De]
        public double TRCoef { get; set; }

        [DisplayName("Температура перегрева, °С"), DefaultValue(110.0), Browsable(true), De]
        public double AlarmTemp { get; set; }

        public override string ToString() { return (""); }
    }
}
