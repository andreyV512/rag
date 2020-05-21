using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Signals
{
    public class SignalEvent
    {
        public object signal=null;
        public int tick=0;
        public bool value=false;
        public override string ToString()
        {
            if (signal == null)
                return ("SignalEvent: пусто");
            if(signal is Signal)
                return (string.Format("SignalEvent: tick={0} value={1} name={2}", tick.ToString(), value ? "true" : "false", (signal as Signal).name));
            return ("SignalEvent: неизвестный сигнал");
        }
    }
    public class CatchSignals
    {
        List<Signal> L = new List<Signal>();
        Queue<SignalEvent> E = null;

        public SignalEvent Next()
        {
            if (E == null)
                return (null);
            if (E.Count == 0)
                return (null);
            return (E.Dequeue());
        }
        public void Clear()
        {
            if (E != null)
                E.Clear();
        }
        public void Start()
        {
            if (E == null)
                E = new Queue<SignalEvent>();
            else
                E.Clear();
        }
        public void Stop()
        {
            E = null;
        }
        public void AddSignal(Signal _signal)
        {
            foreach (Signal s in L)
            {
                if (s == _signal)
                    return;
            }
            L.Add(_signal);
        }
        public void Exec()
        {
            if (E == null)
                return;
            foreach (Signal s in L)
            {
                if (s.prev_val != s.GetVal())
                {
                    SignalEvent e = new SignalEvent();
                    e.signal = s;
                    e.tick = s.last_changed;
                    e.value = s.GetVal();
                    E.Enqueue(e);
                }
            }
        }
    }
}
