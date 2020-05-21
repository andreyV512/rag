using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RAGLIB;

namespace SignalLib
{
    public class DASP52032_real : BoardBase
    {
        public override void Dispose()
        {
            disposed = true;
            int err = DASP52032Connector.DASP52032_Release((byte)devnum);
            if (err != 0)
                FN.fatal("DASP52032_real.Dispose: Ошибка: " + DASP52032Connector.GetError(err));
        }
        public DASP52032_real(BoardPars _pars,DOnPr _OnPr = null)
            : base(_pars,_OnPr)
        {
            int err;
            int DllVersion=0;
            err = DASP52032Connector.DASP52032_GetDllVersion(ref DllVersion);
            pr("DASP52032_real.DASP52032_real: DllVersion=" + DllVersion.ToString());
            err = DASP52032Connector.DASP52032_QuickInstalled((byte)devnum, 0);
            if(err!=0)
                FN.fatal("DASP52032_real.DASP52032_real: Ошибка: " + DASP52032Connector.GetError(err));

        }
        public override byte[] Read()
        {
            if (disposed)
                return (null);
            Int16 data=0;
            int err = DASP52032Connector.DASP52032_ReadGpio((byte)devnum, ref data);
            if (err != 0)
                FN.fatal("DASP52032_real.Read: Ошибка: " + DASP52032Connector.GetError(err));
            values_in[0]=(byte)(data&0xFF);
            values_in[1]=(byte)((data>>8)&0xFF);
            return (values_in);
        }
        public override byte[] ReadOut()
        {
            if (disposed)
                return (null);
            Int16 data = 0;
            int err = DASP52032Connector.DASP52032_ReadBackGpio((byte)devnum, ref data);
            if (err != 0)
                FN.fatal("DASP52032_real.ReadOut: Ошибка: " + DASP52032Connector.GetError(err));
            values_out[0] = (byte)(data & 0xFF);
            values_out[1] = (byte)((data >> 8) & 0xFF);
            return (values_out);
        }
        public override void Write(byte[] _values_out)
        {
            if (disposed)
                return;
            Int16 data = 0;
            data |= (Int16)(_values_out[0]);
            data |= (Int16)(_values_out[1]<<8);
            int err = DASP52032Connector.DASP52032_WriteGpio((byte)devnum, data);
            if (err != 0)
                FN.fatal("DASP52032_real.Write: Ошибка: " + DASP52032Connector.GetError(err));
        }
    }
}
