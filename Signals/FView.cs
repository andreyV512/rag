using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PARLIB;
using UPAR_common;

namespace Signals
{
    public partial class FView : Form
    {
        public static string SaveName = "Сигналы COM";
        L_WindowLPars Wins;
        SignalList SL;
        SignalsPanelPars SignalsPanel;
        public FView(L_WindowLPars _Wins, SignalList _SL, SignalsPanelPars _SignalsPanel)
        {
            InitializeComponent();
            Wins = _Wins;
            SL = _SL;
            SignalsPanel = _SignalsPanel;
        }

        private void FView_Load(object sender, EventArgs e)
        {
            Wins.LoadFormRect(this);
            ucSignals1.IsAlive = SignalsPanel.Alive;
            ucSignals1.Init(SL, SignalsPanel.PanelWidth, SignalsPanel.PanelsDefault);
        }
        private void FView_FormClosed(object sender, FormClosedEventArgs e)
        {
            ucSignals1.SavePanels();
            Wins.SaveFormRect(this);
        }
    }
}
