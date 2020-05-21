using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PARLIB
{
    public class ProtocolPar : ParBase
    {
        [DisplayName("Писать в файл"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsFile { get; set; }

        [DisplayName("Помнить строки"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsSave { get; set; }

        [DisplayName("Показать"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsVisible { get;set;}

        [DisplayName("Период обновления, мс"), Browsable(true), De]
        public int Period { get; set; }

        class ByDayConverter : BooleanConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value != null && value is string)
                {
                    if ((string)value == "По дням") return true;
                    if ((string)value == "По запускам") return false;
                }
                return base.ConvertFrom(context, culture, value);
            }
            public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string) && value != null && value is bool)
                {
                    if ((bool)value == true) return("По дням");
                    if ((bool)value == false) return ("По запускам");
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        [DisplayName("Файлы"), Browsable(true), De]
        [TypeConverter(typeof(ByDayConverter))]
        public bool ByDay { get; set; }

    }
}
