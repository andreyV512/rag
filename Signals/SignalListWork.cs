using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

using PARLIB;
using UPAR_common;
using Signals.Boards;


namespace Signals
{
    public class SignalListWork : SignalList
    {
        public SignalListWork(PCIE1730pars _pars0, PCIE1730pars _pars1, PCI1784Upars _pars1784, SignalsPanelPars _SignalsPanel, Board.DOnPr _OnPr, bool _PCSide = true)
            : base(_pars0, _pars1, _pars1784, _SignalsPanel, _OnPr, _PCSide) { }
        public SignalListWork(PCIE1730pars _pars0, PCIE1730pars _pars1, PCI1784Upars _pars1784, ScrollableControl _Container, SignalsPanelPars _SignalsPanel, Board.DOnPr _OnPr, bool _PCSide = true)
            : base(_pars0, _pars1, _pars1784, _Container, _SignalsPanel, _OnPr, _PCSide) { }
        protected override void SetSignals(PCIE1730pars _pars0, PCIE1730pars _pars1)
        {
            Type tp = GetType();
            foreach (FieldInfo fi in tp.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.FieldType != typeof(Signal))
                    return;
                DNameAttribute dn = Attribute.GetCustomAttribute(fi, typeof(DNameAttribute)) as DNameAttribute;
                if (dn == null)
                    throw new Exception("Сигнал не имеет описания: " + fi.Name);
                SignalInputAttribute si = Attribute.GetCustomAttribute(fi, typeof(SignalInputAttribute)) as SignalInputAttribute;
                if (si == null)
                    throw new Exception("Сигнал не имеет направления: " + fi.Name);
                int iboard = -1;
                SignalPars p = GetPars(_pars0 == null ? null : _pars0.Signals, _pars1 == null ? null : _pars1.Signals, dn.Name, ref iboard);
                if (p == null)
                    throw new Exception("Сигнал не найден и его негде создать: " + fi.Name + ": " + dn.Name);
                p.Input = si.Input;
                Signal s = new Signal(iboard, p, boards[iboard].DigitalOffset, SyncObj, WriteSignal);
                if (!PCSide)
                    s.input = !s.input;
                Add(s);
                s.hint = fi.Name;
                fi.SetValue(this, s);
            }
            ClearSignalPars(_pars0 == null ? null : _pars0.Signals, _pars1 == null ? null : _pars1.Signals);
        }
        void ClearSignalPars(L_SignalPars _L0, L_SignalPars _L1)
        {
            List<SignalPars> LBuf = new List<SignalPars>();
            if (_L0 != null)
            {
                LBuf.Clear();
                foreach (SignalPars sp in _L0)
                {
                    if (this[sp.Name] == null)
                        LBuf.Add(sp);
                }
                foreach (SignalPars sp in LBuf)
                    _L0.Remove(sp);
            }
            if (_L1 != null)
            {
                LBuf.Clear();
                foreach (SignalPars sp in _L1)
                {
                    if (this[sp.Name] == null)
                        LBuf.Add(sp);
                }
                foreach (SignalPars sp in LBuf)
                    _L1.Remove(sp);
            }
        }
        SignalPars GetPars(L_SignalPars _L0, L_SignalPars _L1, string _dn_Name, ref int _board)
        {
            SignalPars p = null;
            _board = -1;
            if (_L0 != null)
                p = _L0[_dn_Name];
            if (p != null)
            {
                _board = 0;
                return (p);
            }
            if (_L1 != null)
                p = _L1[_dn_Name];
            if (p != null)
            {
                _board = 1;
                return (p);
            }
            if (_L0 != null)
            {
                p = _L0.AddNew() as SignalPars;
                p.Name = _dn_Name;
            }
            if (p != null)
            {
                _board = 0;
                return (p);
            }
            if (_L1 != null)
            {
                p = _L1.AddNew() as SignalPars;
                p.Name = _dn_Name;
            }
            if (p != null)
            {
                _board = 0;
                return (p);
            }
            return (null);
        }
    }
}
