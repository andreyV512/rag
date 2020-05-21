using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TTLPars : ParBase
    {
        [DisplayName("Частота сбора TTL, Гц"), Browsable(true), De, DefaultValue(1000.0)]
        public double Frequency { get; set; }
        
        [DisplayName("Минимальный период сигнала, мс"), Browsable(true), De]
        public double Timeout { get; set; }

        [DisplayName("Контроль поперечный"), Browsable(true), De]
        public TTLPointPars CControl { get; set; }

        [DisplayName("Контроль продольный"), Browsable(true), De]
        public TTLPointPars LControl { get; set; }

        [DisplayName("Контроль толщиномер"), Browsable(true), De]
        public TTLPointPars TControl { get; set; }

        [DisplayName("Строб"), Browsable(true), De]
        public TTLPointPars Strobe { get; set; }
    }
}
