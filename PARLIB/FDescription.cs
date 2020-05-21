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
    public partial class FDescription : Form
    {
        string key;
        string name;
        Access acc;
        Access acc_buf;
        public string SaveName;
        ParMainLite parMainLite;
        public FDescription(Access _acc, string _key, string _name, ParMainLite _parMainLite)
        {
            acc = _acc;
            acc_buf = new Access(acc);

            key = _key;
            name = _name;
            InitializeComponent();
            parMainLite = _parMainLite;
        }

        private void FDescription_Load(object sender, EventArgs e)
        {
            SaveName = Name;
            Text = name + " (в XML: " + key + ")";
            parMainLite.Wins.LoadFormRect(this);
            propertyGrid1.SelectedObject = acc_buf;
        }

        private void FDescription_FormClosed(object sender, FormClosedEventArgs e)
        {
            parMainLite.Wins.SaveFormRect(this);
            if (no_save)
                return;
            acc.Copy(acc_buf);
        }
        bool no_save = false;
        private void button1_Click(object sender, EventArgs e)
        {
            no_save = true;
            Close();
        }
    }
}
