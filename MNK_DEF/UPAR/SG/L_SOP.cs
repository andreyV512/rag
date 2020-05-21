using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR.SG
{
    public class L_SOP: ParListBase<SOPPars>
    {
        public new SOPPars this[string _name]
        {
            get
            {
                foreach (SOPPars p in this)
                {
                    if (p.Name == _name)
                        return (p);
                }
                return (null);
            }
        }
        public override object AddNew()
        {
            SOPPars p = base.AddNew() as SOPPars;
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
    }
}
