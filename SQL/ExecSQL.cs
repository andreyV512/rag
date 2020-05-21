using System;
using System.Data.SqlClient;
using System.Threading;

namespace SQL
{
    public class ExecSQL:IDisposable
    {
        private int ret;
        public ExecSQL(string _SQL)
        {
            try
            {
                SqlCommand O = new SqlCommand(_SQL, CDBS.Connection);
                ret = O.ExecuteNonQuery();
                O.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("ExecSQL: " + _SQL + " : " + e.Message);
            }
        }
        public int RowsAffected
        {
            get
            {
                return (ret);
            }
        }
        public void Dispose()
        {
            ;
        }
    }
    public class ExecSQLEx
    {
        private int ret;
        public ExecSQLEx(string _SQL)
        {
                SqlCommand O = new SqlCommand(_SQL, CDBS.Connection);
                ret = O.ExecuteNonQuery();
                O.Dispose();
        }
        public int RowsAffected
        {
            get
            {
                return (ret);
            }
        }
        public void Dispose()
        {
            ;
        }
    }
}
