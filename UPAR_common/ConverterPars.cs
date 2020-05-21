using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using PARLIB;

namespace UPAR_common
{
    [DisplayName("Частотный преобразователь"), Description("Параметры частотного преобразователя")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ConverterPars : ParBase
    {
        //[DisplayName("COM порт"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public ComPortPars ComPort { get; set; }

        [DisplayName("Абонент"), DefaultValue(1), Browsable(true), De]
        public int Abonent { get; set; }

//        [DisplayName("Частота, Гц"), Browsable(true), De]
//        public int Frequency { get; set; }

        class SpeedConverter : Int32Converter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(
              ITypeDescriptorContext context)
            {
                return (new StandardValuesCollection(new int[] { 4, 5, 6 }));
            }
        }
        [DisplayName("Номер регистра скорости"), TypeConverter(typeof(SpeedConverter)), DefaultValue(4), Browsable(true), De]
        public int SpeedPar { get; set; }

        //class TimeoutConverter : Int32Converter
        //{
        //    public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        //    public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        //    public override StandardValuesCollection GetStandardValues(
        //      ITypeDescriptorContext context)
        //    {
        //        List<int> L = new List<int>();
        //        for (int i = 0; i < 16; i++)
        //            L.Add(i);
        //        return (new StandardValuesCollection(L));
        //    }
        //}
//        [DisplayName("Время ожидания, мс"), DefaultValue(1), TypeConverter(typeof(TimeoutConverter)), Browsable(true), De]
//        public int Timeout { get; set; }

        public override string ToString() { return (Abonent.ToString() + ", " + SpeedPar.ToString()); }

        [DisplayName("Контролировать ABC"), DefaultValue(true), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsABC { get; set; }

        [DisplayName("Число повторений"), DefaultValue(3), Browsable(true), De]
        [ConstraintInt(1, 10), TypeConverter(typeof(ConstraintIntTypeConverter))]
        public int Iters { get; set; }
    }
}
