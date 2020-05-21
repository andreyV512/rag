using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace SQL
{
    public class CDBS
    {
        static CDBS Instance = null;
        SqlConnection sqlConnection;
        SqlTransaction tr = null;
        public static SqlTransaction Transaction
        {
            get
            {
                if (Instance == null)
                    Instance = new CDBS();
                return (Instance.tr);
            }
        }

        public static void Create(string _fname_udl)
        {
            Instance = new CDBS(_fname_udl);
        }
        CDBS() : this(Path.ChangeExtension(Application.ExecutablePath, "udl")) { }
        CDBS(string _fname_udl)
        {
            try
            {
                sqlConnection = new SqlConnection(CUDL.GetConnectionString(_fname_udl));
                sqlConnection.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                Environment.Exit(-1);
            }
        }
        public static SqlConnection Connection
        {
            get
            {
                if (Instance == null)
                    Instance = new CDBS();
                return (Instance.sqlConnection);
            }
        }
        public static void BeginTransaction()
        {
            if (Instance == null)
                Instance = new CDBS();
            if (Instance.tr != null)
            {
                Instance.tr.Commit();
                Instance.tr.Dispose();
            }
            Instance.tr = Instance.sqlConnection.BeginTransaction();
        }
        public static void EndTransaction()
        {
            Commit();
        }
        public static void Commit()
        {
            if (Instance == null)
                Instance = new CDBS();
            if (Instance.tr != null)
            {
                Instance.tr.Commit();
                Instance.tr.Dispose();
                Instance.tr = null;
            }
        }
        public static void RollBack()
        {
            if (Instance == null)
                Instance = new CDBS();
            if (Instance.tr != null)
            {
                Instance.tr.Rollback();
                Instance.tr.Dispose();
                Instance.tr = null;
            }
        }
        public static void Close()
        {
            if (Instance == null)
                return;
            Instance.sqlConnection.Close();
            Instance.sqlConnection.Dispose();
            Instance = null;
        }
    }
}
