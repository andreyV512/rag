using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using PARLIB;
using Protocol;
using SQL;
using UPAR;
using UPAR.SG;
using Defect.Work;

namespace Defect.SG
{
    public partial class FMainSG : Form
    {
        string Schema = "Uran";
        TypeSize.DBKey TSKey = null;

        public FMainSG()
        {
            InitializeComponent();
        }
        private void FMain_Load(object sender, EventArgs e)
        {
//            new Execute("Uran.SGtubesLock").Exec();
            L_WindowLPars.CurrentWins.LoadFormRect(this);
            BaseItem.Schema = Schema;

            splitContainer1.SplitterDistance = ParAll.SG.Some.FMain_SplitterDistance;
            foreach (DGV p in splitContainer1.Panel1.Controls.OfType<DGV>())
            {
                p.LoadRectangle();
                p.CC = splitContainer1.Panel1.Controls;
                p.OnPrs = prs;
            }

            ucGraph1.Schema = Schema;

            //TODO:
//            dgvTypeSize.AddButton("График", OnGraphCur);

            dgvGroup.OnCurrent += dgvEtalon.RLoad;
            dgvGroup.AddButton("Перерасчет", GroupRecalc);

            dgvEtalon.OnCurrent += dgvEtalonPars.RLoad;
            dgvEtalon.AddButton("Действия", OnExec);

            dgvTube.OnCurrent += dgvTubePars.RLoad;
            dgvTube.AddButton("Действия", OnExec);

            TypeSize.Adjust();
            TSKey = new TypeSize.DBKey(ParAll.CTS.Name);
            dgvTresh.RLoad(TSKey);
            dgvGroup.RLoad(TSKey);
            dgvTube.RLoad(TSKey);
        }
        public void Init(SignalListDef _SL)
        {
            sgWork1.Init(_SL, OnInsert);
        }
        private void FMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (DGV p in splitContainer1.Panel1.Controls.OfType<DGV>())
                p.SaveRectangle();
            ParAll.SG.Some.FMain_SplitterDistance = splitContainer1.SplitterDistance;
            L_WindowLPars.CurrentWins.SaveFormRect(this);
        }
        void prs(string _msg)
        {
            toolStripStatusLabel1.Text = _msg;
        }
        void pr(string _msg)
        {
            ProtocolST.pr("Main: " + _msg);
        }

        private void протоколToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ProtocolST.Show();
        }

        void OnGraphCur(object _current, Point _point)
        {
            TypeSize ts = _current as TypeSize;
            if (ts == null)
                return;
            Int64 TubeId = -1;
            if (dgvTube.Current != null)
                TubeId = (dgvTube.Current as Tube).Id;
            string GroupName = null;
            if (dgvGroup.Current != null)
                GroupName = (dgvGroup.Current as Group).Name;
            int EtalonId = -1;
            if (dgvEtalon.Current != null)
                EtalonId = (dgvEtalon.Current as Etalon).Id;
            ucGraph1.Exec(ts.Name, TubeId, GroupName, EtalonId);
        }
        //void OnGraph()
        //{
        //    string TSName = null;
        //    if ((uuTypeSize1.UC as UTypeSize).bs.Current != null)
        //        TSName = ((uuTypeSize1.UC as UTypeSize).bs.Current as TypeSize).Name;
        //    Int64 TubeId = -1;
        //    if ((uuTube1.UC as UTube).bs.Current != null)
        //        TubeId = ((uuTube1.UC as UTube).bs.Current as Tube).Id;
        //    string GroupName = null;
        //    if ((uuGroup1.UC as UGroup).bs.Current != null)
        //        GroupName = ((uuGroup1.UC as UGroup).bs.Current as Group).Name;
        //    int EtalonId = -1;
        //    if ((uuEtalon1.UC as UEtalon).bs.Current != null)
        //        EtalonId = ((uuEtalon1.UC as UEtalon).bs.Current as Etalon).Id;
        //    ucGraph1.Exec(TSName, TubeId, GroupName, EtalonId);
        //}
        class tagSOP
        {
            public GraphObject current;
            public SOPPars sop;
            public tagSOP(GraphObject _current, SOPPars _sop)
            {
                current = _current;
                sop = _sop;
            }
        }
        void OnExec(object _Current, Point _point)
        {
            if (_Current == null)
            {
                prs("Труба или эталон не выбраны");
                return;
            }
            foreach (ToolStripMenuItem tm in CMS_Tube.Items)
            {
                if (tm.Text == "Рассчитать ГП")
                    tm.Visible = _Current is Tube;
                else if (tm.Text == "В файл")
                    tm.Tag = _Current;
                else if (tm.Text == "Из файла")
                    tm.Tag = _Current;
                else if (tm.Text == "Рассчитать параметры" || tm.Text == "График")
                {
                    EventHandler deal = null;
                    if (tm.Text == "Рассчитать параметры")
                        deal = new EventHandler(Calc);
                    else if (tm.Text == "График")
                        deal = new EventHandler(Graph);
                    if (deal != null)
                    {
                        tm.DropDownItems.Clear();
                        tm.Click -= deal;
                        if (ParAll.SG.sgPars.SOPs.Count == 0)
                        {
                            tm.Click += deal;
                            tm.Tag = new tagSOP(_Current as GraphObject, null);
                        }
                        else
                        {
                            ToolStripItem tsi = tm.DropDownItems.Add("Мертвые зоны");
                            tsi.Click += deal;
                            tsi.Tag = _Current;
                            tsi.Tag = new tagSOP(_Current as GraphObject, null);
                            foreach (SOPPars s in ParAll.SG.sgPars.SOPs)
                            {
                                tsi = tm.DropDownItems.Add(s.Name);
                                tsi.Click += deal;
                                tsi.Tag = new tagSOP(_Current as GraphObject, s);
                            }
                        }
                    }
                }
            }
            CMS_Tube.Show(_point);
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Данные ГП";
            openFileDialog1.Filter = "Данные ГП (*.sg*)|*.sg|Все файлы (*.*)|*.*";
            openFileDialog1.FileName = "";
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            DateTime dt = DateTime.Now;
            ToolStripItem tsi = sender as ToolStripItem;
            GraphObject go = tsi.Tag as GraphObject;
            if (!go.LoadObjectFile(openFileDialog1.FileName))
            {
                prs("Данные не загружены");
                return;
            }
            DateTime dt1 = DateTime.Now;
            TimeSpan ts = dt1 - dt;
            prs("Данные загружены " + ts.Milliseconds.ToString());
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Данные ГП";
            saveFileDialog1.Filter = "Данные ГП (*.sg*)|*.sg|Все файлы (*.*)|*.*";
            saveFileDialog1.FileName = "";
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            DateTime dt = DateTime.Now;
            ToolStripItem tsi = sender as ToolStripItem;
            GraphObject go = tsi.Tag as GraphObject;
            if (!go.SaveObjectFile(saveFileDialog1.FileName))
            {
                prs("Данные не выгружены");
                return;
            }
            DateTime dt1 = DateTime.Now;
            TimeSpan ts = dt1 - dt;
            prs("Данные выгружены " + ts.Milliseconds.ToString());
        }

        void Graph(object sender, EventArgs e)
        {
            ToolStripItem tsi = sender as ToolStripItem;
            tagSOP tsop = tsi.Tag as tagSOP;
            using (
            FTubeGraph f = new FTubeGraph(tsop.current, tsop.sop))
            {
                if (f.Ok == "Ok")
                    f.ShowDialog();
                else
                    prs(f.Ok);
                if (f.IsChandge)
                    dgvTresh.RLoad(TSKey);
            }
        }
        void Calc(object sender, EventArgs e)
        {
            ToolStripItem tsi = sender as ToolStripItem;
            tagSOP tsop = tsi.Tag as tagSOP;
            Calc0(tsop.sop, tsop.current);
        }
        void Calc0(SOPPars _sop, GraphObject _go)
        {
            if (!_go.CalcPars(_sop))
                prs("Не смогли рассчитать");
            if (_go is Tube)
            {
                dgvTube.bs.ResetCurrentItem();
                dgvTubePars.RLoad((_go as Tube).Key);
            }
            if (_go is Etalon)
            {
                dgvEtalon.bs.ResetCurrentItem();
                dgvEtalonPars.RLoad((_go as Etalon).Key);
            }
        }
        void GroupRecalc(object _current, Point _point)
        {
            foreach (Etalon et in dgvEtalon.bs)
            {
                pr(et.Id.ToString());
                Calc0(ParAll.SG.sgPars.SOPs[et.SOP], et);
            }
        }
        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            Tube tube = dgvTube.Current as Tube;
            Execute E = new Execute(string.Format("{0}.GetSG", Schema));
            E.Input("@id", tube.Id);
            E.OutputString("@group", 50);
            E.OutputDouble("@probability");
            E.OutputInt("@color");
            if (E.Exec() != 1)
            {
                prs("Не удалось рассчитать трубу (проверьте параметры трубы и эталонов)");
                return;
            }
            tube.SG = E.AsString("@group");
            tube.Metric = E.AsDouble("@probability");
            dgvTube.bs.ResetCurrentItem();
            prs("Ок");
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DGV u in splitContainer1.Panel1.Controls.OfType<DGV>())
                u.Edit = (sender as CheckBox).Checked;
        }

        private void splitContainer1_Panel1_Resize(object sender, EventArgs e)
        {
            SplitterPanel sp = sender as SplitterPanel;
            checkBox1.Left = sp.ClientSize.Width - checkBox1.Width;
            checkBox1.Top = sp.ClientSize.Height - checkBox1.Height;
        }


        private void окнаПоУмолчаниюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DGV u in splitContainer1.Panel1.Controls.OfType<DGV>())
                u.ResizeDefault();
        }

        private void FMain_Resize(object sender, EventArgs e)
        {
            splitContainer1.Height = statusStrip1.Top - splitContainer1.Top;
            splitContainer1.Width = ClientSize.Width;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string aaa = "17.17;36;43.17;51.17;68";
            string[] mtresh = aaa.Split(';');
            double[] treshP = new double[mtresh.Length];
            for (int i = 0; i < mtresh.Length; i++)
                treshP[i] = Convert.ToDouble(mtresh[i]);

        }
        void LoadGrid(DataGridView _dg)
        {
            L_GridPars lGridPars = ParAll.SG.Some.Grids;
            GridPars gridPars = lGridPars[_dg.Parent.Name];
            if (gridPars == null)
                return;
            L_ColumnPars lColumnPars = gridPars.Columns;
            foreach (DataGridViewColumn col in _dg.Columns)
            {
                ColumnPars columnPars = lColumnPars[col.Name];
                if (columnPars == null)
                    continue;
                col.Width = columnPars.Width;
            }
        }
        void SaveGrid(DataGridView _dg)
        {
            L_GridPars lGridPars = ParAll.SG.Some.Grids;
            GridPars gridPars = lGridPars[_dg.Parent.Name];
            if (gridPars == null)
            {
                gridPars = lGridPars.AddNewTree(ParAll.ST.MP) as GridPars;
                gridPars.Name = _dg.Parent.Name;
            }
            L_ColumnPars lColumnPars = gridPars.Columns;
            foreach (DataGridViewColumn col in _dg.Columns)
            {
                ColumnPars columnPars = lColumnPars[col.Name];
                if (columnPars == null)
                {
                    columnPars = lColumnPars.AddNewTree(ParAll.ST.MP) as ColumnPars;
                    columnPars.Name = col.Name;
                }
                columnPars.Width = col.Width;
            }
        }

        public static bool IsParam(string _par)
        {
            for (int i = 1; i < Environment.GetCommandLineArgs().Length; i++)
            {
                if (Environment.GetCommandLineArgs()[i] == _par)
                    return (true);
            }
            return (false);
        }
        public void OnInsert(bool _IsGpaph)
        {
            dgvTube.RLoad(TSKey);
            button2_Click(null, null);
            if (!_IsGpaph)
                return;
            using (FTubeGraph f = new FTubeGraph(dgvTube.Current as Tube, null))
            {
                if (f.Ok == "Ok")
                    f.ShowDialog();
                else
                    prs(f.Ok);
                if (f.IsChandge)
                    dgvTresh.RLoad(TSKey);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Int64 TubeId = -1;
            if (dgvTube.Current != null)
                TubeId = (dgvTube.Current as Tube).Id;
            string GroupName = null;
            if (dgvGroup.Current != null)
                GroupName = (dgvGroup.Current as Group).Name;
            int EtalonId = -1;
            if (dgvEtalon.Current != null)
                EtalonId = (dgvEtalon.Current as Etalon).Id;
            ucGraph1.Exec(TSKey.TSName, TubeId, GroupName, EtalonId);
        }

    }
}
