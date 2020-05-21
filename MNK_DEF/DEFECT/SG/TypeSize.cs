using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;

using SQL;
using Protocol;
using UPAR;
using UPAR.TS;

namespace Defect.SG
{
    public class TypeSize : BaseItem
    {
        [DisplayName("Наименование")]
        public string Name { get { return (ownKey.TSName); } set { ownKey.TSName = value; } }

        DBKey ownKey = null;

        public static string Title = "Типоразмеры";
        public static void LoadKey(BaseDBKey _Key, BindingSource _bs)
        {
            Adjust();
            _bs.Clear();
            Select S = new Select(string.Format("SELECT * from {0}.SGTypeSizes order by name", BaseItem.Schema));
            while (S.Read())
                _bs.Add(new TypeSize(S["name"] as string));
            S.Dispose();
        }
        public static bool InsertKey(BaseDBKey _Key, BindingSource _bs)
        {
            string new_name = FindNewName(_bs);
            if (new ExecSQLX(string.Format("insert into {0}.SGTypeSizes values('{1}')", BaseItem.Schema, new_name)).Exec() == 1)
            {
                _bs.Position = _bs.Add(new TypeSize(new_name));
                return (true);
            }
            return (false);
        }

        string saved_name = "";
        TypeSize(string _name)
        {
            ownKey = new DBKey(_name);
            saved_name = Name;
        }
        public override string ToString() { return ("Типоразмер=" + Name); }
        public override bool Update()
        {
            pr("TypeSize.Update()");
            bool ret = new ExecSQLX(string.Format("update {0}.SGTypeSizes set name='{1}' where name='{2}'", BaseItem.Schema, Name, saved_name)).Exec() == 1;
            if (ret)
                saved_name = Name;
            else
                Name = saved_name;
            return (ret);
        }
        public override bool Delete()
        {
            return (new ExecSQLX(string.Format("delete from {0}.SGTypeSizes where name='{1}'", BaseItem.Schema, saved_name)).Exec() == 1);
        }
        static string FindNewName(System.Windows.Forms.BindingSource _bs)
        {
            string head = "Новый";
            string ret;
            for (int i = 0; ; i++)
            {
                ret = head + (i == 0 ? "" : i.ToString());
                bool check = false;
                foreach (TypeSize ts in _bs)
                {
                    if (ts.Name == ret)
                    {
                        check = true;
                        break;
                    }
                }
                if (!check)
                    break;
            }
            return (ret);
        }
        public class DBKey : BaseDBKey
        {
            public string TSName;
            public DBKey(string _Name)
            {
                TSName = _Name;
            }
            public DBKey(DBKey _key)
            {
                TSName = _key.TSName;
            }
        }
        [Browsable(false)]
        public override BaseDBKey Key { get { return (new DBKey(Name)); } }
        public static void Adjust()
        {
            List<string> Ldb = new List<string>();
            Select S = new Select(string.Format("SELECT * from {0}.SGTypeSizes order by name", BaseItem.Schema));
            while (S.Read())
                Ldb.Add(S["name"] as string);
            S.Dispose();
            L_TypeSize Lpar = ParAll.ST.TSSet;
            foreach (string nm in Ldb)
            {
                if (Lpar[nm] == null)
                    new ExecSQLX(string.Format("delete from {0}.SGTypeSizes where name='{1}'", BaseItem.Schema, nm)).Exec();
            }
            foreach (UPAR.TS.TypeSize ts in Lpar)
            {
                bool IsTS=false;
                foreach (string nm in Ldb)
                {
                    if (nm == ts.Name)
                    {
                        IsTS = true;
                        break;
                    }
                }
                if (!IsTS)
                    new ExecSQLX(string.Format("insert into {0}.SGTypeSizes values('{1}')", BaseItem.Schema, ts.Name)).Exec();
            }
        }
    }
}
