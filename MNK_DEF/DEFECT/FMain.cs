using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Share;
using PARLIB;
using UPAR;
using Protocol;
using ResultLib;
using About;

namespace Defect
{
    public partial class FMain : Form
    {
        FPrevTube fPrevTube = null;
        public string SaveName = "Cross главная";
        int space = 4;

        public FMain()
        {
            InitializeComponent();
            Current.UnitType = EUnit.Cross;
        }

        bool ExitZero = false;
        private void FMain_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            L_WindowLPars.CurrentWins = ParAll.ST.Wins;
            if (IsParams())
            {
                using (FParAll f = new FParAll())
                {
                    if (f.Login())
                        f.ShowDialog();
                }
                ParAll.ST.Save();
                ExitZero = true;
                Environment.Exit(0);
                return;
            }

            L_WindowLPars.CurrentWins.LoadFormRect(this);
            ProtocolPar pp = ParAll.ST.Protocol;
            Form protocol = ProtocolST.Instance(this, pp.Period, pp.IsFile, pp.IsSave);
            if (protocol != null)
            {
                L_WindowLPars.CurrentWins.LoadFormRect(protocol);
                if (pp.IsVisible)
                    protocol.Show();
            }
            uSplitter1.Init(uThick1, uCross1);
            uSplitter2.Init(uCross1, uLine1);
            uSplitter3.Init(uLine1, uSum1);
            RResize0();
            RResize();
            uSplitter1.RLoad(ParAll.ST.Some.Splitter1);
            uSplitter2.RLoad(ParAll.ST.Some.Splitter2);
            uSplitter3.RLoad(ParAll.ST.Some.Splitter3);
            uWork1.Init(OnExec, uCross1, uLine1);
            uThick1.Init();
            uCross1.Init(EUnit.Cross);
            uLine1.Init(EUnit.Line);
            uSum1.Init();
            uManage1.Init();
            uLine1.OnRecalc = ReCalc;
            if (IsTest())
                bTest.Visible = true;
            uManage1.DrawStatist();
        }
        private void FMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ExitZero)
                return;
            Form protocol = ProtocolST.Instance(this);
            if (protocol != null)
            {
                ProtocolPar pp = ParAll.ST.Protocol;
                pp.IsFile = ProtocolST.IsFile;
                pp.IsSave = ProtocolST.IsSave;
                pp.IsVisible = protocol.Visible;
                L_WindowLPars.CurrentWins.SaveFormRect(protocol);
                protocol.Close();
            }
            uManage1.Dispose();
            uLine1.Save();
            uWork1.Dispose();
            L_WindowLPars.CurrentWins.SaveFormRect(this);
            ParAll.ST.Some.Splitter1 = uSplitter1.Top;
            ParAll.ST.Some.Splitter2 = uSplitter2.Top;
            ParAll.ST.Some.Splitter3 = uSplitter3.Top;
            if (fPrevTube != null)
                fPrevTube.Dispose();
        }
        void RResize0()
        {
            uWork1.Left = space;
            uManage1.Left = space;
            uThick1.Left = space;
            uSplitter1.Left = space;
            uCross1.Left = space;
            uSplitter2.Left = space;
            uLine1.Left = space;
            uSplitter3.Left = space;
            uSum1.Left = space;

            uSplitter1.Height = space;
            uSplitter2.Height = space;
            uSplitter3.Height = space;
        }
        void RResize()
        {
            int w = ClientSize.Width - 2 * space;
            uWork1.Width = w;
            uManage1.Width = w;
            uThick1.Width = w;
            uCross1.Width = w;
            uLine1.Width = w;
            uSum1.Width = w;
            uSplitter1.Width = w;
            uSplitter2.Width = w;
            uSplitter3.Width = w;

            //            double h = ClientSize.Height - uCross1.Top - space * 2;
            //            double ksize = h / (uCross1.KSize + uLine1.KSize + uSum1.KSize);


            //            double h = ClientSize.Height - uLine1.Top - space * 2;
            //            double ksize = h / (uLine1.KSize + uSum1.KSize);
            //uLine1.Height = (int)(uLine1.KSize * ksize);
            //uSplitter2.Top = uLine1.Top + uLine1.Height;
            //            uSum1.Top = uSplitter2.Top + uSplitter2.Height;
            uSum1.Height = ClientSize.Height - uSum1.Top - space;
        }

        private void FMain_Resize(object sender, EventArgs e)
        {
            RResize();
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FParAll f = new FParAll())
            {
                f.SaveVisible = ParAll.ST.Some.IsSaveInter;
                f.ShowDialog();
            }
            uThick1.LoadSettings();
            uCross1.LoadSettings();
            uLine1.LoadSettings();
            uManage1.LoadSettings();
            Draw();
            ParAll.ST.Save();
        }

        private void протоколToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProtocolST.Show();
        }

        //void ROnLoad(string _fname, bool _isDef, int _version = 0)
        //{
        //    try
        //    {
        //        RK.ST.result = new Result(_fname, EUnit.Line, _version);
        //        //                RK.ST.result.Thick.MZone.Clear();
        //        //                RK.ST.result.Sum.MClass.Clear();
        //        Draw();
        //        uLine1.ViewMode = true;
        //    }
        //    catch (ExceptionLoad e)
        //    {
        //        Clear();
        //        uWork1.prsl(1, e.Message);
        //        RK.ST.result = null;
        //    }
        //}


        void Clear()
        {
            uCross1.Clear();
            uLine1.Clear();
            uThick1.Clear();
            uSum1.Clear();
        }
        bool ViewMode
        {
            set
            {
                uCross1.ViewMode = value;
                uLine1.ViewMode = value;
            }
        }

        private void Draw()
        {
            uCross1.RHide();
            uLine1.RHide();
            DrawWork();
        }
        private void DrawWork()
        {
            if (RK.ST.result == null)
            {
                Clear();
                return;
            }
            uThick1.RDraw();
            uCross1.Draw();
            uLine1.Draw();
            uSum1.Draw();
            uManage1.sgState = RK.ST.result.SG.sgState;
        }

        private void файлToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (RK.ST.result == null)
            {
                пересчитатьToolStripMenuItem.Enabled = false;
                сохранитьToolStripMenuItem.Enabled = false;
                return;
            }
            пересчитатьToolStripMenuItem.Enabled = true;
            сохранитьToolStripMenuItem.Enabled = true;
            поперечныйToolStripMenuItem.Enabled = RK.ST.result.Cross.MZone.Count != 0;
            продольныйToolStripMenuItem.Enabled = RK.ST.result.Line.MZone.Count != 0;
        }

        void pr(string _msg)
        {
            ProtocolST.pr(_msg);
        }

        //void prs(uint _level, string _msg)
        //{
        //    uManage1.OnStatus(_level, _msg);
        //}

        void OnExec(string _msg)
        {
            if (_msg == "CLEAR")
                Clear();
            else if (_msg == "VIEW")
            {
                uCross1.ViewMode = true;
                uLine1.ViewMode = true;
                uSum1.DrawGoodArea();
            }
            else if (_msg == "DRAW")
                DrawWork();
            else if (_msg == "STATIST")
                uManage1.AddStatist();
        }

        void ReCalc()
        {
            if (RK.ST.result == null)
                return;
            RK.ST.result.Compute();
            Draw();
            ViewMode = true;
        }

        private void пересчитатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReCalc();
            uSum1.DrawGoodArea();
        }

        public bool Login()
        {
            if (!ParAll.ST.IsLogin)
                return (true);
            using (FLogin f = new FLogin(ParAll.ST))
            {
                f.ShowDialog();
                return (f.Ok);
            }
        }

        public static ESource IsLocal()
        {
            for (int i = 1; i < Environment.GetCommandLineArgs().Length; i++)
            {
                if (Environment.GetCommandLineArgs()[i] == "local")
                    return (ESource.File);
            }
            return (ESource.SQL);
        }

        public static bool IsParams()
        {
            for (int i = 1; i < Environment.GetCommandLineArgs().Length; i++)
            {
                if (Environment.GetCommandLineArgs()[i] == "params")
                    return (true);
            }
            return (false);
        }

        public static bool IsTest()
        {
            for (int i = 1; i < Environment.GetCommandLineArgs().Length; i++)
            {
                if (Environment.GetCommandLineArgs()[i] == "test")
                    return (true);
            }
            return (false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool aaa = BitConverter.IsLittleEndian;
            int a1 = sizeof(int);
            int v = 1;
            byte[] mv = BitConverter.GetBytes(v);
            byte[] mv1 = new byte[] { 0, 0, 0, 1 };
            int v1 = BitConverter.ToInt32(mv1, 0);
            //byte[] r = ACS.ACS.GetRequest(null, 1);
            //TubeParams tubePars = new TubeParams();
            //tubePars.Local = false;
            //RDPars1 rt = new RDPars1(ParAll.CTS);
            //rt.SaveToDB(tubePars);
        }

        private void поперечныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RK.ST.result == null)
                return;
            string fname = "";
            if (Result.SaveDialog("Сохранение файла измерений Поперечного", "файлы (*.bindkb2)|*.bindkb2|Все файлы (*.*)|*.*", ref fname) != DialogResult.OK)
                return;
            RK.ST.result.Cross.SaveBINDKB2(fname);
        }

        private void продольныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RK.ST.result == null)
                return;
            string fname = "";
            if (Result.SaveDialog("Сохранение файла измерений Продолного", "файлы (*.bindkb2)|*.bindkb2|Все файлы (*.*)|*.*", ref fname) != DialogResult.OK)
                return;
            RK.ST.result.Line.SaveBINDKB2(fname);
        }

        private void поперечныйToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            файлToolStripMenuItem.HideDropDown();
            string fname = "";
            if (Result.OpenDialog("Загрузка файла измерений Поперечного", "файлы (*.bindkb2)|*.bindkb2|Все файлы (*.*)|*.*", ref fname) != DialogResult.OK)
                return;
            try
            {
                if (RK.ST.result == null)
                    RK.ST.result = new Result();
                RK.ST.result.Cross.LoadBINDKB2(fname);
                RK.ST.result.Compute();
                Draw();
                uCross1.ViewMode = true;
            }
            catch (ExceptionLoad ex)
            {
                Clear();
                uWork1.prsl(1, ex.Message);
                RK.ST.result.Cross.MZone.Clear();
            }
        }

        private void продольныйToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            файлToolStripMenuItem.HideDropDown();
            string fname = "";
            if (Result.OpenDialog("Загрузка файла измерений Продольного", "файлы (*.bindkb2)|*.bindkb2|Все файлы (*.*)|*.*", ref fname) != DialogResult.OK)
                return;
            try
            {
                if (RK.ST.result == null)
                    RK.ST.result = new Result();
                RK.ST.result.Line.LoadBINDKB2(fname);
                RK.ST.result.Compute();
                Draw();
                uLine1.ViewMode = true;
            }
            catch (ExceptionLoad ex)
            {
                Clear();
                uWork1.prsl(1, ex.Message);
                RK.ST.result.Line.MZone.Clear();
            }
        }


        private void предыдущаяТрубаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fPrevTube == null || fPrevTube.IsDisposed)
                fPrevTube = new FPrevTube();
            fPrevTube.Show();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Version ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(ver.Build).AddSeconds(ver.Revision * 2);
            using (FAbout f = new FAbout())
            {
                f.Version = buildDate;
                f.ShowDialog();
            }
        }
    }
}
