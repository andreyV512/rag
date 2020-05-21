using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;

using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Save1730Pars : ParBase
    {
        class SignalListFileEditor : FileNameEditor
        {
            protected override void InitializeDialog(OpenFileDialog openFileDialog)
            {
                openFileDialog.Filter = "bak файлы (*.bak)|*.bak|Все файлы (*.*)|*.*";
                openFileDialog.CheckFileExists = false;
            }
        }

        [DisplayName("Записывать в файл"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool IsSave { get; set; }

        [DisplayName("Имя файла"), Browsable(true), De]
        [EditorAttribute(typeof(SignalListFileEditor), typeof(UITypeEditor))]
        public string FileName { get; set; }

        [DisplayName("Размер буфера, Мб"), Browsable(true), De]
        public int Size { get; set; }
    }
}
