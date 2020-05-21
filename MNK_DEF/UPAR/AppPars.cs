using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Drawing.Design;
using PARLIB;

namespace UPAR
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class AppPars : ParBase
    {
        class PathFileEditor : FileNameEditor { protected override void InitializeDialog(OpenFileDialog openFileDialog) { openFileDialog.Filter = "ini файлы (*.exe)|*.exe"; } }

        [DisplayName("Путь"), Browsable(true), De]
        [EditorAttribute(typeof(PathFileEditor), typeof(UITypeEditor))]
        public string Path { get; set; }
    }
}
