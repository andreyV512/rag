using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQL
{
    public class ExecuteX:IDisposable
    {
        SqlCommand cmd;
        SqlDataReader RD = null;
        bool IsRow = false;

        public ExecuteX(string _procedure)
        {
            cmd = new SqlCommand(_procedure, CDBS.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.ReturnValue;
            par.DbType = DbType.Int32;
            par.ParameterName = "@RC";
            cmd.Parameters.Add(par);
        }
        public void Dispose()
        {
            if (RD != null)
            {
                RD.Close();
                RD = null;
            }
        }

        public void Input(string _name, DbType _dbtype, object _value)
        {
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.Input;
            par.ParameterName = _name;
            par.DbType = _dbtype;
            par.Value = _value;
            cmd.Parameters.Add(par);
        }
        public void Output(string _name, DbType _dbtype)
        {
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.Output;
            par.DbType = _dbtype;
            par.ParameterName = _name;
            cmd.Parameters.Add(par);
        }
        //public void Output(string _name, int _size)
        //{
        //    SqlParameter par = new SqlParameter();
        //    par.Direction = ParameterDirection.Output;
        //    par.DbType = DbType.Object;
        //    par.ParameterName = _name;
        //    par.Size = _size;
        //    cmd.Parameters.Add(par);
        //}
        void Exec()
        {
            if (RD != null)
                return;
            try
            {
                RD = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                throw new Exception(cmd.CommandText + " " + e.Message);
            }
        }
        public bool Read()
        {
            Exec();
            IsRow = RD.Read();
            return (IsRow);
        }
        public object Param(string _name)
        {
            return (cmd.Parameters[_name].Value);
        }
        public object this[int _index]
        {
            get
            {
                if (!IsRow)
                    throw new Exception(cmd.CommandText + ": Нет записей в запросе");
                return (RD.GetValue(_index));
            }
        }
        public object this[string _name] { get { return (this[RD.GetOrdinal(_name)]); } }
    }
}
