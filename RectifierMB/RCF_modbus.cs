using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Protocol;
using UPAR_common;

namespace RectifierNS
{
    public class RCF_modbus : IDisposable
    {
        object Sync = new object();
        ModBus modBus;
        bool verbose;
        int Period = 100;
        int Abonent;
        RectifierPars rectifierPars;
        public RCF_modbus(RectifiersPars _rectifiersPars, RectifierPars _rectifierPars, bool _verbose)
        {
            Abonent = _rectifiersPars.Abonent;
            rectifierPars = _rectifierPars;
            verbose = _verbose;
            modBus = new ModBus(_rectifiersPars, verbose);
        }
        public void Dispose()
        {
            modBus.Dispose();
        }
        public string Start()
        {
            string ret;
            lock (Sync)
            {
                //ret = SetMode(_rectifierPars);
                //if (ret != null)
                //    return (ret);
                //Thread.Sleep(Period);
                ret = SetTimeout();
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                ret = SetNominal();
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                ret = SetMaximum();
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                return (OnOff(true));
            }
        }
        public string Stop()
        {
            lock (Sync)
            {
                return (OnOff(false));
            }
        }

        public UIT GetUIT()
        {
            lock (Sync)
            {
                UIT ret = new UIT();
                GetUIT0(ret);
                GetUIT1(ret);
                return (ret);
            }
        }
        public UIT GetUITshort()
        {
            lock (Sync)
            {
                UIT ret = new UIT();
                GetUIT0(ret);
                return (ret);
            }
        }
        void GetUIT0(UIT _ret)
        {
                _ret.error = modBus.ReadInput(Abonent, 1, ref _ret.U);
                if (_ret.error != null)
                    return;
                Thread.Sleep(Period);
                _ret.error = modBus.ReadInput(Abonent, 2, ref _ret.I);
                if (_ret.error != null)
                    return;
        }
        void GetUIT1(UIT _ret)
        {
            _ret.error = modBus.ReadInput(Abonent, 6, ref _ret.Sec);
            if (_ret.error != null)
                return;
            Thread.Sleep(Period);
            _ret.error = modBus.ReadInput(Abonent, 7, ref _ret.Min);
            if (_ret.error != null)
                return;
            Thread.Sleep(Period);
            _ret.error = modBus.ReadInput(Abonent, 8, ref _ret.Hour);
        }
        void pr(string _msg)
        {
            ProtocolST.pr(_msg);
        }
        string SetMode()
        {
            pr("SetMode");
            if (rectifierPars.TpIU == EIU.ByU)
            {
//                string ret = modBus.WriteCheck(_rectifierPars.Abonent, 60, true);
                string ret = modBus.Write(Abonent, 60, true);
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 61, false));
                return (modBus.Write(Abonent, 61, false));
            }
            else
            {
//                string ret = modBus.WriteCheck(_rectifierPars.Abonent, 61, true);
                string ret = modBus.Write(Abonent, 61, true);
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 60, false));
                return (modBus.Write(Abonent, 60, false));
            }

        }
        string SetTimeout()
        {
            pr("SetTimeout");
            double v = rectifierPars.Timeout;
            int Hour = Convert.ToInt32(Math.Floor(v / (60 * 60)));
            v -= Hour * 60 * 60;
            int Min = Convert.ToInt32(Math.Floor(v / 60));
            int Sec = Convert.ToInt32(Math.Floor(v - Min * 60));

            if (rectifierPars.TpIU == EIU.ByU)
            {
                //                string ret = modBus.WriteCheck(_rectifierPars.Abonent, 59, Hour);
                string ret = modBus.Write(Abonent, 59, Hour);
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                //                ret = modBus.WriteCheck(_rectifierPars.Abonent, 58, Min);
                ret = modBus.Write(Abonent, 58, Min);
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 57, Sec));
                return (modBus.Write(Abonent, 57, Sec));
            }
            else
            {
                //                string ret = modBus.WriteCheck(_rectifierPars.Abonent, 56, Hour);
                string ret = modBus.Write(Abonent, 56, Hour);
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                //                ret = modBus.WriteCheck(_rectifierPars.Abonent, 55, Min);
                ret = modBus.Write(Abonent, 55, Min);
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 54, Sec));
                return (modBus.Write(Abonent, 54, Sec));
            }
        }
        string SetNominal()
        {
            pr("SetNominal");
            if (rectifierPars.TpIU == EIU.ByU)
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 51, _rectifierPars.NominalU));
                return (modBus.Write(Abonent, 51, rectifierPars.NominalU));
            else
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 50, _rectifierPars.NominalI));
                return (modBus.Write(Abonent, 50, rectifierPars.NominalI));
        }
        string SetMaximum()
        {
            pr("SetMaximum");
            if (rectifierPars.TpIU == EIU.ByU)
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 53, _rectifierPars.MaxI));
                return (modBus.Write(Abonent, 53, rectifierPars.MaxI));
            else
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 52, _rectifierPars.MaxU));
                return (modBus.Write(Abonent, 52, rectifierPars.MaxU));
        }
        string OnOff(bool _On)
        {
            pr("OnOff");
            if (rectifierPars.TpIU == EIU.ByU)
            {
                //                string ret = modBus.WriteCheck(_rectifierPars.Abonent, 61, false);
                string ret = modBus.Write(Abonent, 61, false);
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 60, _On));
                return (modBus.Write(Abonent, 60, _On));
            }
            else
            {
                //                string ret = modBus.WriteCheck(_rectifierPars.Abonent, 60, false);
                string ret = modBus.Write(Abonent, 60, false);
                if (ret != null)
                    return (ret);
                Thread.Sleep(Period);
                //                return (modBus.WriteCheck(_rectifierPars.Abonent, 61, _On));
                return (modBus.Write(Abonent, 61, _On));
            }
        }
    }
}
