using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using PARLIB;

namespace UPAR.Def
{
    [DisplayName("СОП-ы")]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SOPPars : ParBase
    {
        [DisplayName("Проверочный СОП"), Browsable(true), De]
        public int SOPCheckID { get; set; }

        [DisplayName("Просмотр СОП"), TypeConverter(typeof(BooleanconverterRUS)), DefaultValue(true), Browsable(true), De]
        public bool ViewEtalonCheck { get; set; }

        [DisplayName("Каталог СОП-ов")]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor)), Browsable(true), De]
        public string SOPPath { get; set; }

        [DisplayName("СОП для показа на графике"), Browsable(true), De, DefaultValue("")]
        public string SOPPaint { get; set; }

        public override string ToString() { return (""); }
    }
}
