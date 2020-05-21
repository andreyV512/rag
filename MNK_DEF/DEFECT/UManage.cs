using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ResultLib;
using Protocol;
using PARLIB;
using UPAR;
using ResultLib.SG;
using Share;

namespace Defect
{
    public partial class UManage : UserControl
    {
        public UManage()
        {
            InitializeComponent();
            Clear();
        }
        void Clear()
        {
            udbTube1.Clear();
        }
        public void Init()
        {
            if (ParAll.ST.Defect.IsDBS)
                toolTip1.SetToolTip(lSettings, "Для локальной работы запустите с параметром local");
            else
                toolTip1.SetToolTip(lSettings, "Для работы c СУБД запустите БЕЗ параметра local");
            lSettings.Text = ParAll.ST.Defect.IsDBS ? "Работа с СУБД" : "Работа локальная";
            LoadSettings();
        }
        public void LoadSettings()
        {
            udbTube1.TypeSize = ParAll.CTS.Name;
            usg1.LoadSettings();
        }
        public new void Dispose()
        {
            base.Dispose();
        }

        void pr(string _msg) { ProtocolST.pr("UManage: " + _msg); }

        private void UManage_Resize(object sender, EventArgs e)
        {
            //            int space=4;
        }
        public SGState sgState { get { return (usg1.State); } set { usg1.State = value; } }
        public void DrawStatist() { uStatist1.RDraw(); }
        public void AddStatist() { uStatist1.Add(RK.ST.result.Sum.RClass); }

        //public void OnStatus(uint _level, string _msg)
        //{
        //    if (_level == 0)
        //        lStatus0.Text = _msg;
        //    else
        //        lStatus1.Text = _msg;
        //}
    }
}
