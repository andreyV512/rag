using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using PARLIB;

namespace UPAR.SG
{
    [DisplayName("Параметры")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SGPars : ParBase
    {
        [DisplayName("Период"), DefaultValue(600),Browsable(true), De]
        public int HalfPeriod {get;set;}

        [DisplayName("Период +/-"), DefaultValue(40),Browsable(true), De]
        public int HalfPeriodDif { get; set; }

        [DisplayName("Мертвая зона в начале, %"), DefaultValue(15),Browsable(true), De]
        public int BorderBegin { get; set; }

        [DisplayName("Мертвая зона в конце, %"), DefaultValue(15),Browsable(true), De]
        public int BorderEnd { get; set; }

        public enum EStartPoint
        {
            [Description("U - +")]
            UH,
            [Description("U + -")]
            UL,
            [Description("I - +")]
            IH,
            [Description("I + -")]
            IL
        }

        [DisplayName("Начало периода"), Browsable(true), De]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EStartPoint AlgorithmPoints { get; set; }

        public enum EValIU
        {
            [Description("U")]
            U,
            [Description("I")]
            I,
            [Description("U-I")]
            UmI,
            [Description("I-U")]
            ImU
        }

        [DisplayName("Полный период"), Browsable(true), De, DefaultValue(false)]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool FullPeriod { get; set; }
                
        [DisplayName("Значение"), Browsable(true), De,DefaultValue(EValIU.U)]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EValIU ValIU { get; set; }
        
        class AlgorithmSGConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(
              ITypeDescriptorContext context)
            {
                List<string> L = new List<string>();
                L.Add("Корреляция");
                L.Add("Расстояние");
                L.Add("Вероятность");
                return (new StandardValuesCollection(L));
            }
        }
        [DisplayName("Алгоритм ГП"), DefaultValue("Корреляция")]
        [TypeConverter(typeof(AlgorithmSGConverter)),Browsable(true), De]
        public string AlgorithmSG { get; set; }

        [DisplayName("Достоверность"), DefaultValue(75), Browsable(true), De]
        public int Veracity { get; set; }

        [DisplayName("Количество труб"), DefaultValue(50), Browsable(true), De]
        public int TubeCount { get; set; }

        [DisplayName("СОП-ы"), Browsable(true), De]
        public L_SOP SOPs { get; set; }
    }
}
