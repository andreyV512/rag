using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPAR_common;

namespace RS232
{
    public class ComPortBase : IDisposable
    {
        public delegate void DOnPr(string _msg);
        public event DOnPr OnPr = null;
        protected void pr(string _msg)
        {
            if (OnPr != null)
                OnPr(_msg);
        }
        protected void prfatal(string _msg)
        {
            pr(_msg);
            throw new Exception("ComPort: " + _msg);
        }
        public uint ProtocolLevel;

        protected bool opened = false; public bool Opened { get { return (opened); } }

        public virtual void Dispose()
        {
            Close();
        }
        public virtual void Close()
        {
            opened = false;
            pr("Закрыт");
        }
        public ComPortBase(ComPortPars _par, DOnPr _OnPr = null)
        {
            OnPr = _OnPr;
            ProtocolLevel = 0;
            opened = true;
            pr("Открыт");
        }
        public virtual byte[] Read(int _size)
        {
            return (new byte[0]);
        }
        public virtual byte[] ReadSome(int _size)
        {
            return (new byte[0]);
        }
        public virtual bool Write(byte[] _buf)
        {
            return (true);
        }
        public virtual void ClearBuf() { }
        protected string byte_str(byte[] _buf)
        {
            string ret = "";
            for (int i = 0; i < _buf.Length; i++)
            {
                ret += _buf[i].ToString();
                ret += ".";
            }
            return (ret);
        }
        protected string byte_str(byte[] _buf, uint _nn)
        {
            string ret = "";
            for (uint i = 0; i < _nn; i++)
            {
                ret += _buf[i].ToString();
                ret += ".";
            }
            return (ret);
        }
        protected string byte_strH(byte[] _buf)
        {
            string ret = "";
            for (int i = 0; i < _buf.Length; i++)
            {
                ret += _buf[i].ToString("X2");
                ret += ".";
            }
            return (ret);
        }
        protected string byte_strH(byte[] _buf, uint _nn)
        {
            string ret = "";
            for (uint i = 0; i < _nn; i++)
            {
                ret += _buf[i].ToString("X2");
                ret += ".";
            }
            return (ret);
        }
    }
}
