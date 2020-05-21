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
    [DisplayName("Платы PCIE1730")]
    public class L_PCIE1730pars : ParListBase<PCIE1730pars>
    {
        public override string ToString() { return (string.Format("<{0}>", Count)); }
        public override object AddNew()
        {
            PCIE1730pars p = base.AddNew() as PCIE1730pars;
            p.Devnum = FindNewDevnum();
            return (p);
        }

        private int FindNewDevnum()
        {
            int devnum = -1;
            foreach (PCIE1730pars p in this)
            {
                if (p.Devnum >= devnum)
                    devnum = p.Devnum;
            }
            return (devnum+1);
        }
    }
}
