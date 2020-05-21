#if (Board1730)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Automation.BDaq;

using UPAR_common;

namespace Signals.Boards
{
    public class Board1730 : Board
    {
        byte[] buf_in;
        byte[] buf_out;
        public Board1730(PCIE1730pars _pars, DOnPr _OnPr)
            : base(_pars, _OnPr)
        {
            ctrl_in = new InstantDiCtrl();
            ctrl_out = new InstantDoCtrl();
            ctrl_in.SelectedDevice = new DeviceInformation(_pars.Devnum);
            ctrl_out.SelectedDevice = new DeviceInformation(_pars.Devnum);
            buf_in = new byte[portCount_in];
            buf_out = new byte[portCount_out];
        }

        public override void Dispose()
        {
            ctrl_in.Cleanup();
            ctrl_out.Cleanup();
            disposed = true;
        }
        InstantDiCtrl ctrl_in;
        InstantDoCtrl ctrl_out;
        int portStart = 0;

        public override int Read()
        {
            if (disposed)
                return (0);
            ErrorCode ret = ctrl_in.Read(portStart, portCount_in, buf_in);
            if (ret != ErrorCode.Success)
                throw new Exception("Board1730.Read: Ошибка: " + ret.ToString());
            values_in = BitConverter.ToInt32(buf_in, 0);
            return (values_in);
        }
        public override int ReadOut()
        {
            if (disposed)
                return (0);
            ErrorCode ret = ctrl_out.Read(portStart, portCount_out, buf_in);
            if (ret != ErrorCode.Success)
                throw new Exception("Board1730.ReadOut: Ошибка: " + ret.ToString());
            values_out = BitConverter.ToInt32(buf_in, 0);
            return (values_out);
        }
        public override void Write(int _values_out)
        {
            if (disposed)
                return;
            values_out = _values_out;
            buf_out = BitConverter.GetBytes(values_out);
            ErrorCode ret = ctrl_out.Write(portStart, portCount_out, buf_out);
            if (ret != ErrorCode.Success)
                throw new Exception("Board1730.Write: Ошибка: " + ret.ToString());
        }
        public override void WriteIn(int _values_in) { }
    }
}
#endif
