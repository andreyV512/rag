using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using PARLIB;
using UPAR;

namespace ResultLib
{
    public partial class FResultPars : Form
    {
        public FResultPars(IResultPars _pars)
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = _pars;
        }

        private void FRTPars_Load(object sender, EventArgs e)
        {
            L_WindowLPars.CurrentWins.LoadFormRect(this);
        }

        private void FRTPars_FormClosed(object sender, FormClosedEventArgs e)
        {
            L_WindowLPars.CurrentWins.SaveFormRect(this);
        }

        private void FRTPars_Resize(object sender, EventArgs e)
        {
            propertyGrid1.Width = ClientSize.Width;
            propertyGrid1.Height = panel1.Top;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (propertyGrid1.SelectedObject as IResultPars).SaveSettings();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NeedRecalc = true;
            Close();
        }
        public bool NeedRecalc { get; private set; }

        private void button3_Click(object sender, EventArgs e)
        {
            string fname = "";
            if (Result.SaveDialog("Сохранение настроек результата", "файлы (*.xml)|*.xml|Все файлы (*.*)|*.*", ref fname) != DialogResult.OK)
                return;
            using (FileStream s = new FileStream(fname, FileMode.Create))
                (propertyGrid1.SelectedObject as IResultPars).Serialize(s);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string fname = "";
            if (Result.OpenDialog("Загрузка настроек результата", "файлы (*.xml)|*.xml|Все файлы (*.*)|*.*", ref fname) != DialogResult.OK)
                return;
            using (FileStream s = new FileStream(fname, FileMode.Open))
                propertyGrid1.SelectedObject = Result.DeSerialize(propertyGrid1.SelectedObject.GetType(), s); 
        }
        public IResultPars WorkPars
        {
            get { return (propertyGrid1.SelectedObject as IResultPars); }
        }
    }
}
