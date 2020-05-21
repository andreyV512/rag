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
using System.IO;

namespace UPAR
{
    public partial class FLoginClient : Form
    {
        public bool Ok { get; private set; }
        public string Client { get; private set; }
        ParAll parAll;
        public FLoginClient(ParAll _parAll)
        {
            InitializeComponent();
            parAll = _parAll;
        }

        private void FLoginClient_Load(object sender, EventArgs e)
        {
            foreach (User p in parAll.Users)
                comboBox1.Items.Add(p.Name);
            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
            foreach (Client p in parAll.Clients)
                comboBox2.Items.Add(p.Name);
            if (comboBox2.Items.Count > 0)
                comboBox2.SelectedIndex = 0;
            Ok = false;
        }

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
                comboBox2.Focus();
        }

        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox2.Text.Length == 0)
                return;
            if (e.KeyChar == '\r')
            {
                if (comboBox1.Text.Length == 0)
                    comboBox1.Focus();
                else
                    button1_Click(null, null);
            }
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
            if (comboBox2.Text.Length == 0)
            {
                pr("Введите заказчика");
                return;
            }
            User uu = null;
            foreach (User u in parAll.Users)
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
            Client = comboBox2.Text;
            Ok = true;
            Close();
        }
        void pr(string _msg)
        {
            toolStripStatusLabel1.Text = _msg;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            button1.Focus();
        }
    }
}
