using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Share;
using UPAR_common;
using Protocol;

namespace InverterNS
{
    public class JInverterTh : IJob, IDisposable
    {
        object Sync = new object();
        public string LastError0=null;
        string lastError = null;
        public string LastError
        {
            private set { lock (Sync) { lastError = value; } }
            get { lock (Sync) { return (lastError); } }
        }
        public bool IsError { get { return (LastError != null); } }
        public bool IsComplete { get { return (th == null); } }
        DOnStatus onStatus = null;
        public DOnStatus OnStatus { set { onStatus = value; } }
        public void Exec(int _tick) { }
        StateIn sIn;

        void prs(string _msg)
        {
            if (onStatus != null)
                onStatus(0, "Инвертер: " + _msg);
            pr(_msg);
        }
        void pr(string _msg)
        {
            ProtocolST.pr("JInverterTh: " + _msg);
        }
        MitCOM mitcom;
        int abonent;
        int timeout;
        int frequencyPosition;
        public int frequency;
        bool IsABC;
        int Iters;
        ThreadStart ts;
        Thread th;
        volatile bool terminate = false;

        public JInverterTh(ComPortPars _comPortPars, ConverterPars _converterPars, int _Frequency)
        {
//            mitcom = new MitCOM(_comPortPars, pr);
            mitcom = new MitCOM(_comPortPars, null);
            abonent = _converterPars.Abonent;
            timeout = _comPortPars.Timeout;
            frequencyPosition = _converterPars.SpeedPar;
            frequency = _Frequency;
            IsABC = _converterPars.IsABC;
            Iters = _converterPars.Iters;
            RTimeout = 500;
            ts = new ThreadStart(Run);
            th = null;
        }
        public void Dispose()
        {
            Finish();
            mitcom.Dispose();
        }
        //! @brief Устанавливает скорость в один из параметров
        //! @param number - номер пункта в частотнике ( 4,5,6 )
        //! @param value  - значение в Гц
        void setParameterFrequency0(int _frequency)
        {
            setParameterFrequency0(frequencyPosition, _frequency);
        }
        void setParameterFrequency0(int _number, int _frequency)
        {
            if ((_number < 4) || (_number > 6))
            {
                LastError0="Не верный номер ячейки частоты";
                return;
            }
            pr("setParameterFrequency: Начали");
            string av = (_frequency * 100).ToString("X4");
            mitcom.Clear();
            mitcom.AddReply(Reply.EType.C);
            mitcom.AddReply(Reply.EType.D);
            Reply.EType reply;
            string data;
            mitcom.Exec(abonent, timeout, (80 + _number).ToString(), Request.Etype.A, av, out reply, out data);
            if (reply == Reply.EType.None)
            {
                LastError0="Установка частоты: " + data;
                return;
            }
            if (reply != Reply.EType.C)
            {
                LastError0="Установка частоты: Ответ " + reply.ToString() + " , а должен быть C ; " + data;
                return;
            }
        }
        //! @param number номер пункта пч (4,5,6)
        int getParameterFrequency0()
        {
            return (getParameterFrequency0(frequencyPosition));
        }
        int getParameterFrequency0(int _number)
        {
            if ((_number < 4) || (_number > 6))
                return (-1);
            string av = _number.ToString("D2");
            mitcom.Clear();
            mitcom.AddReply(Reply.EType.E);
            mitcom.AddReply(Reply.EType.D);
            Reply.EType reply;
            string data;
            mitcom.Exec(abonent, timeout, "0" + (_number).ToString(), Request.Etype.B, av, out reply, out data);
            if (reply == Reply.EType.None)
            {
                LastError0="Запрос частоты: " + data;
                return(-1);
            }
            if (reply != Reply.EType.E)
            {
                LastError0="Запрос частоты: Ответ " + reply.ToString() + " , а должен быть E ; " + data;
                return (-1);
            }
            return (Convert.ToInt32(data, 16) / 100);
        }
        void SetMode0(int _mode)
        {
            pr("SetMode: Начали");
            string av = _mode.ToString("X4");
            mitcom.Clear();
            mitcom.AddReply(Reply.EType.C);
            mitcom.AddReply(Reply.EType.D);
            Reply.EType reply;
            string data;
            mitcom.Exec(abonent, timeout, "FB", Request.Etype.A, av, out reply, out data);
            if (reply == Reply.EType.None)
            {
                LastError0="Установка режима: " + data;
                return;
            }
            if (reply != Reply.EType.C)
            {
                LastError0="Установка режима: Ответ " + reply.ToString() + " , а должен быть C ; " + data;
                return;
            }
        }
        int GetMode0()
        {
            pr("GetMode: Начали");
            mitcom.Clear();
            mitcom.AddReply(Reply.EType.E);
            mitcom.AddReply(Reply.EType.D);
            Reply.EType reply;
            string data;
            mitcom.Exec(abonent, timeout, "7B", Request.Etype.B, null, out reply, out data);
            if (reply == Reply.EType.None)
            {
                LastError0="Получение режима: " + data;
                return (-1);
            }
            if (reply != Reply.EType.E)
            {
                LastError0="Получение режима: Ответ " + reply.ToString() + " , а должен быть E ; " + data;
                return (-1);
            }
            return (Convert.ToInt32(data, 16));
        }
        //! Переводит ПЧ в режим NET
        void NETManage0()
        {
            SetMode0(0);
        }
        void Reset0()
        {
            Reset0("9966");
        }
        void Reset0(string _registr)
        {
            pr("Reset: Начали");
            mitcom.Clear();
            mitcom.AddReply(Reply.EType.C);
            mitcom.AddReply(Reply.EType.D);
            Reply.EType reply;
            string data;
            mitcom.Exec(abonent, timeout, "FD", Request.Etype.A, _registr, out reply, out data);
            if (reply == Reply.EType.None)
            {
                LastError0="Сброс ошибок: " + data;
                return;
            }
            if (reply != Reply.EType.C)
            {
                LastError0="Сброс ошибок: Ответ " + reply.ToString() + " , а должен быть C ; " + data;
                return;
            }
        }
        //! Считывает все значения, нужно для проверки в отдельном потоке
        StateIn StateRead0()
        {
            pr("StateRead: Начали");
            mitcom.Clear();
            mitcom.AddReply(Reply.EType.E1);
            mitcom.AddReply(Reply.EType.D);
            Reply.EType reply;
            string data;
            mitcom.Exec(abonent, timeout, "7A", Request.Etype.B, null, out reply, out data);
            if (reply == Reply.EType.None)
            {
                LastError0= "Чтение состояния: " + data;
                return (null);
            }
            if (reply != Reply.EType.E1)
            {
                LastError0="Чтение состояния: Ответ " + reply.ToString() + " , а должен быть E1 ; " + data;
                return (null);
            }
            return (new StateIn(data));
        }
        void StateWrite0(StateOut _stateOut)
        {
            pr("StateWrite: Начали");
            mitcom.Clear();
            mitcom.AddReply(Reply.EType.C);
            mitcom.AddReply(Reply.EType.D);
            Reply.EType reply;
            string data;
            mitcom.Exec(abonent, timeout, "FA", Request.Etype.A1, _stateOut.Data, out reply, out data);
            if (reply == Reply.EType.None)
            {
                LastError0="Запись состояния: " + data;
                return;
            }
            if (reply != Reply.EType.C)
            {
                LastError0="Запись состояния: Ответ " + reply.ToString() + " , а должен быть C ; " + data;
                return;
            }
        }

        //! Включает вращение
        public void Start(int _tick)
        {
            if (th != null)
                return;
            terminate = false;
            th = new Thread(ts);
            th.Start();
        }
        void Run()
        {
            LastError0 = null;
            for (int i = 0; i < Iters; i++)
            {
                LastError0=null;
                Reset0("9966");
                if (LastError0 == null)
                    break;
                if (terminate)
                    return;
            }
            if (LastError0 != null)
            {
                LastError = LastError0;
                return;
            }
            Thread.Sleep(1000);
            if (terminate)
                return;
            for (int i = 0; i < Iters; i++)
            {
                LastError0 = null;
                setParameterFrequency0(frequency);
                if (LastError0 == null)
                    break;
                if (terminate)
                    return;
            }
            if (LastError0 != null)
            {
                LastError = LastError0;
                return;
            }
            for (int i = 0; i < Iters; i++)
            {
                LastError0 = null;
                StateWrite0(new StateOut() { STF = true, RH = true });
                if (LastError0 == null)
                    break;
                if (terminate)
                    return;
            }
            if (LastError0 != null)
            {
                LastError = LastError0;
                return;
            }
            Thread.Sleep(50);
            if (terminate)
                return;
            for (; ; )
            {
                if (terminate)
                    break;
                sIn = StateRead0();
                if (IsABC)
                {
                    if (sIn == null || !sIn.ABC)
                    {
                        LastError="Ошибка ABC";
                        break;
                    }
                }
                if (terminate)
                    break;
                Thread.Sleep(RTimeout);
                if (terminate)
                    break;
            }
            StateWrite0(new StateOut() { STF = false, RH = false });
        }

        //! Останавливает вращение
        public void Finish()
        {
            if (th == null)
                return;
            terminate = true;
            th.Join();
            th = null;
        }
        public class StateIn
        {
            byte state;

            public bool RUN { get { return ((state & (1 << 0)) != 0); } }
            public bool STF { get { return ((state & (1 << 1)) != 0); } }
            public bool STR { get { return ((state & (1 << 2)) != 0); } }
            public bool SU { get { return ((state & (1 << 3)) != 0); } }
            public bool OL { get { return ((state & (1 << 4)) != 0); } }
            public bool FU { get { return ((state & (1 << 6)) != 0); } }
            public bool ABC { get { return ((state & (1 << 7)) != 0); } }
            public StateIn(string _data)
            {
                state = Convert.ToByte(_data, 16);
            }
            public StateIn(byte _state)
            {
                state = _state;
            }
            public StateIn(StateIn _src)
            {
                if (_src == null)
                    state = 0;
                else
                    state = _src.state;
            }
            public override string ToString()
            {
                string ret = "";
                ret += "RUN" + (RUN ? "1" : "0") + ",";
                ret += "STF" + (STF ? "1" : "0") + ",";
                ret += "STR" + (STR ? "1" : "0") + ",";
                ret += "SU" + (SU ? "1" : "0") + ",";
                ret += "OL" + (OL ? "1" : "0") + ",";
                ret += "FU" + (FU ? "1" : "0") + ",";
                ret += "ABC" + (ABC ? "1" : "0");
                return ret;
            }
        }
        public class StateOut
        {
            public bool STF = false;
            public bool STR = false;
            public bool RL = false;
            public bool RM = false;
            public bool RH = false;
            public bool RT = false;
            public bool MRS = false;
            public string Data
            {
                get
                {
                    byte state = 0;
                    if (STF)
                        state |= (1 << 1);
                    if (STR)
                        state |= (1 << 2);
                    if (RL)
                        state |= (1 << 3);
                    if (RM)
                        state |= (1 << 4);
                    if (RH)
                        state |= (1 << 5);
                    if (RT)
                        state |= (1 << 6);
                    if (MRS)
                        state |= (1 << 7);
                    return (state.ToString("X2"));
                }
            }
        }
        public int RTimeout { get; set; }
        public StateIn State { get { lock (Sync) { return (new StateIn(sIn)); } } }
        public StateIn StateRead()
        {
            if (th != null)
            {
                LastError = "Нельзя получать значения при включенном вращении";
                return (null);
            }
            LastError0 = null;
            StateIn ret = StateRead0();
            LastError = LastError0;
            return (ret);
        }
        public int getParameterFrequency()
        {
            if (th != null)
            {
                LastError = "Нельзя получать значения при включенном вращении";
                return (-1);
            }
            LastError0 = null;
            int ret = getParameterFrequency0();
            LastError = LastError0;
            return (ret);
        }
        public bool setParameterFrequency(int _frequency)
        {
            if (th != null)
            {
                LastError = "Нельзя получать значения при включенном вращении";
                return (false);
            }
            LastError0 = null;
            setParameterFrequency0(_frequency);
            LastError = LastError0;
            return (IsError);
        }
        public bool NETManage()
        {
            if (th != null)
            {
                LastError = "Нельзя получать значения при включенном вращении";
                return (false);
            }
            LastError0 = null;
            NETManage0();
            LastError = LastError0;
            return (IsError);
        }
        public bool Reset()
        {
            if (th != null)
            {
                LastError = "Нельзя получать значения при включенном вращении";
                return (false);
            }
            LastError0 = null;
            Reset0();
            LastError = LastError0;
            return (IsError);
        }
    }

}
