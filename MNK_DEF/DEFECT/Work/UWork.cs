using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Share;
using UPAR;
using Signals;
using Protocol;
using Defect.SG;

namespace Defect.Work
{
    public partial class UWork : UserControl
    {
        public delegate void DOnDraw();

        SignalListDef SL = null;
        IJob jCurrent = null;
        DOnExec OnExec = null;
        List<string> LErrors = new List<string>();
        bool blockCheckBox = false;
        UCrossLine uCross = null;
        UCrossLine uLine = null;
        public UWork()
        {
            InitializeComponent();
        }
        public void Init(DOnExec _OnExec, UCrossLine _uCross, UCrossLine _uLine)
        {
            OnExec = _OnExec;
            uCross = _uCross;
            uLine = _uLine;
            timer1.Interval = (int)ParAll.ST.Defect.Some.Period;
            SL = new SignalListDef(ParAll.ST.Defect.PCIE1730, ParAll.ST.Some.SignalsPanel, pr);
            checkBox1.Checked = ParAll.ST.Defect.Some.IsInterruptView;
        }
        public new void Dispose()
        {
            if (jCurrent != null)
            {
                jCurrent.Finish();
                for (; ; )
                {
                    if (jCurrent.IsComplete)
                        break;
                    Thread.Sleep(200);
                }
            }
            if (SL != null)
                SL.Dispose();
            base.Dispose();
        }
        void DisableBy(object _o)
        {
            if (bTune != _o)
                bTune.Enabled = false;
            if (cbWork != _o)
                cbWork.Enabled = false;
            if (cbTest != _o)
                cbTest.Enabled = false;
            bSG.Enabled = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (FTune f = new FTune(SL)) { f.ShowDialog(); }
        }
        void pr(string _msg)
        {
            ProtocolST.pr("Uwork: " + _msg);
        }
        public void prs(string _msg) { prsl(0, _msg); }
        public void prsl(uint _level, string _msg)
        {
            if (_level == 0)
                toolStripStatusLabel1.Text = _msg;
            else
            {
                toolStripStatusLabel2.Text = _msg;
                if (_msg != null)
                    LErrors.Add(_msg);
            }
            ProtocolST.pr("Uwork: " + _msg);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SL.Show();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            ParAll.ST.Defect.Some.IsInterruptView = checkBox1.Checked;
        }
        //========================================================================================================================================================
        private void cbWork_Click(object sender, EventArgs e)
        {
            if (blockCheckBox)
                return;
            if (cbWork.Checked)
            {
                cbWork.Text = "Стоп";
                ClearErrors();
                jCurrent = new JWork(new cIW(true), SL, false, OnExecL, prsl);
                DisableBy(cbWork);
                timer1.Enabled = true;
            }
            else
            {
                cbWork.Text = "Выкл...";
                cbWork.Enabled = false;
                jCurrent.Finish();
            }

        }
        private void cbTest_Click(object sender, EventArgs e)
        {
            if (blockCheckBox)
                return;
            if (cbTest.Checked)
            {
                cbTest.Text = "Стоп";
                ClearErrors();
                jCurrent = new JTest(SL, OnExecL, prsl);
                DisableBy(cbTest);
                timer1.Enabled = true;
            }
            else
            {
                cbTest.Text = "Выкл...";
                cbTest.Enabled = false;
                jCurrent.Finish();
            }
        }
        void EnableAll()
        {
            blockCheckBox = true;
            bTune.Enabled = true;
            cbWork.Enabled = true;
            cbTest.Enabled = true;
            bReStart.Enabled = false;
            cbTest.Text = "Тест";
            cbTest.Checked = false;
            cbWork.Text = "Пуск";
            cbWork.Checked = false;
            uCross.StateH = null;
            uLine.StateH = null;
            blockCheckBox = false;
            bSG.Enabled = true;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            jCurrent.Exec(Environment.TickCount);
            if (jCurrent.IsError)
            {
                prsl(1, jCurrent.LastError);
                EnableAll();
                if (jCurrent is JTest)
                    (jCurrent as JTest).Dispose();
                if (jCurrent is JWork)
                    (jCurrent as JWork).Dispose();
                jCurrent = null;
                return;
            }
            if (jCurrent.IsComplete)
            {
                if (jCurrent is JWork)
                {
                    if (cbWork.Text == "Выкл...")
                    {
                        EnableAll();
                        (jCurrent as JWork).Dispose();
                        jCurrent = null;
                        return;
                    }
                    else
                    {
                        ClearErrors();
                        (jCurrent as JWork).Dispose();
                        jCurrent = new JWork(new cIW(true), SL, true, OnExecL, prsl);
                        timer1.Enabled = true;
                        return;
                    }
                }
                if (jCurrent is JTest)
                {
                    EnableAll();
                    (jCurrent as JTest).Dispose();
                    jCurrent = null;
                    return;
                }
            }
            if (jCurrent is JTest)
            {
                uCross.StateH = (jCurrent as JTest).StateHCross;
                uLine.StateH = (jCurrent as JTest).StateHLine;
            }
            if (jCurrent is JWork)
            {
                uCross.StateH = (jCurrent as JWork).StateHCross;
                uLine.StateH = (jCurrent as JWork).StateHLine;
            }
            timer1.Enabled = true;
        }

        private void bReStart_Click(object sender, EventArgs e)
        {
            if (jCurrent == null || !(jCurrent is JWork))
                return;
            bReStart.Enabled = false;
            (jCurrent as JWork).ReStart();
        }
        void OnExecL(string _msg)
        {
            if (_msg == "RESTART")
            {
                pr("get RESTART");
                if (!checkBox1.Checked)
                {
                    if (jCurrent == null || !(jCurrent is JWork))
                        return;
                    (jCurrent as JWork).ReStart();
                    pr("(jCurrent as JWork).ReStart()");
                    return;
                }
                else
                {
                    pr("bReStart.Enabled");
                    bReStart.Enabled = true;
                    _msg = "VIEW";
                }
            }
            if (OnExec != null)
                OnExec(_msg);
        }
        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            if (LErrors.Count == 0)
                return;
            using (FErrors f = new FErrors())
            {
                f.Add(LErrors);
                f.ShowDialog();
            }
        }
        void ClearErrors()
        {
            LErrors.Clear();
            toolStripStatusLabel2.Text = null;
        }

        private void bSG_Click(object sender, EventArgs e)
        {
            using (FMainSG f = new FMainSG())
            {
                f.Init(SL);
                f.ShowDialog();
            }
        }

    }
}
