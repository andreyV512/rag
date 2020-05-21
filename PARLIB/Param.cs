using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PARLIB
{
    internal class Param
    {
        public string path;
        public string val;
        public override string ToString()
        {
            string ret=path;
            if (val != null)  
            {
                if (val.Length > 0)
                    ret += "=" + val;
            }
            return (ret);
        }
        public Param(string _path, string _val)
        {
            path = _path;
            val = _val;
        }
        public bool IsZero()
        {
            if (path == null)
                return (true);
            if(path.Length==0)
                return (true);
            return (false);
        }
        public static Param FromFileString(string _line)
        {
            if (_line.Length == 0)
                return(null);
            int index = _line.IndexOf('=');
            if (index < 0)
                return(new Param(_line,null));
            else
                return(new Param(_line.Substring(0, index), _line.Substring(index + 1)));
        }
    }
    internal class LParam : List<Param>
    {
        public void Add(string _path, string _val)
        {
            Add(new Param(_path, _val));
        }
    }
}
