using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;


namespace SQL
{
    class CUDL
    {
        public static String GetConnectionString(String _file_name)
        {
            StreamReader sr = new StreamReader(_file_name, Encoding.Unicode);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if(line.StartsWith(";"))
                {
                    line = sr.ReadLine();
                    if(line==null)
                        return ("");
                    string[] m = line.Split(';');
                    string ret = "MultipleActiveResultSets=true";
//                    String aaa = "Integrated Security=SSPI;Persist Security Info=False;User ID=\"\";Initial Catalog=BuranCS;Data Source=RAG\\SQLRAG8W8;Initial File Name=\"\";MultipleActiveResultSets=true";

                    for (int i = 0; i < m.Length; i++)
                    {
                        if (m[i].StartsWith("Integrated Security="))
                        {
                            ret += ";";
                            ret += m[i];
                        }
                        if (m[i].StartsWith("Persist Security Info="))
                        {
                            ret += ";";
                            ret += m[i];
                        }
                        if (m[i].StartsWith("User ID="))
                        {
                            ret += ";";
                            ret += m[i];
                        }
                        if (m[i].StartsWith("Initial Catalog="))
                        {
                            ret += ";";
                            ret += m[i];
                        }
                        if (m[i].StartsWith("Data Source="))
                        {
                            ret += ";";
                            ret += m[i];
                        }
                        if (m[i].StartsWith("Initial File Name="))
                        {
                            ret += ";";
                            ret += m[i];
                        }
                        if (m[i].StartsWith("Password="))
                        {
                            ret += ";";
                            ret += m[i];
                        }
                    }
                    return(ret);
                }
            }
            return ("");
        }
    }
}
