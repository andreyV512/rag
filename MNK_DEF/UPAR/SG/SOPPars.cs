using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Globalization;
using System.Drawing.Design;
using PARLIB;

namespace UPAR.SG
{
    public class SOPPars: ParBase
    {
        [DisplayName("Наименование"), Browsable(true), De, NoCopy]
        public string Name { get; set; }

        [DisplayName("Длина, мм"), Browsable(true), De]
        public int Length { get; set; }

        [DisplayName("Начало, мм"), Browsable(true), De]
        public int BeginPoint { get; set; }

        [DisplayName("Конец, мм"), Browsable(true), De]
        public int EndPoint { get; set; }

        public override string ToString() { return (Name); }
    }
}
