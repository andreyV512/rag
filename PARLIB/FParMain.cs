using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PARLIB
{
    public partial class FParMain : Form
    {
        ParMainLite parMainLite;
        public FParMain()
        {
            InitializeComponent();
        }
        public FParMain(ParMainLite _parMainLite)
        {
            InitializeComponent();
            SaveVisible = true;
            parMainLite = _parMainLite;
        }

        private void FParMainU_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                int splitter1 = pdView1.Splitter1;
                int splitter2 = pdView1.Splitter2;
                int splitterDistance = 0;
                parMainLite.Wins.LoadFormRect(this, ref splitter1, ref splitter2, ref splitterDistance);
                pdView1.Splitter1 = splitter1;
                pdView1.Splitter2 = splitter2;
                pdView1.SelectedObject = parMainLite;
                if (parMainLite.Source == ESource.File)
                    сохранитьВФайлToolStripMenuItem.Text = "Сохранить в СУБД";
                else
                    сохранитьВФайлToolStripMenuItem.Text = "Сохранить в файл";
            }

        }

        private void FParMainU_FormClosed(object sender, FormClosedEventArgs e)
        {
            parMainLite.Wins.SaveFormRect(this, pdView1.Splitter1, pdView1.Splitter2, 0);
            pdView1.Save();
        }
        public PropertyGrid propertyGrid2 { get { return (pdView1.propertyGrid2); } }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (About f = new About())
            {
                f.ShowDialog();
            }
        }

        private void сохранитьВФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (parMainLite.Source == ESource.SQL)
                parMainLite.SaveToFile();
            else
                parMainLite.SaveToSQL();
        }
        public bool SaveVisible
        {
            get { return (сохранитьВФайлToolStripMenuItem.Visible); }
            set { сохранитьВФайлToolStripMenuItem.Visible = value; }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            parMainLite.Save();
        }

    }
}
