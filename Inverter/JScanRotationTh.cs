using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Share;
using RS232;
using UPAR_common;

namespace InverterNS
{
    public class JScanRotationTh : IJob, IDisposable
    {
        public bool IsError { get { return (LastError != null); } }
        public bool IsComplete { get { return (thread == null); } }
        DOnStatus onStatus = null;
        public DOnStatus OnStatus { set { onStatus = value; } }
        public void Exec(int _tick) { }
        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(1, _msg);
        }


        ComPortBase comPort = null;
        object LockObj = new object();
        Thread thread = null;
        double? currentSpeed = null;
        volatile bool terminate = false;
        string l_lastError = null;
        string lastError = null;


        public JScanRotationTh(ComPortPars _ComPortPars)
        {
            comPort = ComPort.Create(_ComPortPars, pr);
            l_lastError = null;
            lastError = null;
        }
        public void Dispose()
        {
            Finish();
            if (comPort != null)
                comPort.Dispose();
            comPort = null;
        }
        public string LastError
        {
            get { lock (LockObj) { return (lastError); } }
        }
        double? GetSpeed()
        {
            l_lastError = null;
            byte[] b = comPort.ReadSome(7);
            string s = Encoding.Default.GetString(b);
            if (s.Length != 7)
            {
                pr("Не верная длина пакета: " + s.Length.ToString());
                return (null);
            }
            if (s[0] != '!')
            {
                pr("Нет начала: !");
                return (null);
            }
            if (s[s.Length - 2] != Convert.ToChar(0xD))
            {
                pr("Нет 0xD");
                return (null);
            }
            if (s[s.Length - 1] != Convert.ToChar(0xA))
            {
                pr("Нет 0xA");
                return (null);
            }
            if (s.Length == 7)
            {
                if (s.Substring(1, 4) == "stop")
                    return (0);
                string s_speed = s.Substring(1, 4).Replace(".", ",");
                double speed = 0;
                if (!Double.TryParse(s_speed, out speed))
                {
                    pr("Не смогли интерпретировать скорость: " + s_speed);
                    return (null);

                }
                return (speed);
            }
            return (null);
        }
        public void pr(string _msg)
        {
            Protocol.ProtocolST.pr("JScanRotationTh: " + _msg);
        }
        public void Start(int _tick)
        {
            if (thread != null)
                return;
            currentSpeed = 0;
            thread = new Thread(new ThreadStart(Run));
            thread.Start();
        }
        public void Finish()
        {
            if (thread == null)
                return;
            terminate = true;
            thread.Join();
            thread = null;
            currentSpeed = 0;
        }
        void Run()
        {
            for (; ; )
            {
                if (terminate)
                    break;
                double? lspeed = GetSpeed();
                lock (LockObj)
                {
                    currentSpeed = lspeed;
                    lastError = l_lastError;
                }
            }
        }

        public double? Speed
        {
            get { lock (LockObj) { return (currentSpeed); } }
        }
    }
}