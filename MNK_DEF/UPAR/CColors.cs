using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Design;

using PARLIB;
using UPAR_common;

namespace UPAR
{
    public class CColors : ParBase
    {
        [DisplayName("Класс 1"), Browsable(true), De]
        [TypeConverter(typeof(RColorConverter)), Editor(typeof(RColorEditor), typeof(UITypeEditor))]
        public Color Class1 { get; set; }

        [DisplayName("Класс 2"), Browsable(true), De]
        [TypeConverter(typeof(RColorConverter)), Editor(typeof(RColorEditor), typeof(UITypeEditor))]
        public Color Class2 { get; set; }

        [DisplayName("Брак"), Browsable(true), De]
        [TypeConverter(typeof(RColorConverter)), Editor(typeof(RColorEditor), typeof(UITypeEditor))]
        public Color Brak { get; set; }

        [DisplayName("Неизмерено"), Browsable(true), De]
        [TypeConverter(typeof(RColorConverter)), Editor(typeof(RColorEditor), typeof(UITypeEditor))]
        public Color NotMeasured { get; set; }

        [DisplayName("Годный участок"), Browsable(true), De]
        [TypeConverter(typeof(RColorConverter)), Editor(typeof(RColorEditor), typeof(UITypeEditor))]
        public Color GoodArea { get; set; }

        //[DisplayName("Алгоритм Общий"), Browsable(true), De]
        //public ColorsGroup EKE { get; set; }

        //[DisplayName("Алгоритм AB"), Browsable(true), De]
        //public ColorsGroup AB { get; set; }

        //[DisplayName("Алгоритм LV"), Browsable(true), De]
        //public ColorsGroup LV { get; set; }

        //[DisplayName("Выбранная зона"), Browsable(true), De]
        //[TypeConverter(typeof(RColorConverter)), Editor(typeof(RColorEditor), typeof(UITypeEditor))]
        //public Color SelectedZone { get; set; }

        //[DisplayName("Линия СОПА на графике"), Browsable(true), De]
        //[TypeConverter(typeof(RColorConverter)), Editor(typeof(RColorEditor), typeof(UITypeEditor))]
        //public Color SOPColor { get; set; }

        public override string ToString() { return (""); }

        public static Color ColorConvert(int _val)
        {
            int r = ((_val & 0xFF) >> 0);
            int g = ((_val & 0xFF00) >> 8);
            int b = ((_val & 0xFF0000) >> 16);
            Color ret = Color.FromArgb(r, g, b);
            return (ret);
        }
        public static int ColorConvert(Color _val)
        {
            int ret = _val.R;
            ret |= _val.G << 8;
            ret |= _val.B << 16;
            return (ret);
        }
        [Browsable(false)]
        public int eClass1 { get { return (ColorConvert(Class1)); } }

        [Browsable(false)]
        public int eClass2 { get { return (ColorConvert(Class2)); } }

        [Browsable(false)]
        public int eBrak { get { return (ColorConvert(Brak)); } }

        [Browsable(false)]
        public int eNotMeasured { get { return (ColorConvert(NotMeasured)); } }

        //[Browsable(false)]
        //public int eSelectedZone { get { return (ColorConvert(SelectedZone)); } }

        //[Browsable(false)]
        //public int eSOPColor { get { return (ColorConvert(SOPColor)); } }
    }
}
