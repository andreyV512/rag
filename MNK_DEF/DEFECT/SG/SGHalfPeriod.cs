using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defect.SG
{
    public class SGHalfPeriod
    {
        public int start;
        public int size;
        public int index;
        public SGHalfPeriod(int _start, int _size, int _index)
        {
            start = _start;
            size = _size;
            index = _index;
        }
        public override string ToString()
        {
            return(string.Format("Полупериод {0}: начало {1} , длина={2}",
                index.ToString(),
                start.ToString(),
                size.ToString()));
        }
    }
}
