using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQL;

namespace PARLIB
{
    public class SerialTreeSQL : SerialTree
    {
        string schema;
        string table_name="Parameters";
        string Unit;
        string s_table;

        public SerialTreeSQL(string _schema, string _Unit)
        {
            schema = _schema;
            Unit = _Unit;
            s_table = schema + "." + table_name;
            CheckTables();
        }

        private void CheckTables()
        {
            Select S = new Select(string.Format("select count(*) as nn from INFORMATION_SCHEMA.TABLES where table_type='BASE TABLE' and TABLE_SCHEMA='{0}' and table_name='{1}'", schema, table_name));
            if (!S.Read())
                FN.fatal("SerialTreeSQL.CheckTables: " + S.SQL + " - не нашли записей");
            int nn = (int)S[0];
            S.Dispose();
            if (nn != 1)
            {
                new ExecSQL(string.Format(
" CREATE TABLE {0}(" +
" 	Path varchar(512) NOT NULL," +
"	Val varchar(512) NULL," +
" CONSTRAINT PK_{1} PRIMARY KEY CLUSTERED" +
" (" +
"	Path ASC" +
" )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
" ) ON [PRIMARY]", s_table, table_name));
            }
        }
        public override string this[string _path]
        {
            get
            {
                string ret = null;
                using (Select S = new Select(string.Format("select val from {0} where path='{1}' order by path", s_table, _path)))
                {
                    if (!S.Read())
                        return (null);
                    ret = S[0] as string;
                }
                return (ret);
            }
        }
        public override List<string> GetList(string _path)
        {
            List<string> L = new List<string>();
            using (Select S = new Select(string.Format("select path from {0} where path like '{1}{2}%{3}' and path not like '{1}{2}%{3}%{3}' order by path", s_table, _path, "{", "}")))
            {
                while (S.Read())
                    L.Add(S[0] as string);
            }
            return (L);
        }
        internal void Save(LParam _L)
        {
            CDBS.BeginTransaction();
            try
            {
                new ExecSQLX(string.Format("delete from {0}", s_table)).Exec();
                foreach (Param p in _L)
                {
                    ExecSQLX E = new ExecSQLX(string.Format("insert into {0} values('{1}','{2}')", s_table, p.path.Replace('[', '{').Replace(']', '}'), p.val));
                    if (E.Exec() != 1)
                    {
                        CDBS.RollBack();
                        throw new Exception("Не смогли записать настройки в базу");
                    }
                }
            }
            catch
            {
                CDBS.RollBack();
                throw new Exception("Не смогли записать настройки в базу");
            }
            CDBS.Commit();
        }
    }
}
