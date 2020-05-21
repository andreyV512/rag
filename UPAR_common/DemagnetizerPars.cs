using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DemagnetizerPars : ParBase
    {
        [DisplayName("COM порт"), Browsable(true), De]
        public ComPortPars ComPort { get; set; }

        [DisplayName("Число повторений"), Browsable(true), DefaultValue(5), De]
        public int Iters { get; set; }

        [DisplayName("Протокол"), Browsable(true), DefaultValue(false), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool Verbose { get; set; }

        [DisplayName("Использовать"), Browsable(true), DefaultValue(false), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool InUse { get; set; }

        public override string ToString() { return (ComPort.ToString()); }
    }

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class DemagnetizerTSPars : ParBase
    {
        [DisplayName("Смещение, %"), Browsable(true), DefaultValue(0), De]
        [ConstraintInt(-90, 90), TypeConverter(typeof(ConstraintIntTypeConverter))]
        public int Offset { get; set; }


        [DisplayName("Частота, Гц"), Browsable(true), DefaultValue(15), De]
        [ConstraintInt(3, 50), TypeConverter(typeof(ConstraintIntTypeConverter))]
        public int Frequency { get; set; }

        public override string ToString() { return (string.Format("{0}%, {1}Гц", Offset.ToString(), Frequency.ToString())); }

    }
}
