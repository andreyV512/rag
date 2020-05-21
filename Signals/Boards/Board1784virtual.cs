using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signals.Boards
{
    class Board1784virtual
    {
        public int DevNum { get; private set; }
        public bool PCSide { get; private set; }
        public Board1784virtual(int _DevNum, bool _PCSide)
        {
            DevNum = _DevNum;
            PCSide = _PCSide;
        }
        public virtual int[] Read()
        {
            return (new int[2] { 0, 0 });
        }
        public virtual void Reset() { }

        public virtual void Write(int[] _vals) { }
        public virtual bool Exec() { return (false); }
        
    }
}
