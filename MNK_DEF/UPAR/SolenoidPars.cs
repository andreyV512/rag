using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using PARLIB;

namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SolenoidPars : ParBase
    {
        [DisplayName("Датчик тока соленоида"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public L502Ch Sensor_I { get; set; }

        [DisplayName("Датчик напряжения соленоида")]
        [TypeConverter(typeof(ExpandableObjectConverter)), Browsable(true), De]
        public L502Ch Sensor_U { get; set; }

        class TempConverter : BooleanConverter
        {
            private readonly string trueString = "По температуре";
            private readonly string falseString = "По сопротивлению";
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value != null && value is string)
                {
                    if ((string)value == trueString) return true;
                    if ((string)value == falseString) return false;
                }
                return base.ConvertFrom(context, culture, value);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value != null && value is bool)
                {
                    if ((bool)value == true) return trueString;
                    if ((bool)value == false) return falseString;
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
        [DisplayName("Тип контроля"), DefaultValue(false), Browsable(true), De]
        [TypeConverter(typeof(TempConverter))]
        public bool ByTemp { get; set; }

        [Browsable(true), De]
        public SolenoidVoltagePars SolVoltage { get; set; }

        [Browsable(true), De]
        public SolenoidTemperaturePars SolTemperaturePars { get; set; }

        [DisplayName("Делитель напряжения"), DefaultValue(75), Browsable(true), De]
        public double Devider { get; set; }

        public override string ToString() { return (ByTemp ? "По температуре" : "По сопротивлению"); }
    }
}
