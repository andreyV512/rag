using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
//using System.Runtime.InteropServices;
using UPAR_common;

namespace RS232
{
    public class ComPort : ComPortBase, IDisposable
    {
        IntPtr hPort;
        IntPtr ptrUWO = IntPtr.Zero;

        public override void Close()
        {
            Win32Com.CancelIo(hPort);
            Win32Com.CloseHandle(hPort);
            hPort = (IntPtr)Win32Com.INVALID_HANDLE_VALUE;
            base.Close();
        }
        public override void Dispose()
        {
            Close();
        }
        public ComPort(ComPortPars _par, DOnPr _OnPr = null)
            : base(_par, _OnPr)
        {
            hPort = Win32Com.CreateFile(_par.Port, Win32Com.GENERIC_READ | Win32Com.GENERIC_WRITE, 0, IntPtr.Zero, Win32Com.OPEN_EXISTING, 0, IntPtr.Zero);
            if (hPort == (IntPtr)Win32Com.INVALID_HANDLE_VALUE)
                prfatal("Не могу прочитать параметры");
            Win32Com.DCB PortDCB = new Win32Com.DCB();
            if (!Win32Com.GetCommState(hPort, ref PortDCB))
                prfatal("Не могу прочитать параметры");
            PortDCB.BaudRate = _par.BaudRate;
            PortDCB.ByteSize = (byte)_par.ByteSize;
            PortDCB.Parity = (byte)_par.Parity;
            PortDCB.StopBits = (byte)_par.StopBits;
            Win32Com.COMMTIMEOUTS CommTimeouts = new Win32Com.COMMTIMEOUTS();
            if (!Win32Com.SetCommState(hPort, ref PortDCB))
                prfatal("Не могу установить параметры");
            CommTimeouts.ReadIntervalTimeout = (uint)_par.ReadIntervalTimeout;
            CommTimeouts.ReadTotalTimeoutConstant = (uint)_par.ReadTotalTimeoutConstant;
            CommTimeouts.ReadTotalTimeoutMultiplier = (uint)_par.ReadTotalTimeoutMultiplier;
            if (!Win32Com.SetCommTimeouts(hPort, ref CommTimeouts))
                prfatal("Не могу выставить задержки");
        }
        public override byte[] Read(int _size)
        {
            byte[] buf = new byte[_size];
            uint ret;
            if (!Win32Com.ReadFile(hPort, buf, (uint)_size, out ret, ptrUWO))
            {
                pr("Ошибка чтения");
                return (new byte[0]);
            }
            if (ProtocolLevel <= 0)
                pr("< " + ret.ToString() + ": " + byte_str(buf, ret));
            if (ret != _size)
            {
                pr("Не смогли прочитать требуемое количество байт");
                return (new byte[0]);
            }
            return (buf);
        }
        public override byte[] ReadSome(int _size)
        {
            byte[] buf = new byte[_size];
            uint ret;
            if (!Win32Com.ReadFile(hPort, buf, (uint)_size, out ret, ptrUWO))
            {
                pr("Ошибка чтения");
                return (new byte[0]);
            }
            if (ProtocolLevel <= 0)
                pr("< " + ret.ToString() + ": " + byte_strH(buf, ret));
            Array.Resize<byte>(ref buf, (int)ret);
            return (buf);
        }
        public override bool Write(byte[] _buf)
        {
            uint l;
            if (!Win32Com.WriteFile(hPort, _buf, (uint)_buf.Length, out l, ptrUWO))
            {
                pr("Ошибка записи");
                return (false);
            }
            if (l != _buf.Length)
            {
                pr("Не смогли записать требуемое количество байт");
                return (false);
            }
            if (ProtocolLevel <= 0)
                pr("> " + l.ToString() + ": " + byte_strH(_buf));
            return (true);
        }
        public override void ClearBuf()
        {
            byte[] buf;
            for (; ; )
            {
                buf = Read(500);
                if (buf.Length == 0)
                    break;
            }
        }
        public static ComPortBase Create(ComPortPars _par, DOnPr _OnPr = null)
        {
#if COMPORT_virtual
            return (new ComPortBase(_par, _OnPr));
#else
            return (new ComPort(_par, _OnPr));
#endif

        }
    }

}
