using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Protocol;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using SQL;

namespace Defect.SG
{
    class TubePars : BaseItem
    {
        [DisplayName("Номер")]
        public int Id { get { return (ownKey.TubeParsId); } set { ownKey.TubeParsId = value; } }

        [DisplayName("Значение")]
        public double Val { get; set; }

        DBKey ownKey = null;

        public static string Title = "Параметры трубы";
        public static void LoadKey(BaseDBKey _Key, BindingSource _bs)
        {
            _bs.Clear();
            if (_Key == null)
                return;
            Tube.DBKey Key = _Key as Tube.DBKey;
            _bs.Clear();
            Select S = new Select(string.Format("SELECT par, val from {0}.SGTubePars where tube_id={1} order by par",
                BaseItem.Schema,
                Key.TubeId
                ));
            while (S.Read())
                _bs.Add(new TubePars(Key, (int)S["par"], SD(S["val"])));
            S.Dispose();

        }
        public static bool InsertKey(BaseDBKey _Key, BindingSource _bs)
        {
            Tube.DBKey Key = _Key as Tube.DBKey;
            int new_id = FindNewId(_bs);
            if (new ExecSQLX(string.Format("insert into {0}.SGTubePars values({1},{2},0)",
                BaseItem.Schema, Key.TubeId.ToString(), new_id.ToString())).Exec() == 1)
            {
                _bs.Position = _bs.Add(new TubePars(Key, new_id, 0));
                return (true);
            }
            else
                return (false);
        }

        
        TubePars(Tube.DBKey _parentKey, int _Id, double _Val)
        {
            ownKey = new DBKey(_parentKey, _Id);
            Val = _Val;
        }
        public override string ToString()
        {
            return ("Пар.трубы: Id=" + Id.ToString() + " Val=" + Val.ToString());
        }
        public static bool Save(Int64 _TubeId, double[] _mval)
        {
            new ExecSQLX(string.Format("delete from {0}.SGTubePars where tube_id={1}", BaseItem.Schema, _TubeId.ToString())).Exec();
            for (int i = 0; i < _mval.Length; i++)
            {
                ExecSQLX E = new ExecSQLX(string.Format("insert into {0}.SGTubePars values({1},{2},{3})",
                    BaseItem.Schema, _TubeId.ToString(), i.ToString(), _mval[i].ToString().Replace(',', '.')));
                if (E.Exec() != 1)
                    return (false);
            }
            return (true);
        }
        public override bool Update()
        {
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGTubePars set val={1} where tube_id={2} and Par={3}",
                BaseItem.Schema,
                Val.ToString().Replace(',', '.'),
                ownKey.TubeId.ToString(),
                ownKey.TubeParsId.ToString()));
            if (E.Exec() == 1)
                return (true);
            else
            {
                Select S = new Select(string.Format("SELECT val from {0}.SGTubePars where tube_id={1} and Par={2}",
                    BaseItem.Schema,
                    ownKey.TubeId.ToString(),
                    ownKey.TubeParsId.ToString()
                    ));
                if (!S.Read())
                    throw new Exception("TubePars: не могу получить предыдуще значения");
                Val = SD(S["val"]);
                S.Dispose();
                return (false);
            }
        }
        public override bool Delete()
        {
            return (new ExecSQLX(string.Format("delete from {0}.SGTubePars where tube_id={1} and Par={2}",
                BaseItem.Schema,
                ownKey.TubeId.ToString(),
                ownKey.TubeParsId.ToString()
                )).Exec() == 1);
        }
        static int FindNewId(BindingSource _bs)
        {
            for (int i = 0; ; i++)
            {
                bool check = false;
                foreach (TubePars o in _bs)
                {
                    if (o.Id == i)
                    {
                        check = true;
                        break;
                    }
                }
                if (!check)
                    return (i);
            }
        }
        public class DBKey : Tube.DBKey
        {
            public int TubeParsId;
            public DBKey(Tube.DBKey _parentKey, int _Id)
                : base(_parentKey)
            {
                TubeParsId = _Id;
            }
            public DBKey(DBKey _ownKey)
                : base(_ownKey)
            {
                TubeParsId = _ownKey.TubeParsId;
            }
        }
        public override BaseDBKey Key { get { return (null); } }
    }
}
