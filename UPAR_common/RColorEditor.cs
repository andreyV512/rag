using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms;

namespace UPAR_common
{
    public class RColorEditor : ColorEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) { return (UITypeEditorEditStyle.Modal); }
        public override bool GetPaintValueSupported(ITypeDescriptorContext context) { return (true); }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            ColorDialog diag = new ColorDialog();
            diag.Color = (Color)value;
            if (diag.ShowDialog() == DialogResult.OK)
                value = diag.Color;
            return (value);
        }
    }
}
