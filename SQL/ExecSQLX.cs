using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQL
{
    public class ExecSQLX: IDisposable
    {
        private SqlCommand O;
        public ExecSQLX(string _SQL)
        {
            O = new SqlCommand();
            O.Connection = CDBS.Connection;
            O.CommandText = _SQL;
            O.Transaction = CDBS.Transaction;
            Error = null;
        }
        public void Dispose()
        {
            O.Dispose();
        }
        public string Error { get; private set; }
        public int Exec()
        {
            int ret = -1;
            try
            {
                ret = O.ExecuteNonQuery();
            }
            catch (Exception _e)
            {
                Error = O.CommandText + " " + _e.Message;
            }
            O.Dispose();
            return (ret);
        }
        // new ExecSQLX("Insert... values(... @par_name ...)...");
        // AddParam("@par_name",...
        public SqlParameter AddParam(string _name, SqlDbType _tp, Object _o)
        {
            SqlParameter par = new SqlParameter();
            //            par.SqlDbType = _tp;
            par.Value = _o;
            par.ParameterName = _name;
            par.Direction = ParameterDirection.Input;
            O.Parameters.Add(par);
            return (par);
        }
        public SqlParameter AddParam(string _name, SqlDbType _tp, Object _o, int _size)
        {
            SqlParameter par = AddParam(_name, _tp, _o);
            par.Size = _size;
            return (par);
        }
        public SqlParameter AddParamOut(string _name, SqlDbType _tp)
        {
            SqlParameter par = new SqlParameter();
            par.SqlDbType = _tp;
            par.ParameterName = _name;
            par.Direction = ParameterDirection.Output;
            O.Parameters.Add(par);
            return (par);
        }
        public SqlParameter AddParamOut(string _name, SqlDbType _tp, int _size)
        {
            SqlParameter par = AddParamOut(_name, _tp);
            par.Size = _size;
            return (par);
        }
        public object this[string _name]
        {
            get
            {
                return (O.Parameters[_name].Value);
            }
        }
    }
}
