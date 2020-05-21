using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading;

using Share;
using RS232;

using UPAR_common;
using Protocol;

namespace Demagnetizer
{
    public class JDemagnetizer : IJob, IDisposable
    {
        ComPortBase comPort=null;
        bool verbose;
        DemagnetizerTSPars demagnetizerTSPars;
        DemagnetizerPars demagnetizerPars;

        public bool IsComplete { get; private set; }
        DOnStatus onStatus = null;
        public DOnStatus OnStatus { set { onStatus = value; } }
        public string LastError { get { return (state.Error); } }
        public bool IsError { get { return (LastError != null); } }
        public void Exec(int _tick) { }
        State state = new State() { Error = null };
        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(0, "Размагничиватель: " + _msg);
        }
        bool IsOnOff = true;

        public JDemagnetizer(DemagnetizerPars _Demagnetizer, DemagnetizerTSPars _DemagnetizerTSPars, bool _IsOnOff)
        {
            demagnetizerPars = _Demagnetizer;
            demagnetizerTSPars = _DemagnetizerTSPars;
            verbose = _Demagnetizer.Verbose;
            if(demagnetizerPars.InUse)
                comPort = ComPort.Create(_Demagnetizer.ComPort, pr);
            IsComplete = true;
            IsOnOff = _IsOnOff;
        }
        public void ResetError()
        {
            state.Error = null;
        }
        public void Start(int _tick)
        {
            if (!demagnetizerPars.InUse)
                return;
            if (IsComplete)
            {
                SetOnOff(true);
                IsComplete = false;
            }
        }
        public void CheckSet()
        {
            if (!demagnetizerPars.InUse)
                return;
            pr("CheckSet");
            state.Error = null;
            GetState0();
            if (IsError)
                return;
            if (CheckState())
                return;
            SetFrequency(demagnetizerTSPars.Frequency);
            if (IsError)
                return;
            Thread.Sleep(200);
            SetOffset(demagnetizerTSPars.Offset);
        }
        bool CheckState()
        {
            if (state.Frequency != demagnetizerTSPars.Frequency)
                return (false);
            if (demagnetizerTSPars.Offset > 0)
            {
                if (state.OffsetNegative != 0)
                    return (false);
                if (state.OffsetPositive != demagnetizerTSPars.Offset)
                    return (false);
            }
            else
            {
                if (state.OffsetPositive != 0)
                    return (false);
                if (state.OffsetNegative != -demagnetizerTSPars.Offset)
                    return (false);
            }
            return (true);
        }
        public void Finish()
        {
            if (!demagnetizerPars.InUse)
                return;
            if (!IsComplete)
                SetOnOff(false);
            IsComplete = true;
        }
        public void Dispose()
        {
            Finish();
            if (comPort != null)
            {
                comPort.Dispose();
                comPort = null;
            }
        }
        void pr(string _msg)
        {
            if (verbose)
                ProtocolST.pr("JDemagnetizer:" + _msg);
        }
        public void SetOffset(int _offset)
        {
            if (!demagnetizerPars.InUse)
                return;
            if (IsError)
                return;
            for (int i = 0; i < demagnetizerPars.Iters; i++)
            {
                SetOffset0(_offset);
                if (!IsError)
                    break;
                Thread.Sleep(100);
            }
        }
        void SetOffset0(int _offset)
        {
            pr("SetOffset");
            state.Error = null;
            if (_offset == 0)
                return;
            bool positive = _offset > 0;
            string cmd = positive ? "!1" : "!2";
            if (_offset < 0)
                _offset = -_offset;
            cmd += _offset.ToString("D2");
            cmd += "\r\n";
            if (!comPort.Write(Encoding.ASCII.GetBytes(cmd)))
            {
                state.XError = "Не смогли записать";
                return;
            }
            byte[] packet = ReadTail();
            if (packet.Length < 3)
            {
                state.XError = "Не смогли прочитать";
                return;
            }
            string reply = Encoding.ASCII.GetString(packet, 0, 3);
            if (positive)
            {
                if (reply != "OK1")
                {
                    state.XError = "Ошибка установки смещения";
                    return;
                }
            }
            else
            {
                if (reply != "OK2")
                {
                    state.XError = "Ошибка установки смещения";
                    return;
                }
            }
        }
        public void SetFrequency(int _frequency)
        {
            if (!demagnetizerPars.InUse)
                return;
            if (IsError)
                return;
            for (int i = 0; i < demagnetizerPars.Iters; i++)
            {
                SetFrequency0(_frequency);
                if (!IsError)
                    break;
                Thread.Sleep(100);
            }
        }

        void SetFrequency0(int _frequency)
        {
            pr("SetFrequency");
            state.Error = null;
            string cmd = "!3" + _frequency.ToString("D2");
            cmd += "\r\n";
            if (!comPort.Write(Encoding.ASCII.GetBytes(cmd)))
            {
                state.XError = "Не смогли записать";
                return;
            }
            byte[] packet = ReadTail();
            if (packet.Length < 3)
            {
                state.XError = "Не смогли прочитать";
                return;
            }
            string reply = Encoding.ASCII.GetString(packet, 0, 3);
            if (reply != "OK3")
                state.XError = "Ошибка установки частоты";
        }
        public void SetOnOff(bool _On)
        {
            if (!demagnetizerPars.InUse)
                return;
            if (IsError)
                return;
            for (int i = 0; i < demagnetizerPars.Iters; i++)
            {
                SetOnOff0(_On);
                //if (!IsError)
                //    break;
                Thread.Sleep(250);
            }
        }
        void SetOnOff0(bool _On)
        {   
            pr("SetOnOff");
            state.Error = null;
            string cmd = _On ? "!4" : "!5";
            cmd += "\r\n";
            if (!comPort.Write(Encoding.ASCII.GetBytes(cmd)))
            {
                state.XError = "Не смогли записать";
                return;
            }
            return;
            if (!IsOnOff)
                return;
            byte[] packet = ReadTail();
            if (packet.Length < 3)
            {
                state.XError = "Не смогли прочитать";
                return;
            }
            string reply = Encoding.ASCII.GetString(packet, 0, 3);
            if (_On)
            {
                if (reply != "OK4")
                {
                    state.XError = "Ошибка включения";
                    return;
                }
            }
            else
            {
                if (reply != "OK5")
                {
                    state.XError = "Ошибка ВЫключения";
                    return;
                }
            }
        }
        public struct State
        {
            public int Frequency;
            public int OffsetPositive;
            public int OffsetNegative;
            public string Error;
            public string XError { set { Error = "Размагничиватель: " + value; } }
        }
        public State GetState()
        {
            if (!demagnetizerPars.InUse)
                return(state);
            state.Error = null;
            GetState0();
            return (state);
        }
        void GetState0()
        {
            pr("GetState0");
            state.Error = null;
            if (!comPort.Write(Encoding.ASCII.GetBytes("!6\r\n")))
            {
                state.XError = "Не смогли записать";
                return;
            }
            byte[] packet = ReadTail();
            if (packet.Length < 9)
            {
                state.XError = "Не смогли прочитать";
                return;
            }
            
            string reply = Encoding.ASCII.GetString(packet, 0, packet.Length-1);
            if (reply.Substring(0, 4) != "PAR:")
            {
                state.XError = "Не корректный ответ 1";
                return;
            }
            string[] m = reply.Substring(4).Split(';');
            if (m.Length != 3)
            {
                state.XError = "Не корректный ответ 2";
                return;
            }
            int v = 0;
            if (!Int32.TryParse(m[0], out v))
            {
                state.XError = "Не корректный ответ 3";
                return;
            }
            state.Frequency = v;
            if (!Int32.TryParse(m[1], out v))
            {
                state.XError = "Не корректный ответ 4";
                return;
            }
            state.OffsetPositive = v;
            if (!Int32.TryParse(m[2], out v))
            {
                state.XError = "Не корректный ответ 5";
                return;
            }
            state.OffsetNegative = v;
        }
        byte[] ReadTail()
        {
            List<byte> L = new List<byte>();
            while (true)
            {
                byte[] packet = comPort.Read(1);
                if (packet.Length == 0)
                    break;
                L.Add(packet[0]);
                if (packet[0] == 0x0A)
                    break;
            }
            return (L.ToArray());
        }
    }
}
