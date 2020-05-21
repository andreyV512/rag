using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace PARLIB
{
    class MetaTreeFile : MetaTree
    {
        public MetaTreeFile(string _file_tree)
        {
            file_tree = _file_tree;
        }
        string file_tree = null;
        public override void Load(ParMainLite _O)
        {
            L.Clear();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(file_tree, Encoding.GetEncoding(1251));
                while (true)
                {
                    string l = sr.ReadLine();
                    if (l == null)
                        break;
                    Param p = Param.FromFileString(l);
                    if (p != null)
                        L.Add(p);
                }
            }
            catch
            {
                return;
            }
            finally
            {
                if (sr != null)
                    sr.Dispose();
            }
            SerialTreeFile serialTree = new SerialTreeFile(L);
            ExecLoad(serialTree, _O, (_O as ParBase).PropertyName);
            serialTree.Dispose();
        }
        public override void Save(ParMainLite _O)
        {
            L.Clear();
            ExecSave(_O, (_O as IParentBase).PropertyName);
            string file_save = file_tree;
            if (_O.ArchivePars)
                file_save=Path.ChangeExtension(file_tree, "tree1");
            using (StreamWriter sw = new StreamWriter(file_save, false, Encoding.GetEncoding(1251)))
            {
                foreach (Param p in L)
                    sw.WriteLine(p.ToString());
                sw.Flush();
            }
            if (_O.ArchivePars)
                RenameFiles(file_save, file_tree);
        }
        void RenameFiles(string _fname_buf, string _fname_current)
        {
            if (СompareFiles(_fname_current, _fname_buf))
            {
                File.Delete(_fname_buf);
            }
            else
            {
                if (File.Exists(_fname_current))
                {
                    string fname_date = Path.GetFileNameWithoutExtension(_fname_current) + File.GetLastWriteTime(_fname_current).ToString("_yyMMdd_HHmmss") + ".tree";
                    File.Delete(fname_date);
                    File.Move(_fname_current, fname_date);
                }
                File.Move(_fname_buf, _fname_current);
            }
        }
        bool СompareFiles(string _fname0, string _fname1)
        {
            StreamReader sr0;
            try
            {
                sr0 = new StreamReader(_fname0);
            }
            catch
            {
                return (false);
            }
            StreamReader sr1;
            try
            {
                sr1 = new StreamReader(_fname1);
            }
            catch
            {
                sr0.Close();
                sr0.Dispose();
                return (false);
            }
            while (!sr0.EndOfStream && !sr1.EndOfStream)
            {
                string s0 = sr0.ReadLine();
                string s1 = sr1.ReadLine();
                if (s0 != s1)
                {
                    sr0.Close();
                    sr1.Close();
                    sr0.Dispose();
                    sr1.Dispose();
                    return (false);
                }
            }
            sr0.Close();
            sr1.Close();
            sr0.Dispose();
            sr1.Dispose();
            return (true);
        }
    }
}
