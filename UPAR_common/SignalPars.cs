using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;
using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(RExpandableObjectConverter))]
    public class SignalPars : ParBase
    {
        [DisplayName("Наименование"), NoCopy, Browsable(true), De]
        public string Name { get; set; }

        [DisplayName("Входящий"), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool Input { get; set; }

        [DisplayName("Цифровой"), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool Digital { get; set; }

        [DisplayName("Номер канала"), NoCopy, Browsable(true), De]
        public int Position { get; set; }

        [DisplayName("Описание"), Browsable(true), De]
        public string Hint { get; set; }

        [DisplayName("Ошибка On"), Browsable(true), De]
        public string EOn { get; set; }

        [DisplayName("Ошибка Off"), Browsable(true), De]
        public string EOff { get; set; }

        [DisplayName("Задержка, мс"), Browsable(true), De]
        public int Timeout { get; set; }

        [DisplayName("Не снимать"), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool NoReset { get; set; }

        [DisplayName("Вербальный"), TypeConverter(typeof(BooleanconverterRUS)), Browsable(true), De]
        public bool Verbal { get; set; }

        [DisplayName("X"), Browsable(true), De]
        public int X { get; set; }

        [DisplayName("Y"), Browsable(true), De]
        public int Y { get; set; }

        [DisplayName("История"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool FIFOEnable { get; set; }

        class BooleanconverterRUS : BooleanConverter
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

        public override string ToString()
        {
            string ret = Position.ToString();
            ret += " ";
            ret += Input ? "Вх" : "Вых";
            ret += Digital ? " Ц" : "";
            ret += " ";
            ret += Name;
            return (ret);
        }
        public List<string> ToCS()
        {
            List<string> L= new List<string>();
            L.Add("[DName(\""+Name+"\")]");
            L.Add("public Signal " + (Input ? "i" : "o") + ";");
            return (L);
        }
    }
}
