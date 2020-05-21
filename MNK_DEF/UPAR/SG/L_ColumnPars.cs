using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR.SG
{
    [DisplayName("Столбцы")]
    public class L_ColumnPars : ParListBase<ColumnPars>
    {
        public new ColumnPars this[string _name]
        {
            get
            {
                foreach (ColumnPars p in this)
                {
                    if (p.Name == _name)
                        return (p);
                }
                return (null);
            }
        }
        public override object AddNew()
        {
            ColumnPars p = base.AddNew() as ColumnPars;
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
