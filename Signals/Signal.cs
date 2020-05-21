using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPAR_common;

namespace Signals
{
    public class Signal
    {
        public delegate void DWriteSignal(Signal _signal);
        DWriteSignal WriteSignal;

        object SyncObj;

        protected bool val;
        public int board { get; private set; }
        public string name { get; private set; }
        public bool input { get; set ;}
        public bool digital { get; private set; }
        public int position { get; private set; }
        public int PositionOffset
        {
            get
            {
                if (digital)
                    return (digital_offset + position);
                else
                    return (position);
            }
        }
        public string hint { get; set; }
        public string eOn { get; private set; }
        public string eOff { get; private set; }
        public TimeSpan timeout { get; private set; }
        public bool no_reset { get; private set; }
        public bool verbal { get; private set; }
        public bool prev_val { get; private set; }
        public int last_changed { get; private set; }
        public int X;
        public int Y;
        int digital_offset;
        public Signal(int _board, SignalPars _p, int _digital_offset, object _SyncObj, DWriteSignal _WriteSignal)
        {
            board = _board;
            digital_offset = _digital_offset;
            name = _p.Name;
            digital = _p.Digital;
            position = _p.Position;
            hint = _p.Hint;
            eOn = _p.EOn;
            eOff = _p.EOff;
            timeout = new TimeSpan(0, 0, 0, 0, _p.Timeout);
            no_reset = _p.NoReset;
            verbal = _p.Verbal;
            val = false;
            prev_val = val;
            last_changed = Environment.TickCount;
            X = _p.X;
            Y = _p.Y;
            SyncObj = _SyncObj;
            WriteSignal = _WriteSignal;
            input = _p.Input;
        }
        internal void SetVal(bool _val)
        {
            SetVal(_val, Environment.TickCount);
        }
        internal void SetVal(bool _val, int _tick)
        {
            prev_val = val;
            if (_val == val)
                return;
            val = _val;
            last_changed = _tick;
        }
        public bool GetVal()
        {
            return (val);
        }
        public bool Val
        {
            get
            {
                lock (SyncObj)
                {
                    return (val);
                }
            }
            set
            {
                lock (SyncObj)
                {
                    SetVal(value, Environment.TickCount);
                }
                if (!input)
                    WriteSignal(this);
            }
        }
    }
}
