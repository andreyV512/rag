using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using Protocol;

namespace PARLIB
{
    [DisplayName("Окна")]
    [Sortable]
    public class L_WindowLPars : ParListBase<WindowLPars>
    {
        public static L_WindowLPars CurrentWins { get; set; }
        public new WindowLPars this[string _name]
        {
            get
            {
                foreach (WindowLPars p in this)
                {
                    if (p.Name == _name)
                        return (p);
                }
                return (null);
            }
        }
        public WindowLPars this[Control _form]
        {
            get
            {
                return (this[GetName(_form)]);
            }
        }
        public void LoadFormRect(Control _form)
        {
            int splitter1 = 0;
            int splitter2 = 0;
            int splitterDistance = 0;
            bool IsVisible = false;
            LoadFormRect(_form, ref splitter1, ref splitter2, ref splitterDistance, ref IsVisible);
        }
        public void LoadFormRect(Control _form, ref bool _IsVisible)
        {
            int splitter1 = 0;
            int splitter2 = 0;
            int splitterDistance = 0;
            LoadFormRect(_form, ref splitter1, ref splitter2, ref splitterDistance, ref _IsVisible);
        }
        public void LoadFormRect(Control _form, ref int _splitter1, ref int _splitter2, ref int _splitterDistance)
        {
            bool IsVisible = false;
            LoadFormRect(_form, ref _splitter1, ref _splitter2, ref _splitterDistance, ref IsVisible);
        }
        public bool IsVisible(Control _form)
        {
            WindowLPars win = this[_form];
            return (win==null?false:win.IsVisible);
        }
        public void LoadFormRect(Control _form, ref int _splitter1, ref int _splitter2, ref int _splitterDistance, ref bool _IsVisible)
        {
            WindowLPars win = this[_form];
            if (win == null)
                return;
            try
            {
                SetWinSize(win);
                _form.Left = win.Left;
                _form.Top = win.Top;
                _form.Width = win.Width;
                _form.Height = win.Height;
            }
            catch 
            {
                //_form.Left = 100;
                //_form.Top = 100;
                //_form.Width = 200;
                //_form.Height = 100;
            }
            _splitter1 = win.Splitter1;
            _splitter2 = win.Splitter2;
            _splitterDistance = win.SplitterDistance;
            _IsVisible = win.IsVisible;
            ProtocolST.pr("Load: " + win.ToStringFull());
        }
        public void SaveFormRect(Control _form)
        {
            SaveFormRect(_form, 0, 0, 0, false);
        }
        public void SaveFormRect(Control _form, bool _IsVisible)
        {
            SaveFormRect(_form, 0, 0, 0, _IsVisible);
        }
        public void SaveFormRect(Control _form, int _splitter1, int _splitter2, int _splitterDistance)
        {
            SaveFormRect(_form, _splitter1, _splitter2, _splitterDistance, false);
        }
        static string GetName(Control _form)
        {
            string save_name = _form.Text;
            FieldInfo fi = _form.GetType().GetField("SaveName");
            if (fi != null)
                save_name = fi.GetValue(_form) as string;
            return (save_name);
        }
        public void SaveFormRect(Control _form, int _splitter1, int _splitter2, int _splitterDistance, bool _IsVisible)
        {
            WindowLPars win = this[_form];
            if (win == null)
            {
                win = this.AddNew() as WindowLPars;
                win.Name = GetName(_form);
            }
            win.Left = _form.Left;
            win.Top = _form.Top;
            win.Width = _form.Width;
            win.Height = _form.Height;
            win.Splitter1 = _splitter1;
            win.Splitter2 = _splitter2;
            win.SplitterDistance = _splitterDistance;
            win.IsVisible = _IsVisible;
            ProtocolST.pr("Save: " + win.ToStringFull());
        }
        void SetWinSize(WindowLPars _w)
        {
            int min = 100;
            Rectangle r = SystemInformation.VirtualScreen;

            if (_w.Width < min)
                _w.Width = min;
            if (_w.Width > r.Width * 2)
                _w.Width = r.Width * 2;
            if (_w.Left + _w.Width < min)
                _w.Left = min - _w.Width;
            if (_w.Left > r.Width)
                _w.Left = r.Width - min;

            if (_w.Height < min)
                _w.Height = min;
            if (_w.Height > r.Height * 2)
                _w.Height = r.Height * 2;
            if (_w.Top + _w.Height < min)
                _w.Top = min - _w.Height;
            if (_w.Top > r.Height)
                _w.Top = r.Height - min;
        }
    }
}
