using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using SQL;
using UPAR;

namespace Defect.SG
{
    class Tresh : BaseItem
    {
        [DisplayName("Номер")]
        public int Id { get { return (ownKey.TreshId); } set { ownKey.TreshId = value; } }


        [DisplayName("Значение,%")]
        public double Val { get; set; }

        DBKey ownKey = null;

        public static string Title = "Пороги";

        Tresh(TypeSize.DBKey _parentKey, int _Id, double _Val)
        {
            ownKey = new DBKey(_parentKey, _Id);
            Val = _Val;
        }
        public override string ToString()
        {
            return ("Порог: Id=" + Id.ToString() + " Val=" + Val.ToString());
        }
        public static bool InsertKey(BaseDBKey _Key, BindingSource _bs)
        {
            TypeSize.DBKey key = _Key as TypeSize.DBKey;
            int new_id = FindNewId(_bs);
            if (new ExecSQLX(string.Format("insert into {0}.SGTresh values('{1}',{2},0)",
                BaseItem.Schema, key.TSName, new_id.ToString())).Exec() == 1)
            {
                _bs.Position = _bs.Add(new Tresh(key, new_id, 0));
                return (true);
            }
            else
                return (false);
        }
        public override bool Update()
        {
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGTresh set val={1} where typeSize='{2}' and Par={3}",
                BaseItem.Schema,
                Val.ToString().Replace(',', '.'),
                ownKey.TSName,
                ownKey.TreshId.ToString()
            ));
            int ret = E.Exec();
            if (ret == 1)
                return (true);
            else
            {
                Select S = new Select(string.Format("SELECT val from {0}.SGTresh where typeSize='{1}' and Par={2}",
                    BaseItem.Schema,
                    ownKey.TSName,
                    ownKey.TreshId.ToString()));
                if (!S.Read())
                    throw new Exception("Tresh: не могу получить предыдуще значения");
                Val = SD(S["val"]);
                S.Dispose();
                return (false);
            }
        }
        public override bool Delete()
        {
            int ret = new ExecSQLX(string.Format("delete from {0}.SGTresh where typeSize='{1}' and Par={2}",
                    BaseItem.Schema,
                    ownKey.TSName,
                    ownKey.TreshId.ToString())).Exec();
            if (ret != 1)

                return (false);
            new ExecSQLX(string.Format(
" UPDATE x SET x.par = x.new_par FROM (" +
" SELECT par, ROW_NUMBER() OVER (ORDER BY [par]) - 1  AS new_par" +
" FROM {0}.SGTresh where typeSize='{1}' ) x",
            BaseItem.Schema,
            ownKey.TSName)).Exec();
            return (true);
        }
        public static void LoadKey(BaseDBKey _Key, BindingSource _bs)
        {
            TypeSize.DBKey key = _Key as TypeSize.DBKey;
            if (key == null)
                return;
            _bs.Clear();
            double[] vals = ValsPercent(key.TSName);
            if (vals == null)
                return;
            int i = 0;
            foreach (double val in vals)
            {
                Tresh p = new Tresh(key, i++, val);
                _bs.Add(p);
            }
        }
        static int FindNewId(BindingSource _bs)
        {
            for (int i = 0; ; i++)
            {
                bool check = false;
                foreach (Tresh o in _bs)
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
        public static List<double> ValsPercentL(string _TSName)
        {
            Select S = new Select(string.Format("select val from {0}.SGTresh where typesize='{1}' order by par", BaseItem.Schema, _TSName));
            List<double> L = new List<double>();
            while (S.Read())
                L.Add(SD(S["val"]));
            S.Dispose();
            return (L);
        }
        public static double[] ValsPercent(string _TSName)
        {
            return (ValsPercentL(_TSName).ToArray());
        }
        public static List<int> ValsL(string _TSName)
        {
            List<double> L = ValsPercentL(_TSName);
            List<int> Lret = new List<int>();
            double k = ParAll.SG.sgPars.HalfPeriod;
            k /= 100;
            for (int i = 0; i < L.Count; i++)
                Lret.Add((int)(L[i] * k));
            return (Lret);
        }
        public static int[] Vals(string _TSName)
        {
            return (ValsL(_TSName).ToArray());
        }
        public static bool Save(string _TSName, int[] _tresh)
        {
            double[] treshPercent = new double[_tresh.Length];
            double k = 100;
            k /= ParAll.SG.sgPars.HalfPeriod; 
            for (int i = 0; i < _tresh.Length; i++)
                treshPercent[i] = Math.Round(_tresh[i] * k, 2);
            CDBS.BeginTransaction();
            bool ret = Save0(_TSName, treshPercent);
            if (ret)
                CDBS.Commit();
            else
                CDBS.RollBack();
            return (ret);
        }
        public static bool Save0(string _TSName, double[] _treshPercent)
        {
            ExecSQLX E = new ExecSQLX(string.Format("delete from {0}.SGTresh where typeSize='{1}'",
                    BaseItem.Schema,
                    _TSName
                    ));
            int ret = E.Exec();
            if (_treshPercent == null)
                return (true);
            for (int i = 0; i < _treshPercent.Length; i++)
            {
                ExecSQLX EE = new ExecSQLX(string.Format("insert into {0}.SGTresh values('{1}',{2},{3})",
                    BaseItem.Schema, _TSName, i.ToString(), _treshPercent[i].ToString().Replace(',', '.')));
                if (EE.Exec() != 1)
                    return (false);
            }
            new ExecSQLX(string.Format(
" UPDATE x SET x.par = x.new_par FROM (" +
" SELECT par, ROW_NUMBER() OVER (ORDER BY val)-1  AS new_par" +
" FROM {0}.SGTresh where typeSize='{1}' ) x",
            BaseItem.Schema, _TSName)).Exec();
            return (true);
        }
        public class DBKey : TypeSize.DBKey
        {
            public int TreshId;
            public DBKey(DBKey _ownKey)
                : base(_ownKey)
            {
                TreshId = _ownKey.TreshId;
            }
            public DBKey(TypeSize.DBKey _parentKey, int _TreshId)
                : base(_parentKey)
            {
                TreshId = _TreshId;
            }
        }
        [Browsable(false)]
        public override BaseDBKey Key { get { return (ownKey); } }
    }
}
