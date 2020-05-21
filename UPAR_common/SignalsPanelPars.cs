using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR_common
{
    public class SignalsPanelPars: ParBase
    {
        [DisplayName("Ширина панели"), DefaultValue(100), Browsable(true), De]
        public int PanelWidth { get; set; }

        [DisplayName("Панели по умолчанию"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        public bool PanelsDefault { get; set; }

        [DisplayName("Показать"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        public bool Visible { get; set; }

        [DisplayName("Ручной режим"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        public bool Alive { get; set; }
    }
}
