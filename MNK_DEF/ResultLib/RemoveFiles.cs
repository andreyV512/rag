using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using UPAR;
using UPAR_common;
using Protocol;

namespace ResultLib
{
    public static class RemoveFiles
    {
        public static string Remove(SaveFilePars _pars, string _extention)
        {
            if (_pars.Path == null)
                return (null);
            if (_pars.MaxNumber == null)
                return (null);
            if (_pars.MaxNumber.Value == 0)
                return (null);

            DirectoryInfo di = new DirectoryInfo(_pars.Path);
            if (!di.Exists)
                return (null);
            FileInfo[] Lfi = di.GetFiles("*." + _extention);
            if (Lfi.Length <= _pars.MaxNumber.Value)
                return (null);

            Array.Sort(Lfi, (x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.LastWriteTime, y.LastWriteTime));
            //for (int i = 0; i < Lfi.Length; i++)
            //{
            //    pr("--- " + Lfi[i].Name + " " + Lfi[i].LastWriteTime.ToLocalTime());

            //}

            for (int i = 0; i < Lfi.Length-_pars.MaxNumber.Value; i++)
            {
                try
                {
                    pr(Lfi[i].Name + " " + Lfi[i].LastWriteTime.ToLongDateString());
                    Lfi[i].Delete();
                    
                }
                catch(Exception e)
                {
                    return ("Не могу удалить файл: " + Lfi[i].FullName+" "+e.Message);
                }
            }
            return (null);
        }
        public static void RemoveMsg(SaveFilePars _pars, string _extention)
        {
            string ret = Remove(_pars, _extention);
            if (ret == null)
                return;
            MessageBox.Show(ret, "Внимание!!!");
        }
        static void pr(string _msg)
        {
            ProtocolST.pr("RemoveFiles: " + _msg);
        }

    }
}
