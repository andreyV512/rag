using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using Protocol;
using SQL;
using UPAR.SG;

namespace Defect.SG
{
    public class Etalon : GraphObject
    {
        [DisplayName("Номер")]
        public int Id { get { return (ownKey.EtalonId); } set { ownKey.EtalonId = value; } }

        [DisplayName("Дата")]
        public DateTime Dt { get; set; }

        [DisplayName("СОП")]
        public string SOP { get; set; }

        [DisplayName("Включен")]
        public bool IsOn { get; set; }

        DBKey ownKey = null;

        public static string Title = "Эталоны";
        public static void LoadKey(BaseDBKey _Key, BindingSource _bs)
        {
            _bs.Clear();
            if (_Key == null)
                return;
            Group.DBKey Key = _Key as Group.DBKey;
            Select S = new Select(string.Format("SELECT id, dt, sop, IsOn from {0}.SGEtalons where typeSize='{1}' and sGroup='{2}' order by id",
                BaseItem.Schema,
                Key.TSName,
                Key.GroupName
                ));
            while (S.Read())
                _bs.Add(new Etalon(Key, (int)S["id"], (DateTime)S["dt"], S["SOP"] as string, Convert.ToBoolean(S["IsOn"])));
            S.Dispose();
        }
        public static bool InsertKey(BaseDBKey _Key, BindingSource _bs)
        {
            if (_Key == null)
                return (false);
            Group.DBKey Key = _Key as Group.DBKey;
            int new_id = FindNewId(_bs);
            Select S = new Select(string.Format("insert into {0}.SGEtalons (typeSize,sGroup,id) output inserted.dt values('{1}','{2}',{3})",
                BaseItem.Schema, Key.TSName, Key.GroupName, new_id.ToString()));
            bool ret = S.Read();
            if (ret)
                _bs.Position = _bs.Add(new Etalon(Key, new_id, (DateTime)S["dt"], null, true));
            S.Dispose();
            return (ret);
        }
        public static int TubeToEtalon(Tube _tb, Group.DBKey _gKey)
        {
            if (_gKey == null)
                return(-1);
            string SQL = string.Format(
            "insert {0}.SGEtalons output Inserted.ID" +
            " select '{1}','{2}'," +
            " (select isnull(max(id)+1,0) from {0}.SGEtalons where typeSize='{1}'and sGroup='{2}')" +
            " ,t.dt, t.sop, 'True', t.img" +
            " from Uran.SGTubes t where t.id={3}",
            BaseItem.Schema,
            _gKey.TSName,
            _gKey.GroupName,
            _tb.Id.ToString());
            Select S = new Select(SQL);
            if (!S.Read())
            {
                S.Dispose();
                return (-1);
            }
            int new_id = (int)S["id"];
            S.Dispose();
            SQL = string.Format(
            "insert {0}.SGEtalonPars" +
            " select '{1}','{2}',{3},p.par,p.val" +
            " from {0}.SGTubePars p" +
            " where p.tube_id={4}",
            BaseItem.Schema,
            _gKey.TSName,
            _gKey.GroupName,
            new_id.ToString(),
            _tb.Id.ToString());
            new ExecSQLX(SQL).Exec();
            return (new_id);
        }

        Etalon(Group.DBKey _parentKey, int _Id, DateTime _Dt, string _SOP, bool _IsOn)
            : base(_parentKey.TSName)
        {
            ownKey = new DBKey(_parentKey, _Id);
            Dt = _Dt;
            SOP = _SOP;
            IsOn = _IsOn;
        }
        public override string ToString()
        {
            return ("Эталон: Id=" + Id.ToString() + " Dt=" + Dt.ToString() + " IsOn=" + (IsOn ? "True" : "False"));
        }
        public override bool Update()
        {
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGEtalons set dt=@dt, IsOn='{1}' where typeSize='{2}' and sGroup='{3}' and id={4}",
                BaseItem.Schema,
                IsOn ? "true" : "false",
                ownKey.TSName,
                ownKey.GroupName,
                Id.ToString()));
            E.AddParam("@dt", SqlDbType.DateTime, Dt);
            pr("Update");
            bool ret = E.Exec() == 1;
            if (!ret)
            {
                Select S = new Select(string.Format("SELECT dt from {0}.SGEtalons where typeSize='{1}' and sGroup='{2}' and id={3}",
                BaseItem.Schema,
                ownKey.TSName,
                ownKey.GroupName,
                Id.ToString()
                ));
                if (!S.Read())
                    throw new Exception("Etalon: не могу получить предыдуще значения");
                Dt = (DateTime)S["dt"];
                IsOn = Convert.ToBoolean(S["IsOn"]);
                S.Dispose();
            }
            return (ret);
        }
        public override bool Delete()
        {
            return (new ExecSQLX(string.Format("delete from {0}.SGEtalons where typeSize='{1}' and sGroup='{2}' and id={3}",
                BaseItem.Schema,
                ownKey.TSName,
                ownKey.GroupName,
                Id.ToString()
                )).Exec() == 1);
        }
        static int FindNewId(BindingSource _bs)
        {
            for (int i = 0; ; i++)
            {
                bool check = false;
                foreach (Etalon et in _bs)
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
        public class DBKey : Group.DBKey
        {
            public int EtalonId;
            public DBKey(DBKey _ownKey)
                : base(_ownKey)
            {
                EtalonId = _ownKey.EtalonId;
            }
            public DBKey(Group.DBKey _parentKey, int _Id)
                : base(_parentKey)
            {
                EtalonId = _Id;
            }
        }
        [Browsable(false)]
        public override BaseDBKey Key { get { return (ownKey); } }
        public override void UpdateSOP(SOPPars _sop)
        {
            SOP = _sop == null ? null : _sop.Name;
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGEtalons set SOP={1} where typeSize='{2}' and sGroup='{3}' and id={4}",
                BaseItem.Schema,
                SOP == null ? "null" : "'" + SOP + "'",
                ownKey.TSName,
                ownKey.GroupName,
                Id.ToString()));
            bool ret = E.Exec() == 1;
            if (!ret)
                throw new Exception("Etalon: не могу записать SOP");
        }
        public override bool CalcPars(SOPPars _sop)
        {
            ExecuteX E = new ExecuteX(string.Format("{0}.CalcSGEtalonParsSOP", BaseItem.Schema));
            E.Input("@pars", DbType.Boolean, true);
            E.Input("@typeSize", DbType.String, ownKey.TSName);
            E.Input("@sGroup", DbType.String, ownKey.GroupName);
            E.Input("@id", DbType.Int64, Convert.ToInt64(Id));
            E.Input("@SOPLenght", DbType.Int32, _sop == null ? DBNull.Value : (object)_sop.Length);
            E.Input("@SOPStart", DbType.Int32, _sop == null ? DBNull.Value : (object)_sop.BeginPoint);
            E.Input("@SOPStop", DbType.Int32, _sop == null ? DBNull.Value : (object)_sop.EndPoint);
            E.Read();
            bool ret = (int)E.Param("@RC") == 1;
            E.Dispose();
            return (ret);
        }
        public override SGHalfPeriod[] GetHalfPeriods(SOPPars _sop)
        {
            ExecuteX E = new ExecuteX(string.Format("{0}.CalcSGEtalonParsSOP", BaseItem.Schema));
            E.Input("@pars", DbType.Boolean, false);
            E.Input("@typeSize", DbType.String, ownKey.TSName);
            E.Input("@sGroup", DbType.String, ownKey.GroupName);
            E.Input("@id", DbType.Int64, Convert.ToInt64(Id));
            E.Input("@SOPLenght", DbType.Int32, _sop == null ? DBNull.Value : (object)_sop.Length);
            E.Input("@SOPStart", DbType.Int32, _sop == null ? DBNull.Value : (object)_sop.BeginPoint);
            E.Input("@SOPStop", DbType.Int32, _sop == null ? DBNull.Value : (object)_sop.EndPoint);
            List<SGHalfPeriod> L = new List<SGHalfPeriod>();
            int index = 0;
            while (E.Read())
                L.Add(new SGHalfPeriod((int)E["par"], Convert.ToInt32((Single)E["val"]), index++));
            E.Dispose();
            return (L.ToArray());
        }
        protected override string GetImg()
        {
            Select S = new Select(string.Format("select img from {0}.SGEtalons where typesize='{1}' and sGroup='{2}' and id={3}",
                BaseItem.Schema, ownKey.TSName, ownKey.GroupName, Id.ToString()));
            string ret = null;
            if (S.Read())
                ret = S["img"] as string;
            S.Dispose();
            return (ret);
        }
        protected override bool SetImg(string _data)
        {
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGEtalons set img=@textdata where typesize='{1}' and sGroup='{2}' and id={3}",
                BaseItem.Schema, ownKey.TSName, ownKey.GroupName, Id.ToString()));
            E.AddParam("@textdata", SqlDbType.Text, _data, _data.Length);
            return (E.Exec() == 1);
        }
    }
}
