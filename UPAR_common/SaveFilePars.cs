using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms.Design;
//using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Reflection;
using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(RExpandableObjectConverter))]
    public class SaveFilePars : ParBase
    {
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor)), DisplayName("Каталог сохранения труб"), Browsable(true), De]
        [DefaultValue(null)]
        public string Path { get; set; }
        
        [DisplayName("Максимальное количество сохраненных труб в каталоге"), Browsable(true), DefaultValue(null), De]
        public int? MaxNumber { get; set; }
    }
}
