using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Xml.Serialization;

using PARLIB;


namespace UPAR.Def
{
    [Editor(typeof(FilterPars.Editor), typeof(UITypeEditor))]
    public class FilterPars : ParBase
    {
        [DisplayName("Использовать"), Browsable(true), De, Confirm]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsFilter { get; set; }

        [DisplayName("Тип"), Browsable(true), De, Confirm]
        [TypeConverter(typeof(CurrentTypeConverter))]
        public int CurrentType { get; set; }

        [DisplayName("Подтип"), Browsable(true), De, Confirm]
        [TypeConverter(typeof(CurrentSubTypeConverter))]
        public int CurrentSubType { get; set; }

        [DisplayName("Порядок"), Browsable(true), De, Confirm]
        [TypeConverter(typeof(OrderConverter))]
        public int Order { get; set; }

        [DisplayName("Частота среза"), Browsable(true), De, Confirm]
        public double CutoffFrequency { get; set; }

        [DisplayName("Центр фильтра"), Browsable(true), De, Confirm]
        public double CenterFrequency { get; set; }

        [DisplayName("Ширина фильтра"), Browsable(true), De, Confirm]
        public double WidthFrequency { get; set; }

        [DisplayName("Пульсации в полосе пропускания"), Browsable(true), Dynamic, De, Confirm]
        public double RippleDb { get; set; }

        [DisplayName("Пульсации в полосе подавления"), Browsable(true), Dynamic, De, Confirm]
        public double Rolloff { get; set; }

        class CurrentTypeConverter : Int32Converter
        {
            public static string[] modes = { "Баттерворта", "Чебышева", "Эллиптический" };
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return (new StandardValuesCollection(new List<int> { 0, 1, 2 }));
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

        class CurrentSubTypeConverter : Int32Converter
        {
            public static string[] modes = { "Низких частот", "Высоких частот", "Полосовой", "Заграждающий" };
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                return (new StandardValuesCollection(new List<int> { 0, 1, 2, 3 }));
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

        class OrderConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {
                List<int> L = new List<int>();
                for (int i = 0; i < 21; i++)
                    L.Add(i);
                return (new StandardValuesCollection(L));
            }
        }

        [XmlIgnore]
        public GridItem GI;

        class Editor : UITypeEditor
        {
            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return (UITypeEditorEditStyle.Modal); }
            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                FilterPars p = value as FilterPars;
                if (p == null)
                    return (value);
                p.GI = provider as GridItem;
                using (FFilter f = new FFilter(value as FilterPars, false)) { f.ShowDialog(); }
                p.GI = null;
                return (value);
            }
        }
        public bool IsBrowse(string _vname)
        {
            return (false);
        }
        public bool DynamicEnable(string _name)
        {
            bool ret = false;
            switch (_name)
            {
                //case "Частота среза":
                //    if (CurrentSubTypeConverter.modes[CurrentSubType] == "Полосовой" || CurrentSubTypeConverter.modes[CurrentSubType] == "Заграждающий")
                //        ret = true;
                //    break;
                //case "Центр фильтра":
                //    if (CurrentSubTypeConverter.modes[CurrentSubType] == "Низких частот" || CurrentSubTypeConverter.modes[CurrentSubType] == "Высоких частот")
                //        ret = true;
                //    break;
                //case "Ширина фильтра":
                //    if (CurrentSubTypeConverter.modes[CurrentSubType] == "Полосовой" || CurrentSubTypeConverter.modes[CurrentSubType] == "Заграждающий")
                //        ret = true;
                //    break;
                case "Пульсации в полосе пропускания":
                    if (CurrentTypeConverter.modes[CurrentType] == "Чебышева" || CurrentTypeConverter.modes[CurrentType] == "Эллиптический")
                        ret = true;
                    break;
                case "Пульсации в полосе подавления":
                    if (CurrentTypeConverter.modes[CurrentType] == "Эллиптический")
                        ret = true;
                    break;
            }
            return (ret);
        }
        public override string ToString()
        {
            if (!IsFilter)
                return ("Отключен");
            string ret = CurrentTypeConverter.modes[CurrentType] + " " + CurrentSubTypeConverter.modes[CurrentSubType] + ": ";
            ret += Order.ToString();
            if (DynamicEnable("Частота среза")) { ret += ";"; ret += CutoffFrequency.ToString(); }
            if (DynamicEnable("Центр фильтра")) { ret += ";"; ret += CenterFrequency.ToString(); }
            if (DynamicEnable("Ширина фильтра")) { ret += ";"; ret += WidthFrequency.ToString(); }
            if (DynamicEnable("Пульсации в полосе пропускания")) { ret += ";"; ret += RippleDb.ToString(); }
            if (DynamicEnable("Пульсации в полосе подавления")) { ret += ";"; ret += Rolloff.ToString(); }
            return (ret);
        }
        public void SetView()
        {
            foreach (PropertyInfo pi in GetType().GetProperties())
            {
                if (Attribute.GetCustomAttribute(pi, typeof(DynamicAttribute)) == null)
                    continue;

                PropertyDescriptor pd = TypeDescriptor.GetProperties(pi.DeclaringType)[pi.Name];
                BrowsableAttribute ba = (BrowsableAttribute)pd.Attributes[typeof(BrowsableAttribute)];
                FieldInfo isBrowsable = ba.GetType().GetField("Browsable", BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);

                DisplayNameAttribute nm = (DisplayNameAttribute)pd.Attributes[typeof(DisplayNameAttribute)];
                isBrowsable.SetValue(ba, DynamicEnable(nm.DisplayName));
            }
        }

        public void Load(System.IO.BinaryReader _br)
        {
            IsFilter = _br.ReadBoolean();
            CurrentType = _br.ReadInt32();
            CurrentSubType = _br.ReadInt32();
            Order = _br.ReadInt32();
            CutoffFrequency = _br.ReadDouble();
            WidthFrequency = _br.ReadDouble();
            CenterFrequency = _br.ReadDouble();
            RippleDb = _br.ReadDouble();
            Rolloff = _br.ReadDouble();
        }
        public void Save(System.IO.BinaryWriter _bw)
        {
            _bw.Write(IsFilter);
            _bw.Write(CurrentType);
            _bw.Write(CurrentSubType);
            _bw.Write(Order);
            _bw.Write(CutoffFrequency);
            _bw.Write(WidthFrequency);
            _bw.Write(CenterFrequency);
            _bw.Write(RippleDb);
            _bw.Write(Rolloff);
        }
        public FilterPars Clone()
        {
            FilterPars f = new FilterPars();
            f.Copy(this);
            return (f);
        }
        public void Copy(FilterPars _src)
        {
            IsFilter = _src.IsFilter;
            CurrentType = _src.CurrentType;
            CurrentSubType = _src.CurrentSubType;
            Order = _src.Order;
            CutoffFrequency = _src.CutoffFrequency;
            WidthFrequency = _src.WidthFrequency;
            CenterFrequency = _src.CenterFrequency;
            RippleDb = _src.RippleDb;
            Rolloff = _src.Rolloff;

        }
    }
}
