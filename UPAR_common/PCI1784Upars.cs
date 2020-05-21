using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class PCI1784Upars : ParBase
    {
        [DisplayName("Номер платы"), DefaultValue(0), Browsable(true), De]
        public int Devnum { get; set; }

        [DisplayName("Период чтения, мс"), DefaultValue(0), Browsable(true), De]
        public int Period { get; set; }

        [DisplayName("Pазмер буфера событий"), DefaultValue(0), Browsable(true), De]
        public int EventSize { get; set; }

        [Browsable(true), De]
        public L_LirPars Lirs { get; set; }
    }
}
