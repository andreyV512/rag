using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BankLib
{
    public class TickPosition
    {
        public TickPosition() { }
        public TickPosition(int _tick, double _position)
        {
            tick = _tick;
            position = _position;
        }
        public TickPosition(TickPosition _tickPosition)
        {
            tick = _tickPosition.tick;
            position = _tickPosition.position;
        }

        public double tick = 0;
        public double position = 0;

        public override string ToString()
        {
            return ("TickPosition: " + tick.ToString() + " " + position.ToString());
        }
    }
    public class L_TickPosition : List<TickPosition>
    {
        public L_TickPosition()
        {
            Capacity = 200;
        }
        public int? startTick = null;
        public new void Add(TickPosition _tickPosition)
        {
            if (startTick == null)
                return;
            TickPosition tp = new TickPosition(_tickPosition);
            tp.tick -= startTick.Value;
            base.Add(tp);
        }
        public void Add(int _tick, int _position)
        {
            Add(new TickPosition(_tick, _position));
        }
        public double? TickByPosition(double _position)
        {
            TickPosition p0 = null;
            TickPosition p1 = null;
            for (int i = 0; i < Count; i++)
            {
                TickPosition p = this[i];
                if (p.position < _position)
                    p0 = p;
                if (_position <= p.position)
                {
                    p1 = p;
                    break;
                }
            }
            if (p0 == null || p1 == null)
                return (null);
            double ret = p0.tick + (_position - p0.position) * (p1.tick - p0.tick) / (p1.position - p0.position);
            if (ret < 0)
                return (null);
            return (ret);
        }
        public new void Clear()
        {
            startTick = null;
            base.Clear();
        }
        public TickPosition Last
        {
            get { return (this[this.Count - 1]); }
        }
    }
}
