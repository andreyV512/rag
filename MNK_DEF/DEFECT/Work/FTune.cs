using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PARLIB;
using UPAR;
using Defect.LCard;
using Protocol;
using InverterNS;

namespace Defect.Work
{
    public partial class FTune : Form 
    {
        SignalListDef SL;
        object Sync = new object();

        public FTune(SignalListDef _SL)
        {
            InitializeComponent();
            SL = _SL;
        }
        private void FTune_Load(object sender, EventArgs e)
        {
            L_WindowLPars.CurrentWins.LoadFormRect(this);
            uRectifierC.Init(ParAll.ST.Defect.Cross.Rectifiers, ParAll.CTS.Cross.Rectifier);
            uRectifierL.Init(ParAll.ST.Defect.Line.Rectifiers, ParAll.CTS.Line.Rectifier);
            uDemagnetizer1.Init(ParAll.ST.Defect.Demagnetizer, ParAll.CTS.DemagnetizerTS);
            uGSPF1.Init(SL,uDemagnetizer1.jDemagnetizer);
            uacs1.Init();
            uInverter1.Init(ParAll.ST.Defect.Line.ComPortConverters,
                   ParAll.ST.Defect.Line.Converter,
                   ParAll.ST.TSSet.Current.Line.Frequency);
        }

        private void FTune_FormClosed(object sender, FormClosedEventArgs e)
        {
            uRectifierC.Dispose();
            uRectifierL.Dispose();
            uGSPF1.Dispose();
            uacs1.Dispose();
            uInverter1.Dispose();
            uDemagnetizer1.Dispose();
            L_WindowLPars.CurrentWins.SaveFormRect(this);
        }
        void pr(string _msg)
        {
            ProtocolST.pr(_msg);
        }

        private void FTune_Resize(object sender, EventArgs e)
        {
            uGSPF1.Width=ClientSize.Width-uGSPF1.Left*2;
            uGSPF1.Height = ClientSize.Height - uGSPF1.Top;
        }

    }
}
