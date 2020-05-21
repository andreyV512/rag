using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SQL;

namespace Defect
{
    public partial class UStatist : UserControl
    {
        public UStatist()
        {
            InitializeComponent();
            Clear();
        }
        public void RDraw()
        {
            int all;
            int brak;
            using (Select S = new Select("select * from dbo.Statist"))
            {
                if (!S.Read())
                {
                    Clear();
                    return;
                }
                all = Convert.ToInt32(S[0]);
                brak = Convert.ToInt32(S[1]);
            }
            int ok=all-brak;
            uAll.Value = all.ToString();
            uOk.Value = ok.ToString();
            uBrak.Value = brak.ToString();
        }
        public void Clear()
        {
            uAll.Value = null;
            uOk.Value = null;
            uBrak.Value = null;
        }
        public void Add(bool _OK)
        {
            if(_OK)
                new ExecSQL("update dbo.Statist set tubes=tubes+1");
            else
                new ExecSQL("update dbo.Statist set tubes=tubes+1, brak=brak+1");
            RDraw();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            new ExecSQL("update dbo.Statist set tubes=0, brak=0");
            Clear();
        }
    }
}
