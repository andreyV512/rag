using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Protocol;
using SQL;

namespace Defect.SG
{
    class EtalonPars : BaseItem
    {
        [DisplayName("Номер")]
        public int Id { get { return (ownKey.EtalonParsId); } set { ownKey.EtalonParsId = value; } }

        [DisplayName("Значение")]
        public double Val { get; set; }

        DBKey ownKey = null;

        public static string Title = "Параметры эталона";
        public static void LoadKey(BaseDBKey _Key, BindingSource _bs)
        {
            _bs.Clear();
            if (_Key == null)
                return;
            Etalon.DBKey Key = _Key as Etalon.DBKey;
            _bs.Clear();
            Select S = new Select(string.Format("SELECT par, val from {0}.SGEtalonPars where typeSize='{1}' and sGroup='{2}' and etalon={3} order by par",
                BaseItem.Schema,
                Key.TSName,
                Key.GroupName,
                Key.EtalonId
                ));
            while (S.Read())
                _bs.Add(new EtalonPars(Key, (int)S["par"], SD(S["val"])));
            S.Dispose();
        }
        public static bool InsertKey(BaseDBKey _Key, BindingSource _bs)
        {
            Etalon.DBKey Key = _Key as Etalon.DBKey;
            int new_id = FindNewId(_bs);
            if (new ExecSQLX(string.Format("insert into {0}.SGEtalonPars values('{1}','{2}',{3},{4})",
                BaseItem.Schema, Key.TSName, Key.GroupName, Key.EtalonId.ToString(), new_id.ToString())).Exec() == 1)
            {
                _bs.Position = _bs.Add(new EtalonPars(Key, new_id, 0));
                return (true);
            }
            else
                return (false);
        }


        EtalonPars(Etalon.DBKey _parentKey, int _Id, double _Val)
        {
            ownKey = new DBKey(_parentKey, _Id);    
            Val = _Val;
        }
        public override string ToString()
        {
            return ("Пар.эталона: Id=" + Id.ToString() + " Val=" + Val.ToString());
        }
        public override bool Update()
        {
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGEtalonPars set val={1} where typeSize='{2}' and sGroup='{3}' and etalon={4} and par={5}",
                BaseItem.Schema,
                Val.ToString().Replace(',', '.'),
                ownKey.TSName,
                ownKey.GroupName,
                ownKey.EtalonId.ToString(),
                ownKey.EtalonParsId.ToString()));
            if (E.Exec() == 1)
                return (true);
            else
            {
                Select S = new Select(string.Format("SELECT val from {0}.SGEtalonPars where typeSize='{1}' and sGroup='{2}' and etalon={3} and par={4}",
                    BaseItem.Schema,
                    ownKey.TSName,
                    ownKey.GroupName,
                    ownKey.EtalonId.ToString(),
                    ownKey.EtalonParsId.ToString()
                    ));
                if (!S.Read())
                    throw new Exception("EtalonPars: не могу получить предыдуще значения");
                Val = SD(S["val"]);
                S.Dispose();
                return (false);
            }
        }
        public override bool Delete()
        {
            return (new ExecSQLX(string.Format("delete from {0}.SGEtalonPars where typeSize='{1}' and sGroup='{2}' and par={3}",
                BaseItem.Schema,
                ownKey.TSName,
                ownKey.GroupName,
                ownKey.EtalonParsId.ToString()
                )).Exec() == 1);
        }
        static int FindNewId(BindingSource _bs)
        {
            for (int i = 0; ; i++)
            {
                bool check = false;
                foreach (EtalonPars et in _bs)
                {
                    if (et.Id == i)
                    {
                        check = true;
                        break;
                    }
                }
                if (!check)
                    return (i);
            }
        }

        public static bool Save(string _TSName, string _GroupName, int _EtalonId, double[] _mval)
        {
            new ExecSQLX(string.Format("delete from {0}.SGEtalonPars where typeSize='{1}' and sGroup='{2}' and etalon={3}",
                BaseItem.Schema, _TSName, _GroupName, _EtalonId.ToString())).Exec();
            for (int i = 0; i < _mval.Length; i++)
            {
                ExecSQLX E = new ExecSQLX(string.Format("insert into {0}.SGEtalonPars values('{1}','{2}',{3},{4},{5})",
                    BaseItem.Schema, _TSName, _GroupName, _EtalonId.ToString(), i.ToString(), _mval[i].ToString().Replace(',', '.')));
                if (E.Exec() != 1)
                    return (false);
            }
            return (true);
        }
        public class DBKey : Etalon.DBKey
        {
            public int EtalonParsId;
            public DBKey(Etalon.DBKey _parentKey, int _Id):base(_parentKey)
            {
                EtalonParsId = _Id;
            }
            public DBKey(DBKey _ownKey):base(_ownKey)
            {
                EtalonParsId = _ownKey.EtalonParsId;
            }
        }
        [Browsable(false)]
        public override BaseDBKey Key { get { return (ownKey); } }
    }
}
