using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;


namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SolenoidVPars : ParBase
    {
        [DisplayName("Датчик напряжения")]
        [TypeConverter(typeof(ExpandableObjectConverter)), Browsable(true), De]
        public L502Ch Sensor_U { get; set; }

        [DisplayName("Минимально допустимое напряжение, В"), DefaultValue(260), Browsable(true), De]
        public double VoltageSolenoid { get; set; }

        public override string ToString() { return ("> " + VoltageSolenoid.ToString() + " В"); }
    }
}
