using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;

namespace PARLIB
{
    public abstract class DictionaryConverter : Int32Converter
    {
        protected Dictionary<int, string> D;
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(
          ITypeDescriptorContext context)
        {
            return (new StandardValuesCollection(D.Keys));
        }
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            string v = value as string;
            foreach (KeyValuePair<int, string> entry in D)
            {
                if (entry.Value == v)
                    return (entry.Key);
            }
            return (D.First().Key);
        }
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            string ret;
            return (D.TryGetValue(Convert.ToInt32(value), out ret) ? ret : D.First().Value);
        }
    }

}
