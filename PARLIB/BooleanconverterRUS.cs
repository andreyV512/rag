using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PARLIB
{
    public class BooleanconverterRUS : BooleanConverter
    {
        private readonly string trueString = "Да";
        private readonly string falseString = "Нет";
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
}
