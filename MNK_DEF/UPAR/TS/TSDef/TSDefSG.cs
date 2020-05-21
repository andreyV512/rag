using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR.TS.TSDef
{
    [DisplayName("Группа прочности")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TSDefSG : ParBase
    {
        [DisplayName("Частота, кГц"), Browsable(true), DefaultValue(270.0), De]
        public double Frequency { get; set; }
         
        [DisplayName("Амплитуда, В"), Browsable(true), DefaultValue(2.0), De]
        public double Voltage { get; set; }

        [Browsable(true), De]
        public TSSGSensors Sensors { get; set; }

        public override string ToString() { return (null); }
    }
    public class SGWorkPars
    {
        [DisplayName("Частота генератора, кГц")]
        public double Frequency { get; set; }

        [DisplayName("Амплитуда генератора, В")]
        public double Voltage { get; set; }

        [DisplayName("Время сбора СОП, с")]
        public int SOPPeriod { get; set; }

        [DisplayName("Показывать график сигнала")]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsGraph { get; set; }

        public SGWorkPars()
        {
            Frequency = ParAll.CTS.Cross.SG.Frequency;
            Voltage = ParAll.CTS.Cross.SG.Voltage;
            SOPPeriod = ParAll.SG.SOPPeriod;
            IsGraph = ParAll.SG.IsGraph;
        }
        public void Save()
        {
            ParAll.CTS.Cross.SG.Frequency = Frequency;
            ParAll.CTS.Cross.SG.Voltage = Voltage;
            ParAll.SG.SOPPeriod = SOPPeriod;
            ParAll.SG.IsGraph = IsGraph;
        }
    }

}
