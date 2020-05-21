using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Protocol
{
    public partial class UCProtocol : UserControl
    {
        private FileStream fs;
        private string fpath;
        public UCProtocol()
        {
            InitializeComponent();
        }
        public void AddList(string _msg)
        {
            Protocol.BeginUpdate();
            if (!CBSave.Checked)
            {
                int vCount = Protocol.ClientSize.Height / Protocol.ItemHeight;
                for (; ; )
                {
                    if (Protocol.Items.Count <= 0)
                        break;
                    if (vCount > Protocol.Items.Count)
                        break;
                    Protocol.Items.RemoveAt(0);
                }

            }

            Protocol.Items.Add(_msg==null?"":_msg);
            Protocol.TopIndex = Protocol.Items.Count - 1;
            Protocol.EndUpdate();
            if (fs != null)
            {
                byte[] buf = Encoding.Default.GetBytes(_msg + "\n");
                fs.Write(buf, 0, buf.Length);
                fs.Flush();
            }
        }
        private void UCProtocol_Resize(object sender, EventArgs e)
        {
            int space = 2;
            Protocol.Top = CBFile.Top + CBFile.Height + space;
            Protocol.Height = ClientSize.Height - Protocol.Top - space;
            Protocol.Left = space;
            Protocol.Width = ClientSize.Width - space - space;
        }

        private void FsOpen()
        {
            if (fs != null)
                return;
            fpath = "Pro" + DateTime.Now.ToString("yyMMdd_HHmmss") + ".txt";
            fs = File.Open(fpath, FileMode.Create);
            AddList("Открыт файл: " + fpath);
        }
        public void FsClose()
        {
            if (fs == null)
                return;
            fs.Close();
            fs = null;
            AddList("Закрыт файл: " + fpath);
        }

        private void CBFile_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
                FsOpen();
            else
                FsClose();
        }
        public bool IsFile { get { return (CBFile.Checked); } set { CBFile.Checked = value; } }
        public bool IsSave { get { return (CBSave.Checked); } set { CBSave.Checked = value; } }
    }
}
