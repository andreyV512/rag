using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQL;
using PARLIB;

namespace Signals.Boards
{
    class Board1784SQL : Board1784virtual
    {
        string schema;
        public Board1784SQL(string _schema, int _DevNum, bool _PCSide = true)
            : base(_DevNum, _PCSide)
        {
            schema = _schema;
            CheckTables();
        }
        void CheckTables()
        {
            Select S;
            int nn;
            S = new Select(string.Format(
"select count(*) as nn from INFORMATION_SCHEMA.TABLES where table_type='BASE TABLE' and TABLE_SCHEMA='{0}' and table_name='Board1784'"
            , schema));
            if (!S.Read())
                FN.fatal("BoardSQL.CheckTables: " + S.SQL + " - не нашли записей");
            nn = (int)S[0];
            S.Dispose();
            if (nn != 1)
                new ExecSQL(string.Format("CREATE TABLE {0}.Board1784(DevNum int NOT NULL,Lir0 int NOT NULL,Lir1 int NOT NULL,Command varchar(50) NULL)", schema));
            S = new Select(string.Format("select count(*) as nn from {0}.Board1784 where DevNum={1}", schema, DevNum));
            S.Read();
            nn = (int)S[0];
            S.Dispose();
            if (nn < 1)
                new ExecSQL(string.Format("insert into {0}.Board1784 (DevNum,Lir0,Lir1) values({1},0,0)", schema, DevNum.ToString()));
        }
        public override int[] Read()
        {

            int[] ret = new int[2];

            Select S = new Select(string.Format("select Lir0,Lir1 as nn from {0}.Board1784 where DevNum={1}", schema, DevNum));
            S.Read();
            ret[0] = (int)S[0];
            ret[1] = (int)S[1];
            S.Dispose();
            return (ret);
        }
        public override void Write(int[] _vals)
        {
            new ExecSQL(string.Format("update {0}.Board1784 set Lir0={1}, Lir1={2} where DevNum={3}",
                schema,
                _vals[0].ToString(),
                _vals[1].ToString(),
                DevNum.ToString()));
        }
        public override void Reset()
        {
            new ExecSQL(string.Format("update {0}.Board1784 set Command='reset' where DevNum={1}",
            schema,
            DevNum.ToString()));
        }
        public override bool Exec()
        {
            Select S = new Select(string.Format("select Command from {0}.Board1784 where DevNum={1}", schema, DevNum));
            S.Read();
            string cmd = S[0] as string;
            S.Dispose();
            if (cmd == "reset")
            {
                new ExecSQL(string.Format("update {0}.Board1784 set Lir0=0, Lir1=0, Command=null where DevNum={1}", schema, DevNum));
                return (true);
            }
            return (false);
        }
    }
}
