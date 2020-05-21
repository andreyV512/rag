using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SQL
{
    public class Execute
    {
        private SqlCommand cmd;
        private int ret;
        private string SQL;
        public Execute(string _SQL)
        {
            SQL = _SQL;
            cmd = new SqlCommand(_SQL, CDBS.Connection);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlParameter par = new SqlParameter();
            par.Direction=ParameterDirection.ReturnValue;
            par.DbType=DbType.Int32;
            par.ParameterName="@RC";
            cmd.Parameters.Add(par);
        }
        public void Input(string _name,string _value)
        {
            SqlParameter par = new SqlParameter();
            par.Direction=ParameterDirection.Input;
            par.DbType=DbType.String;
            par.ParameterName=_name;
            par.Value=_value;
            cmd.Parameters.Add(par);
        }
        public void Input(string _name,int _value)
        {
            SqlParameter par = new SqlParameter();
            par.Direction=ParameterDirection.Input;
            par.DbType=DbType.Int32;
            par.ParameterName=_name;
            par.Value=_value;
            cmd.Parameters.Add(par);
        }
        public void Input(string _name, Int64 _value)
        {
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.Input;
            par.DbType = DbType.Int64;
            par.ParameterName = _name;
            par.Value = _value;
            cmd.Parameters.Add(par);
        }
        public void Input(string _name, double _value)
        {
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.Input;
            par.DbType = DbType.Double;
            par.ParameterName = _name;
            par.Value = _value;
            cmd.Parameters.Add(par);
        }
        void Output(string _name)
        {
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.Output;
            par.DbType = DbType.String;
            par.ParameterName = _name;
            par.Value = "";
            par.Size = 1024;
            cmd.Parameters.Add(par);
        }
        public void OutputString(string _name, int _size)
        {
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.Output;
            par.DbType = DbType.String;
            par.ParameterName = _name;
            par.Value = "";
            par.Size = _size;
            cmd.Parameters.Add(par);
        }
        public void OutputDouble(string _name)
        {
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.Output;
            par.DbType = DbType.Double;
            par.ParameterName = _name;
            par.Value = 0;
            cmd.Parameters.Add(par);
        }
        public void OutputInt(string _name)
        {
            SqlParameter par = new SqlParameter();
            par.Direction = ParameterDirection.Output;
            par.DbType = DbType.Int32;
            par.ParameterName = _name;
            par.Value = 0;
            cmd.Parameters.Add(par);
        }
        public string AsString(string _name)
        {
            return (cmd.Parameters[_name].Value.ToString());
        }
        public double AsDouble(string _name)
        {
            return ((double)cmd.Parameters[_name].Value);
        }
        public int AsInt(string _name)
        {
            return ((int)cmd.Parameters[_name].Value);
        }
        public int Exec()
        {
            try
            {
                ret = cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw new Exception("Execute:Exec: " + SQL + " : " + e.Message);
            }
            return ((int)cmd.Parameters["@RC"].Value);
        }
        public int RowsAffected()
        {
            return (ret);
        }

    }
}
