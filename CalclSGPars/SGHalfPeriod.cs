using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalclSGPars
{
    public class SGPeriod
    {
        public int start=0;
        public int size=0;
        public SGPeriod() { }
        public SGPeriod(SGPeriod _prev)
        {
            start = _prev.start + _prev.size;
            size = 0;
        }

    }
    public class SGHalfPeriod
    {
        public int start;
        public int size;
        public int index;
        public SGHalfPeriod(int _index, int _start, int _size)
        {
            start = _start;
            size = _size;
            index = _index;
        }
        public SGHalfPeriod()
        {
            start = 0;
            size = 0;
            index = 0;
        }
        public SGHalfPeriod(SGHalfPeriod _prev)
        {
            start = _prev.start + _prev.size;
            size = 0;
            index = _prev.index + 1;
        }
        public override string ToString()
        {
            return (string.Format("Полупериод {0}: начало {1} , длина={2}",
                index.ToString(),
                start.ToString(),
                size.ToString()));
        }
    }
}
