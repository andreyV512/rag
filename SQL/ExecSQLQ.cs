using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace SQL
{
    public class ExecSQLQ
    {
        private SqlCommand O;
        private SqlTransaction tr;
        public ExecSQLQ(string _SQL)
        {
            tr = null;
            try
            {
                tr = CDBS.Connection.BeginTransaction();
                O = new SqlCommand();
                O.Connection = CDBS.Connection;
                O.Transaction = tr;
                O.CommandText = _SQL;
            }
            catch (Exception e)
            {
                throw new Exception("ExecSQLQ:ExecSQLQ: " + _SQL + " : " + e.Message);
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
            RollBack();
            O.Dispose();
        }
        public void AddParam(DbType _tp)
        {
            SqlParameter par = new SqlParameter();
            par.DbType = _tp;
            O.Parameters.Add(par);
        }
        public void SetParam(int _index,Object _o)
        {
            O.Parameters[_index].Value = _o;
        }
        public void Commit()
        {
            if (tr == null)
                return;
            tr.Commit();
            tr.Dispose();
            tr=null;
        }
        public void RollBack()
        {
            tr.Rollback();
            tr.Dispose();
        }
    }
}
