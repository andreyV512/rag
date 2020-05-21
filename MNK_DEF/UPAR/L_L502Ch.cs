using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using PARLIB;

namespace UPAR
{
    [TypeConverter(typeof(CollectionTypeConverter)), Editor(typeof(FLBase.Editor), typeof(UITypeEditor))]
    public class L_L502Ch : ParListBase<L502Ch> { }
}
