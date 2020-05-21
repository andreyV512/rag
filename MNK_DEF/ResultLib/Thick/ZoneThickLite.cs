using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Share;

namespace ResultLib.Thick
{
    public class ZoneThickLite
    {
        public double? Level { get; private set; }
        public int Index { get; private set; }
        public int Length { get; private set; }
        public bool Last { get; private set; }
        public EClass Class { get; private set; }

        ZoneThickLite(int _index)
        {
            Index = _index;
            Length = 0;
            Last = false;
            Level = null;
            Class = EClass.None;
        }
        public ZoneThickLite(int _index, double? _Level, EClass _CLass, int _length, bool _last)
            : this(_index)
        {
            Length = _length;
            Last = _last;
            Level = _Level;
            Class = _CLass;
        }
        public override string ToString()
        {
            return ("ZoneThickLite[" + Index + "]: Length=" + Length.ToString() + (Last ? " последняя" : ""));
        }
    }
}
