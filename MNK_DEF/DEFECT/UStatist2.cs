using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Share;
using SQL;

namespace Defect
{
    public partial class UStatist2 : UserControl
    {
        public UStatist2()
        {
            InitializeComponent();
            Clear();
        }
        public void RDraw()
        {
            using (Select S = new Select("select * from dbo.Statist2"))
            {
                if (!S.Read())
                {
                    Clear();
                    CheckTable();
                    return;
                }
                uOk.Value = Convert.ToInt32(S[0]).ToString();
                uClass2.Value = Convert.ToInt32(S[1]).ToString();
                uBrak.Value = Convert.ToInt32(S[2]).ToString();
                uAll.Value = (Convert.ToInt32(S[0]) + Convert.ToInt32(S[1]) + Convert.ToInt32(S[2])).ToString();
            }
        }
        public void Clear()
        {
            uAll.Value = null;
            uOk.Value = null;
            uClass2.Value = null;
            uBrak.Value = null;
        }
        public void Add(EClass _rclass)
        {
            switch (_rclass)
            {
                case EClass.Brak:
                    new ExecSQL("update dbo.Statist2 set brak=brak+1");
                    break;
                case EClass.Class2:
                    new ExecSQL("update dbo.Statist2 set class2=class2+1");
                    break;
                default:
                    new ExecSQL("update dbo.Statist2 set class1=class1+1");
                    break;
            }
            RDraw();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            new ExecSQL("update dbo.Statist2 set class1=0, class2=0, brak=0");
            RDraw();
        }
        void CheckTable()
        {
            int nn = 0;
            using (Select S = new Select("select count(*) as nn from INFORMATION_SCHEMA.TABLES where table_type='BASE TABLE' and TABLE_SCHEMA='dbo' and table_name='Statist2'"))
            {
                if (!S.Read())
                    throw new Exception("UStatist2.CheckTables: " + S.SQL + " - не нашли записей");
                nn = (int)S[0];
            }
            if (nn != 1)
            {
                new ExecSQL("CREATE TABLE [dbo].[Statist2]( [Class1] [int] NOT NULL, [Class2] [int] NOT NULL, [Brak] [int] NOT NULL ) ON [PRIMARY]");
                new ExecSQL("insert into [dbo].[Statist2] values(0,0,0)");
            }
        }
    }
}
