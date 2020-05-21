using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;


using SQL;

namespace PARLIB
{
    class MetaTreeSQL : MetaTree
    {
        string schema;
        string table_name = "Parameters";
        string Unit;
        public MetaTreeSQL(string _schema, string _Unit)
        {
            schema = _schema;
            Unit = _Unit;
            CheckTables(schema);
        }
        private void CheckTables(string _schema)
        {
            Select S = new Select(string.Format("select count(*) as nn from INFORMATION_SCHEMA.TABLES where table_type='BASE TABLE' and TABLE_SCHEMA='{0}' and table_name='{1}'", _schema, table_name));
            if (!S.Read())
                FN.fatal("SerialTreeSQL.CheckTables: " + S.SQL + " - не нашли записей");
            int nn = (int)S[0];
            S.Dispose();
            if (nn != 1)
            {
                new ExecSQL(string.Format(
" CREATE TABLE {0}(" +
"	Unit [varchar](50) NOT NULL," +
"	Val [varchar](max) NULL," +
" CONSTRAINT PK_{1} PRIMARY KEY CLUSTERED" +
" (" +
"	Unit ASC" +
" )WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]" +
" ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]", _schema + "." + table_name, table_name));
            }
        }

        public override void Load(ParMainLite _O)
        {
            L.Clear();
            Select S = new Select(string.Format("select Val from {0} where Unit='{1}'", schema + "." + table_name, Unit));
            string img = null;
            if (S.Read())
                img = S["Val"] as string;
            S.Dispose();
            if (img == null)
                return;
            MemoryStream ms = null;
            StreamReader sr = null;
            try
            {
                byte[] bbb = Encoding.GetEncoding(1251).GetBytes(img);
                ms = new MemoryStream(bbb);
                sr = new StreamReader(ms, Encoding.GetEncoding(1251));
                while (true)
                {
                    string l = sr.ReadLine();
                    if (l == null)
                        break;
                    Param p = Param.FromFileString(l);
                    if (p != null)
                        L.Add(p);
                }
            }
            catch
            {
                return;
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
                if (ms != null)
                    ms.Dispose();
            }
            SerialTreeFile serialTree = new SerialTreeFile(L);
            ExecLoad(serialTree, _O, (_O as ParBase).PropertyName);
            serialTree.Dispose();
        }
        public override void Save(ParMainLite _O)
        {
            L.Clear();
            ExecSave(_O, (_O as IParentBase).PropertyName);
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(ms, Encoding.GetEncoding(1251));
            foreach (Param p in L)
                sw.WriteLine(p.ToString());
            sw.Flush();
            string img = Encoding.GetEncoding(1251).GetString(ms.ToArray());
            sw.Dispose();
            ms.Dispose();
            ExecSQLX E = new ExecSQLX(string.Format("update {0} set Val=@textdata where Unit='{1}'",
                schema + "." + table_name, Unit));
            E.AddParam("@textdata", SqlDbType.Text, img, img.Length);
            int ret = E.Exec();
            if (ret == 0)
            {
                ExecSQLX EE = new ExecSQLX(string.Format("insert into {0} values('{1}',@textdata)",
                    schema + "." + table_name, Unit));
                EE.AddParam("@textdata", SqlDbType.Text, img, img.Length);
                int aaa = EE.Exec();
            }
        }
    }
}
