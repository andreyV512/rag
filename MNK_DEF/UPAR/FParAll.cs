using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using PARLIB;
using Protocol;

namespace UPAR
{
    public partial class FParAll : FParMain
    {
        public FParAll()
            : base(ParAll.ST as ParMainLite)
        {
            InitializeComponent();
        }
        private void FParAll_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                if (User.current.Group == EGroup.Master)
                {
                    ToolStripMenuItem pr = FileItem.DropDownItems.Add("Протокол") as ToolStripMenuItem;
                    pr.Click += new EventHandler(pr_Click);
                }
                else
                    propertyGrid2.ContextMenuStrip = null;
                ProtocolPar pp = ParAll.ST.Protocol;
                Form protocol = ProtocolST.Instance(this, pp.Period, pp.IsFile, pp.IsSave);
                if (protocol!=null)
                {
                    ParAll.ST.Wins.LoadFormRect(protocol);
                    if (pp.IsVisible)
                        protocol.Show();
                }
            }
        }

        void pr_Click(object sender, EventArgs e)
        {
            ProtocolST.Show();
        }

        private void FParAll_FormClosed(object sender, FormClosedEventArgs e)
        {
            ProtocolPar pp = ParAll.ST.Protocol;
            Form protocol = ProtocolST.Instance(this);
            if (protocol != null)
            {
                pp.IsFile = ProtocolST.IsFile;
                pp.IsSave = ProtocolST.IsSave;
                pp.IsVisible = protocol.Visible;
                ParAll.ST.Wins.SaveFormRect(protocol);
                protocol.Close();
            }
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
        public bool LoginClient()
        {
            using (FLoginClient f = new FLoginClient(ParAll.ST))
            {
                f.ShowDialog();
                return (f.Ok);
            }
        }
    }
}
