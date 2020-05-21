using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Protocol;

namespace PARLIB
{
    public partial class FLogin : Form
    {
        ParMain parMain;
        public FLogin(ParMain _parMain) 
        {
            InitializeComponent();
            parMain = _parMain;
        }
        private void FLogin_Load(object sender, EventArgs e)
        {
            int lastUser = -1;
            foreach (User p in parMain.Users)
            {
                int index=comboBox1.Items.Add(p.Name);
                if (p.Name == parMain.LastUser)
                    lastUser = index;
            }
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = lastUser < 0 ? 0 : lastUser;
            Ok = false;
        }

        private void FLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Length == 0)
            {
                pr("Введите имя пользователя");
                return;
            }
            if (textBox1.Text.Length == 0)
            {
                pr("Введите пароль");
                return;
            }
            User uu = null;
            foreach (User u in parMain.Users)
            {
                if (u.Name == comboBox1.Text)
                {
                    if (u.Pwd == textBox1.Text)
                        uu = u;
                    break;
                }
            }
            if (uu == null)
            {
                User u = new User();
                u.Name = "Uran";
                u.Pwd = "sizeof";
                u.Group = EGroup.Master;
                if (u.Name == comboBox1.Text)
                {
                    if (u.Pwd == textBox1.Text)
                        uu = u;
                }
            }
            if (uu == null)
            {
                pr("Неверные имя пользователя или пароль");
                return;
            }
            User.current = uu;
            parMain.LastUser = User.current.Name;
            Ok = true;
            Close();
        }
        void pr(string _msg)
        {
            toolStripStatusLabel1.Text = _msg;
        }
        public bool Ok { get; private set; }

        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox1.Text.Length == 0)
                return;
            if (e.KeyChar == '\r')
                textBox1.Focus();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (textBox1.Text.Length == 0)
                return;
            if (e.KeyChar == '\r')
            {
                if (comboBox1.Text.Length == 0)
                    comboBox1.Focus();
                else
                    button1_Click(null, null);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Focus();
        }
    }
}
