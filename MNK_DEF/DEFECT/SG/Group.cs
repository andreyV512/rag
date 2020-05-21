using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using SQL;
using Protocol;

namespace Defect.SG
{
    public class Group : BaseItem
    {
        string saved_name = "";
        [DisplayName("Наименование")]
        public string Name { get { return (ownKey.GroupName); } set { ownKey.GroupName = value; } }

        [DisplayName("Цвет")]
        public Color RColor { get; set; }

        [DisplayName("Включен")]
        public bool IsOn { get; set; }

        DBKey ownKey = null;

        public static string Title = "Группы";
        public static void LoadKey(BaseDBKey _Key, BindingSource _bs)
        {
            _bs.Clear();
            if (_Key == null)
                return;
            TypeSize.DBKey Key = _Key as TypeSize.DBKey;
            Select S = new Select(string.Format("SELECT * from {0}.SGroups where typeSize='{1}' order by name", BaseItem.Schema, Key.TSName));
            while (S.Read())
                _bs.Add(new Group(Key, S["name"] as string, Color.FromArgb((int)S["color"]), Convert.ToBoolean(S["IsOn"])));
            S.Dispose();
        }
        public static bool InsertKey(BaseDBKey _Key, BindingSource _bs)
        {
            TypeSize.DBKey key = _Key as TypeSize.DBKey;
            string new_name = FindNewName(_bs);
            if (new ExecSQLX(string.Format("insert into {0}.SGroups (typeSize, name, color) values('{1}','{2}',3)", BaseItem.Schema, key.TSName, new_name, Color.Black.ToArgb())).Exec() == 1)
            {
                _bs.Position = _bs.Add(new Group(key, new_name, Color.Black, true));
                return (true);
            }
            return (false);
        }


        int width = 14;
        int height = 14;


        Group(TypeSize.DBKey _parentKey, string _name, Color _color, bool _IsOn)
        {
            ownKey = new DBKey(_parentKey, _name);
            saved_name = Name;
            RColor = _color;
            IsOn = _IsOn;
        }
        public override string ToString()
        {
            return ("ГП=" + Name + " IsOn=" + (IsOn ? "True" : "False"));
        }
        public override bool Update()
        {
            pr("Group.Update()");
            bool ret = new ExecSQLX(string.Format("update {0}.SGroups set name='{1}',color={2}, IsOn='{3}' where typeSize='{4}' and name='{5}'",
                BaseItem.Schema,
                Name,
                DBColor,
                IsOn ? "true" : "false",
                ownKey.TSName,
                saved_name
                )).Exec() == 1;
            if (ret)
                saved_name = Name;
            else
            {
                Name = saved_name;
                Select S = new Select(string.Format("SELECT * from {0}.SGroups where typeSize='{1} and name='{2}'", BaseItem.Schema, ownKey.TSName, Name));
                if (!S.Read())
                    throw new Exception("Group: не могу получить предыдуще значения");
                RColor = Color.FromArgb((int)S["color"]);
                IsOn = Convert.ToBoolean(S["IsOn"]);
                S.Dispose();
            }
            return (ret);
        }
        public override bool Delete()
        {
            return (new ExecSQLX(string.Format("delete from {0}.SGroups where typeSize='{1}' and name='{2}'", BaseItem.Schema, ownKey.TSName, saved_name)).Exec() == 1);
        }
        static string FindNewName(System.Windows.Forms.BindingSource _bs)
        {
            string head = "Новая";
            string ret;
            for (int i = 0; ; i++)
            {
                ret = head + (i == 0 ? "" : i.ToString());
                bool check = false;
                foreach (Group gr in _bs)
                {
                    if (gr.Name == ret)
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
        [Browsable(false)]
        int DBColor { get { return (RColor.ToArgb()); } set { RColor = Color.FromArgb(value); } }

        [DisplayName("Цвет"), Browsable(false)]
        public Bitmap Picture
        {
            get
            {
                Bitmap bm = new Bitmap(width, height);
                Color lcolor = RColor;
                if (lcolor == null)
                    lcolor = Color.Black;
                Graphics.FromImage(bm).FillRectangle(new SolidBrush(lcolor), 0, 0, width, height);
                return (bm);
            }
            set
            {
                RColor = value.GetPixel(0, 0);
            }
        }
        public class DBKey : TypeSize.DBKey
        {
            public string GroupName;
            public DBKey(DBKey _ownKey)
                : base(_ownKey)
            {
                GroupName = _ownKey.GroupName;
            }
            public DBKey(TypeSize.DBKey _parentKey, string _Name)
                : base(_parentKey)
            {
                GroupName = _Name;
            }
        }
        [Browsable(false)]
        public override BaseDBKey Key { get { return (ownKey); } }
    }
}
