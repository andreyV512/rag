using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPAR_common;

namespace Signals.Boards
{
    abstract public class Board
    {
        public delegate void DOnPr(string _msg);
        public DOnPr OnPr = null;
        protected void pr(string _msg)
        {
            if (OnPr != null)
                OnPr(_msg);
        }
        protected bool disposed = false;
        public bool Disposed() { return (disposed); }
        public virtual void Dispose() { disposed = true; }
        protected int values_in;
        protected int values_out;
        public int ValuesIn { get { return (values_in); } }
        public int ValuesOut { get { return (values_out); } }
        public int portCount_in { get; protected set; }
        public int portCount_out { get; protected set; }
        public int Timeout { get; private set; }
        public int DigitalOffset { get; private set; }
        protected Board(PCIE1730pars _pars, DOnPr _OnPr)
        {
            OnPr = _OnPr;
            DigitalOffset = _pars.DigitalOffset;
            portCount_in = _pars.PortcountIn;
            portCount_out = _pars.PortcountOut;
            Timeout = _pars.SignalListTimeout;
            values_in = 0;
            values_out = 0;
        }
        public abstract int Read();
        public abstract int ReadOut();
        public abstract void Write(int _values_out);
        public abstract void WriteIn(int _values_in);
        static public bool GetBit(int _values, int _position)
        {
            return ((_values & (1 << _position)) != 0);
        }
        static public void SetBit(ref int _values, int _position, bool _val)
        {
            if (_val)
                _values |= (1 << _position);
            else
                _values &= (~((1) << _position));
        }
    }
}
