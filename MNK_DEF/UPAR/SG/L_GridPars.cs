using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using PARLIB;

namespace UPAR.SG
{
    [DisplayName("Столбцы")]
    public class L_GridPars: ParListBase<GridPars>
    {
        public new GridPars this[string _name]
        {
            get
            {
                foreach (GridPars p in this)
                {
                    if (p.Name == _name)
                        return (p);
                }
                return (null);
            }
        }
        public override object AddNew()
        {

            GridPars p = base.AddNew() as GridPars;
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
