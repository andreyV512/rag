using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Reflection;

using UPAR_common;
using Signals.Boards;
using PARLIB;

namespace Signals
{
    public abstract class SignalList : List<Signal>
    {
        FView fView = null;
        ScrollableControl Container = null;
        ThreadStart ts;
        Thread th;
        protected object SyncObj = new object();
        protected Board[] boards = new Board[2] { null, null };
        Board1784virtual board1784 = null;
        protected volatile bool terminate = false;
        L_SignalPars signalPars0 = null;
        L_SignalPars signalPars1 = null;
        UCSignals ucSignals = null;
        SignalsPanelPars SignalsPanel;
        protected bool PCSide;
        CatchSignals catchSignals = new CatchSignals();

        SignalList(PCIE1730pars _pars0, PCIE1730pars _pars1, PCI1784Upars _pars1784, Board.DOnPr _OnPr, bool _PCSide)
        {
            PCSide = _PCSide;
            if (_pars0 != null)
            {
#if (BoardVirtual)
            boards[0] = new BoardVirtual(_pars0, _OnPr);
#endif
#if (BoardSQL)
                boards[0] = new BoardSQL(0, _pars0, "Uran", _PCSide, _OnPr);
#endif
#if (Board1730)
                boards[0] = new Board1730(_pars0, _OnPr);
#endif
            }

            if (_pars1 != null)
            {
#if (BoardVirtual)
            boards[1] = new BoardVirtual(_pars1, _OnPr);
#endif
#if (BoardSQL)
                boards[1] = new BoardSQL(1, _pars1, "Uran", _PCSide, _OnPr);
#endif
#if (Board1730)
                boards[0] = new Board1730(_pars1, _OnPr);
#endif
            }

            if (_pars1784 != null)
            {
#if (BoardVirtual)
                board1784 = new Board1784virtual(_pars1784.Devnum, _PCSide);
#endif
#if (BoardSQL)
                board1784 = new Board1784SQL("Uran", _pars1784.Devnum, _PCSide);

#endif
            }
            SetSignals(_pars0, _pars1);
#if (BoardSQL)
            if (!_PCSide)
                InitOut();
#endif
            ResetAll();
            ts = new ThreadStart(Run);
            th = new Thread(ts);
            th.Start();
        }
        protected abstract void SetSignals(PCIE1730pars _pars0, PCIE1730pars _pars1);
        public SignalList(PCIE1730pars _pars0, PCIE1730pars _pars1, PCI1784Upars _pars1784, SignalsPanelPars _SignalsPanel, Board.DOnPr _OnPr, bool _PCSide = true)
            : this(_pars0, _pars1, _pars1784, _OnPr, _PCSide)
        {
            SignalsPanel = _SignalsPanel;
            if (SignalsPanel.Visible)
                Show();
        }
        public SignalList(PCIE1730pars _pars0, PCIE1730pars _pars1, PCI1784Upars _pars1784, ScrollableControl _Container, SignalsPanelPars _SignalsPanel, Board.DOnPr _OnPr, bool _PCSide = true)
            : this(_pars0, _pars1, _pars1784, _OnPr, _PCSide)
        {
            Container = _Container;
            SignalsPanel = _SignalsPanel;
            if (SignalsPanel.Visible)
                Show();
        }
        void InitSignals()
        {
            Type tp = GetType();
            foreach (FieldInfo fi in tp.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.FieldType != typeof(Signal))
                    return;
                DNameAttribute dn = Attribute.GetCustomAttribute(fi, typeof(DNameAttribute)) as DNameAttribute;
                if (dn == null)
                    throw new Exception("Сигнал не имеет описания: " + fi.Name);
                Signal s = this[dn.Name];
                if (s == null)
                    throw new Exception("Сигнал не найден: " + fi.Name + ": " + dn.Name);
                s.hint = fi.Name;
                fi.SetValue(this, s);
            }
        }
        void InitSignals(PCIE1730pars _pars)
        {
            Type tp = GetType();
            foreach (FieldInfo fi in tp.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (fi.FieldType != typeof(Signal))
                    return;
                DNameAttribute dn = Attribute.GetCustomAttribute(fi, typeof(DNameAttribute)) as DNameAttribute;
                if (dn == null)
                    throw new Exception("Сигнал не имеет описания: " + fi.Name);
                Signal s = this[dn.Name];
                if (s == null)
                    throw new Exception("Сигнал не найден: " + fi.Name + ": " + dn.Name);
                s.hint = fi.Name;
                fi.SetValue(this, s);
            }
        }
        void InitOut()
        {
            int tick = Environment.TickCount;
            int valuesOut = boards[0].ReadOut();
            valuesOut = 0;
            foreach (Signal p in this)
            {
                if (!p.input && p.board == 0)
                    p.SetVal(Board.GetBit(valuesOut, p.PositionOffset), tick);
            }
            if (boards[1] != null)
            {

                valuesOut = boards[1].ReadOut();
                foreach (Signal p in this)
                {
                    if (!p.input && p.board == 1)
                        p.SetVal(Board.GetBit(valuesOut, p.PositionOffset), tick);
                }
            }
        }
        void Run()
        {
            while (!terminate)
            {
                int tick = Environment.TickCount;
                lock (SyncObj)
                {
                    int valuesIn = boards[0].Read();
                    foreach (Signal p in this)
                    {
                        if (p.input && p.board == 0)
                            p.SetVal(Board.GetBit(valuesIn, p.PositionOffset), tick);
                    }
                    if (boards[1] != null)
                    {
                        valuesIn = boards[1].Read();
                        foreach (Signal p in this)
                        {
                            if (p.input && p.board == 1)
                                p.SetVal(Board.GetBit(valuesIn, p.PositionOffset), tick);
                        }
                    }
                    if (board1784 != null)
                        IsLirsReset = board1784.Exec();
                    catchSignals.Exec();
                    DoRun0();
                }
                Thread.Sleep(boards[0].Timeout);
            }

        }
        protected virtual void DoRun0()
        {
        }
        public void Show()
        {
            if (Container == null)
            {
                if (fView == null)
                    fView = new FView(L_WindowLPars.CurrentWins, this, SignalsPanel);
                else if (fView.IsDisposed)
                    fView = new FView(L_WindowLPars.CurrentWins, this, SignalsPanel);
                fView.Show();
            }
            else
            {
                if (ucSignals == null)
                {
                    ucSignals = new UCSignals();
                    ucSignals.IsAlive = SignalsPanel.Alive;
                    ucSignals.Init(this, SignalsPanel.PanelWidth, SignalsPanel.PanelsDefault);
                    ucSignals.Dock = DockStyle.Fill;
                    if (Container is Form)
                        (Container as Form).Controls.Add(ucSignals);
                    else if (Container is Panel)
                        (Container as Panel).Controls.Add(ucSignals);
                }
            }
        }

        public void Dispose()
        {
            if (Container == null)
            {
                if (fView != null)
                {
                    SignalsPanel.Visible = !fView.IsDisposed;
                    if (!fView.IsDisposed)
                    {
                        fView.Close();
                        fView = null;
                    }
                }
            }
            else
            {
                if (ucSignals != null)
                {
                    ucSignals.SavePanels();
                    ucSignals.Dispose();
                    ucSignals = null;
                }

            }
            SavePanels(signalPars0);
            SavePanels(signalPars1);
            Stop();
            ResetAll();
        }


        public void WriteSignal(Signal _signal)
        {
            lock (SyncObj)
            {
                int vv = boards[_signal.board].ReadOut();
                Board.SetBit(ref vv, _signal.PositionOffset, _signal.GetVal());
                boards[_signal.board].Write(vv);
            }
        }

        public void Stop()
        {
            terminate = true;
            th.Join();
        }
        public void Reset()
        {
            lock (SyncObj)
            {
                foreach (Signal p in this)
                {
                    if (!p.input && !p.no_reset)
                        p.Val = false;
                }
            }
        }
        public void ResetAll()
        {
            lock (SyncObj)
            {
                foreach (Signal p in this)
                {
                    if (!p.input)
                        p.Val = false;
                }
            }
        }
        public Signal this[string _name]
        {
            get
            {
                foreach (Signal s in this)
                {
                    if (s.name == _name)
                        return (s);
                }
                return (null);
            }
        }
        void SavePanels(L_SignalPars _L)
        {
            if (_L == null)
                return;
            foreach (Signal s in this)
            {
                SignalPars p = _L[s.name];
                if (p == null)
                    continue;
                p.X = s.X;
                p.Y = s.Y;
            }
        }
        public int[] Lirs
        {
            get
            {
                if (board1784 == null)
                    return (null);
                lock (SyncObj)
                {
                    return (board1784.Read());
                }
            }
            set
            {
                if (board1784 == null)
                    return;
                lock (SyncObj)
                    board1784.Write(value);
            }
        }
        public void ResetLirs()
        {
            lock (SyncObj)
            {
                if (board1784 != null)
                    board1784.Reset();
            }
        }
        bool isLirsReset = false;
        public bool IsLirsReset
        {
            get
            {
                bool ret = isLirsReset;
                isLirsReset = false;
                return (ret);
            }
            private set
            {
                if (value == true)
                    isLirsReset = true;
            }
        }
        public SignalEvent CatchNext()
        {
            lock (SyncObj)
            {
                return (catchSignals.Next());
            }
        }
        public void CatchAdd(Signal _signal)
        {
            lock (SyncObj)
            {
                catchSignals.AddSignal(_signal);
            }
        }
        public void CatchClear()
        {
            lock (SyncObj)
            {
                catchSignals.Clear();
            }
        }
        public void CatchStart()
        {
            lock (SyncObj)
            {
                catchSignals.Start();
            }
        }
        public void CatchStop()
        {
            lock (SyncObj)
            {
                catchSignals.Stop();
            }
        }
    }
}
