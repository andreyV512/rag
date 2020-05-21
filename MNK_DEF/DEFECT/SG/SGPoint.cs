using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defect.SG
{
    public class SGPoint
    {
        public int start;
        public int size;
        public int par;
        public int tresh;
        public SGPoint(int _start,int _size, int _par,int _tresh)
        {
            start = _start;
            size = _size;
            par = _par;
            tresh = _tresh;
        }
        public override string ToString()
        {
            return(string.Format("Точка[{0}]: tresh={1} start={2} size={3} ",
                par.ToString(),
                tresh.ToString(),
                start.ToString(),
                size.ToString()));
        }
        public int Position { get { return (start + tresh); } }

    }
}
