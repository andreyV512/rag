using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PARLIB;
using UPAR_common;
using Signals.Boards;

namespace Signals
{
    public class SignalListMan : SignalList
    {
        public SignalListMan(PCIE1730pars _pars0, PCIE1730pars _pars1, PCI1784Upars _pars1784, SignalsPanelPars _SignalsPanel, Board.DOnPr _OnPr, bool _PCSide = true)
            : base(_pars0, _pars1, _pars1784, _SignalsPanel, _OnPr, _PCSide) { }
        public SignalListMan(PCIE1730pars _pars0, PCIE1730pars _pars1, PCI1784Upars _pars1784, ScrollableControl _Container, SignalsPanelPars _SignalsPanel, Board.DOnPr _OnPr, bool _PCSide = true)
            : base(_pars0, _pars1, _pars1784, _Container, _SignalsPanel, _OnPr, _PCSide) { }
        protected override void SetSignals(PCIE1730pars _pars0, PCIE1730pars _pars1)
        {
            for (int b = 0; b < boards.Length; b++)
            {
                if (boards[b] == null)
                    continue;
                for (int pos = 0; pos < boards[b].portCount_in * 8; pos++)
                {
                    SignalPars par = null;
                    par = GetPars(_pars0.Signals, pos, boards[b].DigitalOffset, true);
                    if (par == null)
                        par = SignalParsDef(b, true, pos, boards[b].DigitalOffset);
                    Signal s = new Signal(b, par, boards[b].DigitalOffset, SyncObj, WriteSignal);
                    if (!PCSide)
                        s.input = !s.input;
                    Add(s);
                }
                for (int pos = 0; pos < boards[b].portCount_out * 8; pos++)
                {
                    SignalPars par = null;
                    par = GetPars(_pars0.Signals, pos, boards[b].DigitalOffset, false);
                    if (par == null)
                        par = SignalParsDef(b, false, pos, boards[b].DigitalOffset);
                    Signal s = new Signal(b, par, boards[b].DigitalOffset, SyncObj, WriteSignal);
                    if (!PCSide)
                        s.input = !s.input;
                    Add(s);
                }
            }
        }
        SignalPars SignalParsDef(int _iboard, bool _input, int _pos, int _DigitalOffset)
        {
            SignalPars par = new SignalPars();
            par.Digital = _pos >= _DigitalOffset;
            par.Position = par.Digital ? _pos - _DigitalOffset : _pos;
            //par.Name = string.Format("{0}{2}:{0}",
            //    _iboard.ToString(),
            //    par.Digital ? "Д" : "",
            //    par.Position.ToString()
            //    );
            par.Input = _input;
            return (par);
        }
        SignalPars GetPars(L_SignalPars _L, int _pos, int _DigitalOffset, bool _input)
        {
            foreach (SignalPars par in _L)
            {
                if (par.Input != _input)
                    continue;
                if (_pos < _DigitalOffset)
                {
                    if (par.Digital)
                        continue;
                    if (par.Position != _pos)
                        continue;
                }
                else
                {
                    if (!par.Digital)
                        continue;
                    if (par.Position != _pos + _DigitalOffset)
                        continue;
                }
                return (par);
            }
            return (null);
        }
    }
}
