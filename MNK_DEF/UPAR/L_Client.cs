using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;

using PARLIB;

namespace UPAR
{
    [DisplayName("Заказчики")]
    [Sortable]
    public class L_Client : ParListBase<Client>
    {
        public Client Current { get; set; }
        public override string ToString() { return (Current != null ? Current.Name : null); }
        public new Client this[string _name]
        {
            get
            {
                foreach (Client p in this)
                {
                    if (p.Name == _name)
                        return (p);
                }
                return (null);
            }
        }
        public override object AddNew()
        {
            Client p = base.AddNew() as Client;
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
