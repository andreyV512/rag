using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Globalization;

using PARLIB;
using UPAR.TS.TSDef;
using UPAR.Def;
using UPAR_common;


namespace UPAR.TS
{
    public class TypeSize : ParBase
    {
        [DisplayName("Наименование"), NoCopy, Browsable(true), De, Confirm]
        public string Name { get; set; }

        [Browsable(true), De]
        public TSCross Cross { get; set; }

        [Browsable(true), De]
        public TSLine Line { get; set; }

        [DisplayName("Минимально годная длина, мм"), Browsable(true), De, Confirm]
        public int MinGoodLength { get; set; }

        [DisplayName("Размагничиватель"), Browsable(true), De]
        public DemagnetizerTSPars DemagnetizerTS { get; set; }

        public override string ToString()
        {
            if (Name == null || Name.Length == 0)
                return ("?");
            return (Name);
        }
    }
}