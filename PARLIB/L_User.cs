using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;


namespace PARLIB
{
    [DisplayName("Пользователи")]
    [Sortable]
    public class L_User : ParListBase<User>
    {
        public new User this[string _name]
        {
            get
            {
                foreach (User p in this)
                {
                    if (p.Name == _name)
                        return (p);
                }
                return (null);
            }
        }
        public override object AddNew()
        {

            User p = base.AddNew() as User;
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
        public User CurrentUser
        {
            get
            {
                return (User.current);
            }
        }
    }
}
