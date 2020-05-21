using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using PARLIB;

namespace UPAR_common
{
    [TypeConverter(typeof(CollectionTypeConverter)), Editor(typeof(FLBase.Editor), typeof(UITypeEditor))]
    [DisplayName("ЛИРы")]
    public class L_LirPars:ParListBase<LirPars>{} 
}
