using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Signals
{
    public partial class UCSignals : UserControl
    {
        public UCSignals()
        {
            InitializeComponent();
        }
        SignalList SL;
        [Category("RAG"), Description("Можно ли нажимать кнопки")]
        public bool IsAlive { set; get; }
        public void Init(SignalList _SL, int _PanelWidth, bool _PanelsDefault)
        {
            Clear();
            SL = _SL;
            foreach (Signal p in _SL)
            {
                if (p.input)
                {
                    UCSignalIn pi = new UCSignalIn(p);
                    pi.alive = false;
                    pi.Width = _PanelWidth;
                    Controls.Add(pi);
                }
                else
                {
                    UCSignalOut po = new UCSignalOut(p);
                    po.alive = IsAlive;
                    po.Width = _PanelWidth;
                    Controls.Add(po);
                }
            }
            timer1.Start();
            if (_PanelsDefault)
                SetPanelsDefault();
            else
                SetPanels();
        }
        void SetPanels()
        {
            foreach (UCSignalIn p in Controls.OfType<UCSignalIn>())
                p.PanelLoad();
            foreach (UCSignalOut p in Controls.OfType<UCSignalOut>())
                p.PanelLoad();
        }
        void SetPanelsDefault()
        {
            int ltop = 0;
            int space = 2;
            int rleft = 0;
            foreach (UCSignalIn p in Controls.OfType<UCSignalIn>())
            {
                p.Left = space;
                rleft = p.Left + p.Width + space;
                p.Top = ltop + space;
                ltop += p.Height + space;
            }
            ltop = 0;
            foreach (UCSignalOut p in Controls.OfType<UCSignalOut>())
            {
                p.Left = rleft;
                p.Top = ltop + space;
                ltop += p.Height + space;
            }
        }
        public void SavePanels()
        {
            foreach (UCSignalIn p in Controls.OfType<UCSignalIn>())
                p.PanelSave();
            foreach (UCSignalOut p in Controls.OfType<UCSignalOut>())
                p.PanelSave();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (Control p in Controls)
            {
                if (p is UCSignalIn)
                    ((UCSignalIn)p).Exec();
                if (p is UCSignalOut)
                    ((UCSignalOut)p).Exec();
            }
        }
        public void Clear()
        {
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                Control p = Controls[i];
                if (p is UCSignalIn || p is UCSignalOut)
                    Controls.Remove(p);
            }
        }
    }
}
