using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQL
{
    public class Select: IDisposable
    {
        private SqlDataReader RD = null;
        bool ret = false;
        private bool disposed = false;
        private SqlCommand cmd;
        public void Dispose()
        {
            if (disposed)
                return;
            disposed = true;
            if (RD != null)
            {
                RD.Close();
                RD.Dispose();
            }
            cmd.Dispose();
        }
        public Select(string _SQL)
        {
            cmd = new SqlCommand(_SQL, CDBS.Connection);
        }
        private void CheckRows()
        {
            if (!ret)
                throw new Exception("Select: CheckRows: Нет записей в запросе: " + cmd.CommandText);
        }
        public bool Read()
        {
            if (RD == null)
                RD = cmd.ExecuteReader();
            ret = RD.Read();
            return (ret);
        }
        public object this[string _name] { get { return (this[RD.GetOrdinal(_name)]); } }
        public object this[int _index]
        {
            get
            {
                CheckRows();
                return (RD.GetValue(_index));
            }
        }
        public SqlParameter AddParam(string _name, SqlDbType _tp, Object _o)
        {
            SqlParameter par = new SqlParameter();
            par.SqlDbType = _tp;
            par.Value = _o;
            par.ParameterName = _name;
            cmd.Parameters.Add(par);
            return (par);
        }
        public SqlParameter AddParam(string _name, SqlDbType _tp, Object _o, int _size)
        {
            SqlParameter par = AddParam(_name, _tp, _o);
            par.Size = _size;
            return (par);
        }
        public string SQL { get { return (cmd.CommandText); } }
    }
}
