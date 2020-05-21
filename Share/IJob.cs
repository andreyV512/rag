using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Share
{
    public delegate void DOnStatus(uint _level, string _msg);
    public delegate void DOnExec(string _msg);
    public interface IJob
    {
        void Exec(int _tick);
        bool IsComplete { get; }
        bool IsError { get; }
        void Finish();
        DOnStatus OnStatus { set; }
        void Start(int _tick);
        string LastError { get; }
    }
    /*
        public void Dispose() { }
        public bool IsComplete { get; private set; }
        public void Start(int _tick){}
        public void Finish() { IsComplete = true; }
        DOnStatus onStatus = null;
        public DOnStatus OnStatus { set { onStatus = value; } }
        public string LastError { get; private set; }  
        public bool IsError { get { return (LastError != null); } }
        public void Exec(int _tick) { }
        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(1, _msg);
        }
   */

}
