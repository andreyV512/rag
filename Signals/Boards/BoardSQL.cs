using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PARLIB;
using UPAR_common;
using SQL;

namespace Signals.Boards
{
    public class BoardSQL : Board
    {
        string schema;
        bool PCSide;
        int Devnum;
        public BoardSQL(int _board, PCIE1730pars _par, string _schema, bool _PCSide, DOnPr _OnPr = null)
            : base(_par, _OnPr)
        {
            schema = _schema;
            PCSide = _PCSide;
            Devnum = _board;
            CheckTables();
        }
        private void CheckTables()
        {
            Select S;
            int nn;
            S = new Select(string.Format(
"select count(*) as nn from INFORMATION_SCHEMA.TABLES where table_type='BASE TABLE' and TABLE_SCHEMA='{0}' and table_name='BoardSQL'"
            , schema));
            if (!S.Read())
                FN.fatal("BoardSQL.CheckTables: " + S.SQL + " - не нашли записей");
            nn = Convert.ToInt32(S["nn"]);
            S.Dispose();
            if (nn != 1)
            {
                new ExecSQL(string.Format(
                    "CREATE TABLE {0}.BoardSQL(" +
                    "   DevNum int," +
                    "	Input varchar(32) NULL," +
                    "	Output varchar(32) NULL" +
                    ") ON [PRIMARY]"
                , schema));
            }
            S = new Select(string.Format("select count(*) as nn from {0}.BoardSQL where DevNum={1}", schema,Devnum));
            S.Read();
            nn = Convert.ToInt32( S["nn"]);
            S.Dispose();
            if (nn < 1)
                new ExecSQL(string.Format("insert into {0}.BoardSQL values({1},replicate('.',32),replicate('.',32))", schema, Devnum.ToString()));
        }
        string InputSide { get { return (PCSide ? "Input" : "OutPut"); } }
        string OutputSide { get { return (PCSide ? "OutPut" : "Input"); } }
        public override int Read()
        {
            Select S = new Select(string.Format("select {0} from {1}.BoardSQL where DevNum={2}", InputSide, schema, Devnum));
            if (!S.Read())
                FN.fatal("BoardSQL.Read: " + S.SQL + " - не нашли записей");
            string sval = S[0] as string;
            values_in = 0;
            int i = 0;
            foreach (char c in sval)
                SetBit(ref values_in, i++, c == '1');
            S.Dispose();
            return (values_in);
        }
        public override int ReadOut()
        {
            Select S = new Select(string.Format("select {0} from {1}.BoardSQL where DevNum={2}", OutputSide, schema, Devnum.ToString()));
            if (!S.Read())
                FN.fatal("BoardSQL.ReadOut: " + S.SQL + " - не нашли записей");
            string sval = S[0] as string;
            values_out = 0;
            int i = 0;
            foreach (char c in sval)
                SetBit(ref values_out, i++, c == '1');
            S.Dispose();
            return (values_out);
        }
        public override void Write(int _values_out)
        {
            if (values_out == _values_out)
                return;
            values_out = _values_out;
            string sval = "";
            for (int i = 0; i < 32; i++)
                sval += GetBit(values_out, i) ? "1" : ".";
            new ExecSQL(string.Format("update {0}.BoardSQL set {1}='{2}' where DevNum={3}", schema, OutputSide, sval, Devnum.ToString()));
        }
        public override void WriteIn(int _values_in)
        {
            values_in = _values_in;
            string sval = "";
            for (int i = 0; i < 32; i++)
                sval += GetBit(values_in, i) ? "1" : ".";
            new ExecSQL(string.Format("update {0}.BoardSQL set {1}='{2}' where DevNum={3}", schema, InputSide, sval, Devnum.ToString()));
        }
    }
}
