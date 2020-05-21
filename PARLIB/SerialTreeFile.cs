using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data.SQLite;

namespace PARLIB
{
    class SerialTreeFile: SerialTree
    {
        SQLiteConnection connection;
        public SerialTreeFile(LParam _L)
        {
            connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();
            string SQL = "CREATE TABLE LST (path TEXT PRIMARY KEY NOT NULL, val  TEXT)";
            using (SQLiteCommand p = new SQLiteCommand(SQL, connection)) p.ExecuteNonQuery();
            Load(_L);
//            new SerialTreeSQL("Uran").Save(_L);
        }
        public void Dispose()
        {
            if (connection == null)
                return;
            connection.Close();
            connection.Dispose();
            connection = null;
        }
        void Load(LParam _L)
        {
            foreach (Param p in _L)
            {
                if (p.IsZero())
                    break;
                string SQL;
                if (p.val==null || p.val.Length==0)
                    SQL = string.Format("insert into LST (path) values('{0}')", p.path);
                else
                    SQL = string.Format("insert into LST values('{0}','{1}')", p.path, p.val);
                using (SQLiteCommand cmd = new SQLiteCommand(SQL, connection))
                {
                    if (cmd.ExecuteNonQuery() != 1)
                        FN.fatal("Ошибка:" + SQL);
                }
            }
        }
        public override string this[string _path]
        {
            get
            {
                using (SQLiteCommand c = new SQLiteCommand(string.Format("select val,path from LST where path='{0}' order by path", _path), connection))
                {
                    SQLiteDataReader rd = c.ExecuteReader();
                    string ret = null;
                    while (rd.Read())
                    {
                        if (rd.GetValue(0).GetType() == typeof(DBNull))
                            return (null);
                        ret = rd.GetString(0);
                    }
                    rd.Close();
                    return (ret);
                }
            }
        }
        public override List<string> GetList(string _path)
        {
            List<string> L = new List<string>();
            using (SQLiteCommand c = new SQLiteCommand(string.Format("select path from LST where path like '{0}[%]' and path not like '{0}[%]%]%' order by path", _path), connection))
            {
                SQLiteDataReader rd = c.ExecuteReader();
                while (rd.Read())
                    L.Add(rd.GetString(0));
                rd.Close();
            }
            return (L);
        }
    }
}
