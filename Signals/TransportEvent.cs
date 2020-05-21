using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signals
{
    public class TransportEvent
    {
        public bool need { get; private set; }
        protected object signal = null;
        public int Tick { get; protected set; }
        public int Position { get; protected set; }
        public TransportEvent(object _signal, bool _need, int _position)
        {
            Tick = 0;
            signal = _signal;
            need = _need;
            Position = _position;
        }
        public string Name
        {
            get
            {
                if (signal == null)
                    return ("Нет сигнала");
                if (signal is Signal)
                    return ((signal as Signal).name);
                return (null);
            }
        }
        public override string ToString()
        {
            return (string.Format("nm={0} need={1} tick={2} pos={3}",
              Name,
              need ? "true" : "false",
              Tick.ToString(),
              Position.ToString()
              ));
        }
    }
    public class TESignal : TransportEvent
    {
        public bool Was { get; private set; }
        public TESignal(object _signal, bool _need, int _position)
            : base(_signal, _need, _position)
        {
            Was = false;
        }
        public bool Check(SignalEvent _event)
        {
            if (Was)
                return (true);
            if (_event.signal != signal)
                return (false);
            if (_event.value != need)
                return (false);
            Tick = _event.tick;
            Was = true;
            return (true);
        }
        public override string ToString()
        {
            return ("TESignal: " + base.ToString() + " was=" + (Was ? "true" : "false"));
        }
    }
    public class TEStrobe : TransportEvent
    {
        int count = 0;
        int zone_size = 0;
        int start_position = 0;
        public double Speed { get; private set; }
        public TEStrobe(object _signal, bool _need, int _start_position, int _zone_size)
            : base(_signal, _need, 0)
        {
            zone_size = _zone_size;
            start_position = _start_position;
            Speed = 0;
            Tick = 0;
            count = 0;
        }

        public override string ToString()
        {
            return (string.Format("TEStrobe: {0} Vd={1} count={2}", base.ToString(), Speed.ToString("F2"), count.ToString()));
        }
        public bool Check(SignalEvent _event)
        {
            if (_event.signal != signal)
                return (false);
            if (_event.value != need)
                return (false);
            Speed = zone_size;
            Speed /= _event.tick - Tick;
            Tick = _event.tick;
            count++;
            Position = start_position + count * zone_size;
            return (true);
        }
        public int FirstTick { set { Tick = value; } }
    }
}
