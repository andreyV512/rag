using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;

namespace Signals
{
    public class JAlarmList : IJob
    {
        class Item
        {
            public Signal signal;
            public bool val;
            public Item(Signal _signal, bool _val)
            {
                signal = _signal;
                val = _val;
            }
            public bool Ok
            {
                get
                {
                    return (signal.Val == val);
                }
            }
            public string Message
            {
                get
                {
                    if (Ok)
                        return (null);
                    return (string.Format("{0} сигнал {1}",
                        val ? "Пропал" : "Появился",
                        signal.name));
                }
            }
        }
        DOnStatus onStatus = null;
        public DOnStatus OnStatus { set { onStatus = value; } }
        public bool IsError { get { return (LastError != null); } }
        public string LastError { get; private set; }
        public bool IsComplete { get; private set; }

        public void Finish()
        {
            IsComplete = true;
        }
        public void Exec(int _tick)
        {
            if (IsError || IsComplete)
                return;
            foreach (Item item in Items)
            {
                if (!item.Ok)
                {
                    LastError = item.Message;
                    break;
                }
            }
        }

        List<Item> Items = new List<Item>();
        public void Add(Signal _signal, bool _val)
        {
            foreach (Item item in Items)
            {
                if (item.signal == _signal)
                {
                    item.val = _val;
                    return;
                }
            }
            Items.Add(new Item(_signal, _val));
        }
        public void Remove(Signal _signal)
        {
            foreach (Item item in Items)
            {
                if (item.signal == _signal)
                {
                    Items.Remove(item);
                    return;
                }
            }
        }
        public void Clear()
        {
            Items.Clear();
            IsComplete = false;
            LastError = null;
        }
        public void Start(int _tick) { }
    }
}
