using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Globalization;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Reflection;
using PARLIB;


namespace UPAR.TS
{
//    [TypeConverter(typeof(CollectionTypeConverter)),Editor(typeof(FLBase_Editor), typeof(UITypeEditor))]
    [Sortable]
    public class L_TypeSize : ParListBase<TypeSize>
    {
        public TypeSize Current { get; set; }
        public override string ToString() { return (Current!=null?Current.Name:null); }
        public override object AddNew()
        {

            TypeSize p = base.AddNew() as TypeSize;
            p.Name = FindNewName();
            return (p);
        }
        string FindNewName()
        {
            for (int i = 0; ; i++)
            {
                if (this["Новый" + i.ToString()] == null)
                    return ("Новый" + i.ToString());
            }
        }
        public override TypeSize this[string _val]
        {
            get
            {
                foreach (TypeSize p in this)
                {
                    if (p.Name == _val)
                        return (p);
                }
                return (null);
            }
        }
        public TypeSize RCopy(TypeSize _src, string _new_name)
        {
            TypeSize p = AddNew() as TypeSize;
            parMainLite.MP.Copy(_src, p);
            p.Name = _new_name;
            return (p);
        }
    }
}
