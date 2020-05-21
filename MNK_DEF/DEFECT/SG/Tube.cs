using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

using Protocol;
using SQL;
using UPAR.SG;
using System.Data.SqlTypes;

#if CALC_LOCAL
using UPAR;
using CalclSGPars;
#endif

namespace Defect.SG
{
    public class Tube : GraphObject
    {
        [DisplayName("Номер")]
        public Int64 Id { get { return (ownKey.TubeId); } set { ownKey.TubeId = value; } }

        [DisplayName("Дата")]
        public DateTime Dt { get; set; }

        [DisplayName("Наименование")]
        public string Name { get; set; }

        [DisplayName("ГП")]
        public string SG { get; set; }

        [DisplayName("Метрика")]
        public double Metric { get; set; }

        [DisplayName("СОП")]
        public string SOP { get; set; }

        DBKey ownKey = null;

        public static string Title = "Трубы";
        public static void LoadKey(BaseDBKey _Key, BindingSource _bs)
        {
            _bs.Clear();
            if (_Key == null)
                return;
            TypeSize.DBKey Key = _Key as TypeSize.DBKey;
            Select S = new Select(string.Format("SELECT id, dt, name, sGroup, probability, SOP from {0}.SGTubes where typeSize='{1}' order by id desc",
                BaseItem.Schema, Key.TSName));
            while (S.Read())
            {
                object oMetric = S["probability"];
                _bs.Add(new Tube(
                    Key,
                    (Int64)S["id"],
                    (DateTime)S["dt"],
                    S["name"] as string,
                    S["sGroup"] as string,
                    oMetric.GetType() == typeof(Single) ? Convert.ToDouble(oMetric) : 0,
                    S["SOP"] as string
                    ));
            }
            S.Dispose();
        }
        public static bool InsertKey(BaseDBKey _Key, BindingSource _bs)
        {
            TypeSize.DBKey Key = _Key as TypeSize.DBKey;
            Select S = new Select(string.Format("insert into {0}.SGTubes (typeSize) output Inserted.id,inserted.dt values('{1}')",
            BaseItem.Schema, Key.TSName));
            bool ret = S.Read();
            if (ret)
                _bs.Position = _bs.Add(new Tube(Key, (Int64)S["id"], (DateTime)S["dt"], null, null, 0, null));
            S.Dispose();
            return (ret);
        }
        Tube(TypeSize.DBKey _parentKey, Int64 _Id, DateTime _Dt, string _Name, string _SG, double _Metric, string _SOP)
            : base(_parentKey.TSName)
        {
            ownKey = new DBKey(_parentKey, _Id);
            Dt = _Dt;
            Name = _Name;
            SG = _SG;
            Metric = _Metric;
            SOP = _SOP;
        }
        public override string ToString()
        {
            return ("Труба=" + Id.ToString() + " Дата=" + Dt.ToString() + " Name=" + Name + " ГП=" + SG + " Metric=" + Metric.ToString());
        }
        public override bool Update()
        {
            pr("Tube.Update()");
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGTubes set dt=@dt, name='{1}', sGroup='{2}' where id={3}",
                BaseItem.Schema,
                Name,
                SG,
                Id.ToString()
                ));
            E.AddParam("@dt", SqlDbType.DateTime, Dt);
            bool ret = E.Exec() == 1;
            if (!ret)
            {
                Select S = new Select(string.Format("SELECT dt, name, sGroup from {0}.SGTubes where Id={1}",
                    BaseItem.Schema, Id.ToString()));
                if (!S.Read())
                    throw new Exception("Tube: не могу получить предыдуще значения");
                Dt = (DateTime)S["dt"];
                Name = S["name"] as string;
                SG = S["sGroup"] as string;
                S.Dispose();
            }
            return (ret);
        }
        public override void UpdateSOP(SOPPars _sop)
        {
            SOP = _sop == null ? null : _sop.Name;
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGTubes set SOP={1} where id={2}",
                BaseItem.Schema,
                SOP == null ? "null" : "'" + SOP + "'",
                Id.ToString()
                ));
            bool ret = E.Exec() == 1;
            if (!ret)
                throw new Exception("Tube: не могу записать SOP");
        }
        public override bool Delete()
        {
            return (new ExecSQLX(string.Format("delete from {0}.SGTubes where Id={1}", BaseItem.Schema, Id.ToString())).Exec() == 1);
        }
        public class DBKey : TypeSize.DBKey
        {
            public Int64 TubeId;
            public DBKey(TypeSize.DBKey _parentKey, Int64 _Id)
                : base(_parentKey)
            {
                TubeId = _Id;
            }
            public DBKey(DBKey _ownKey)
                : base(_ownKey)
            {
                TubeId = _ownKey.TubeId;
            }
        }
        [Browsable(false)]
        public override BaseDBKey Key { get { return (ownKey); } }
#if CALC_LOCAL
        public override bool CalcPars(SOPPars _sop)
        {
            ExecuteX E = new ExecuteX(string.Format("{0}.CalcSGTubeParsSOP", BaseItem.Schema));
            E.Input("@pars", DbType.Boolean, true);
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
            SGPars pars = ParAll.ST.Defect.Cross.SolidGroup.sgPars;
            List<SGHalfPeriod> L = new List<SGHalfPeriod>();
            IEnumerator ie = SGCalc.Calc(
                SqlBoolean.False,
                new SqlString(GetImg()),
                GetThech(),
                new SqlInt32(pars.HalfPeriod),
                new SqlInt32(pars.HalfPeriodDif),
                new SqlBoolean(pars.FullPeriod),
                new SqlString(pars.AlgorithmPoints.ToString()),
                new SqlString(pars.ValIU.ToString()),
                new SqlInt32(pars.BorderBegin),
                new SqlInt32(pars.BorderEnd),
                _sop == null ? SqlInt32.Null : new SqlInt32(_sop.Length),
                _sop == null ? SqlInt32.Null : new SqlInt32(_sop.BeginPoint),
                _sop == null ? SqlInt32.Null : new SqlInt32(_sop.EndPoint)
                );
            int index = 0;
            while (ie.MoveNext())
            {
                SGCalc.Result result = ie.Current as SGCalc.Result;
                L.Add(new SGHalfPeriod(result.par, Convert.ToInt32(result.val), index++));
            }
            return (L.ToArray());
        }
        SqlString GetThech()
        {
            string ret = null;
            using (Select S = new Select(string.Format(
"select tr.val from {0}.SGTresh tr join {0}.SGTubes tb on tb.typeSize=tr.typeSize " +
"where tb.id={1} order by par",
                    BaseItem.Schema, Id.ToString())))
            {
                while (S.Read())
                {
                    if (ret != null)
                        ret += ";";
                    ret += S[0].ToString();
                }

            }
            return (new SqlString(ret));
        }
#else
        public override bool CalcPars(SOPPars _sop)
        {
            ExecuteX E = new ExecuteX(string.Format("{0}.CalcSGTubeParsSOP", BaseItem.Schema));
            E.Input("@pars", DbType.Boolean, true);
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
            ExecuteX E = new ExecuteX(string.Format("{0}.CalcSGTubeParsSOP", BaseItem.Schema));
            E.Input("@pars", DbType.Boolean, false);
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
#endif
        protected override string GetImg()
        {
            Select S = new Select(string.Format("select img from {0}.SGTubes where id={1}", BaseItem.Schema, Id.ToString()));
            string ret = null;
            if (S.Read())
                ret = S["img"] as string;
            S.Dispose();
            return (ret);
        }
        protected override bool SetImg(string _data)
        {
            ExecSQLX E = new ExecSQLX(string.Format("update {0}.SGTubes set img=@textdata where id={1}",
                BaseItem.Schema, Id.ToString()));
            E.AddParam("@textdata", SqlDbType.Text, _data, _data.Length);
            return (E.Exec() == 1);
        }
    }
}
