using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using PARLIB;

namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class L502Ch : ParBase
    {
        [DisplayName("Физический канал"), Browsable(true), De, Confirm]
        public int ChPhisical { get; set; }

        static string[] ranges = { "±10", "±5", "±2", "±1", "±0.5", "±0.2" };
        class RangeConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(
              ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < ranges.Length; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string v = value as string;
                for (int i = 0; i < ranges.Length; i++)
                {
                    if (ranges[i] == v)
                        return (i);
                }
                return (0);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return (ranges[Convert.ToInt32(value)]); }
        }
        int lrange;
        [DisplayName("Диапазон, В"), Browsable(true), De, Confirm]
        [TypeConverter(typeof(RangeConverter))]
        public int Range { get { return (lrange); } set { lrange = value < 6 ? value : 5; } }

        [DisplayName("В процентах"), Browsable(true), De, DefaultValue(true), Confirm]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsPersent { get; set; }

        static string[] modes = { "С общей землей", "Дифференциальный", "Измерение нуля" };
        class ModeConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < modes.Length; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string v = value as string;
                for (int i = 0; i < modes.Length; i++)
                {
                    if (modes[i] == v)
                        return (i);
                }
                return (0);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return (modes[Convert.ToInt32(value)]); }
        }

        [DisplayName("Режим"), Browsable(true), De, Confirm]
        [TypeConverter(typeof(ModeConverter))]
        public int Mode { get; set; }

        [DisplayName("Усреднение"), Browsable(true), De, Confirm]
        public int Avg { get; set; }

        [DisplayName("Множитель"), DefaultValue(1), Browsable(true), De, Confirm]
        public double Gain { get; set; }

        static string[] offsets = { "Ближе", "Дальше" };
        class OffsetConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < offsets.Length; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                string v = value as string;
                for (int i = 0; i < offsets.Length; i++)
                {
                    if (offsets[i] == v)
                        return (i);
                }
                return (0);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) { return (offsets[Convert.ToInt32(value)]); }
        }

        //[DisplayName("Линия"), Browsable(true), De]
        //[TypeConverter(typeof(OffsetConverter))]
        //public int Offset { get; set; }

        [DisplayName("Смещение"), Browsable(true), De, Confirm]
        public int IOffset { get; set; }

        public override string ToString()
        {
            string snn = null;
            if (PropertyIndex >= 0)
                snn = PropertyIndex.ToString() + ", ";
            snn +=
                ChPhisical.ToString() +
                ", " + ranges[Range] +
                ", " + modes[Mode] +
                ", " + Avg.ToString() +
                ", " + Gain.ToString(); 
            if (IOffset != 0)
                snn += ", " + IOffset.ToString();
            if(IsPersent)
                snn += ", %";
            return (snn);
        }

        double[] K = new double[6] { 10, 20, 50, 100, 200, 500 };
        [Browsable(false)]
        public double ValToPercent { get { return (K[Range]); } }
    }
}
