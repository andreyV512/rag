using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Defect.SG.ColorPicker
{
    class ColorColumn : DataGridViewColumn
    {
        public ColorColumn()
            : base(new ColorCell())
        {
            ValueType = typeof(Color);
        }
    }
}
