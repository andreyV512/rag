using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PARLIB
{
    public abstract class SerialTree
    {
        public abstract string this[string _path]{get;}
        public abstract List<string> GetList(string _path);
    }
}
