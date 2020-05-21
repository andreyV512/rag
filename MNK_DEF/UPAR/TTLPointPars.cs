using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class TTLPointPars : ParBase
    {
        [DisplayName("Бит"), Browsable(true), De]
        public int Position { get; set; }

        public override string ToString()
        {
            return ("Бит " + Position.ToString());
        }
    }
}
