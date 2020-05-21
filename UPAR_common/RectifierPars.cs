using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR_common
{
    public enum EIU
    {
        [Description("По току")]
        ByI,
        [Description("По напряжению")]
        ByU
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class RectifierPars : ParBase
    {
        [DisplayName("Стабилизация"), Browsable(true), DefaultValue(EIU.ByI), De, Confirm]
        [TypeConverter(typeof(EnumTypeConverter))]
        public EIU TpIU { get; set; }

        [DisplayName("Длительность работы, с"), DefaultValue(45), Browsable(true), De, Confirm]
        public int Timeout { get; set; }

        [DisplayName("Требуемый ток, А"), Browsable(true), DefaultValue(1.0), De, Confirm]
        public double NominalI { get; set; }

        [DisplayName("Требуемое напряжение, В"), Browsable(true), DefaultValue(220.0), De, Confirm]
        public double NominalU { get; set; }

        [DisplayName("Максимальный ток, А"), Browsable(true), DefaultValue(1.0), De, Confirm]
        public double MaxI { get; set; }

        [DisplayName("Максимальное напряжение, В"), Browsable(true), DefaultValue(260.0), De, Confirm]
        public double MaxU { get; set; }

        [DisplayName("Сопротивления перегрева, Ом"), Browsable(true), De, Confirm]
        public double MaxR { get; set; }

        [DisplayName("Период опроса, мс"), DefaultValue(1000), Browsable(true), De, Confirm]
        public int Period { get; set; }

        public override string ToString()
        {
            string ret = string.Format("{0}c, ", Timeout.ToString());
            switch (TpIU)
            {
                case EIU.ByI:
                    ret+=string.Format("{0}А, макс: {1}В",NominalI.ToString(),MaxU.ToString());
                    break;
                case EIU.ByU:
                    ret+=string.Format("{0}В, макс: {1}А",NominalU.ToString(),MaxI.ToString());
                    break;
            }
            return ret;
        }
    }
}
