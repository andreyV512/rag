using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace SQL
{
    public class ExecSQLP
    {
        private SqlCommand O;
        public ExecSQLP(string _SQL)
        {
            try
            {
                O = new SqlCommand();
                O.Connection = CDBS.Connection;
                O.CommandText = _SQL;
            }
            catch (Exception e)
            {
                throw new Exception("ExecSQLQ:ExecSQLP: " + _SQL + " : " + e.Message);
            }
        }
        public int Exec()
        {
            try
            {
                return (O.ExecuteNonQuery());
            }
            catch (Exception e)
            {
                throw new Exception("ExecSQLQ:Exec: " + O.CommandText + " : " + e.Message);
            }
        }
        public void Dispose()
        {
            O.Dispose();
        }
        public void AddParam(DbType _tp)
        {
            SqlParameter par = new SqlParameter();
            par.DbType = _tp;
            O.Parameters.Add(par);
        }
        public void AddParam(string _name,SqlDbType _tp,int _size)
        {
            SqlParameter par = new SqlParameter();
            par.SqlDbType = _tp;
            par.Size = _size;
            par.ParameterName = _name;
            O.Parameters.Add(par);
        }
        public void AddParamOut(string _name, SqlDbType _tp)
        {
            SqlParameter par = new SqlParameter();
            par.DbType = DbType.Object;
            par.SqlDbType = _tp;
            par.ParameterName = _name;
            par.Direction = ParameterDirection.Output;
            O.Parameters.Add(par);
        }
        public void SetParam(string _name, Object _o)
        {
            O.Parameters[_name].Value = _o;
        }
    }
}
