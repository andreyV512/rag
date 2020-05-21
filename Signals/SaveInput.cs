using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UPAR_common;

namespace Signals
{
    public class SaveInput: IDisposable
    {
        int[] input;
        int[] output;
        Save1730Pars pars;
        int count = 0;
        int size;
        SaveInput(Save1730Pars _pars)
        {
            pars = _pars;
            size = pars.Size * 512 * 1024/sizeof(int);
            count = 0;
            input = new int[size];
            output = new int[size];
        }

        public void Add(int _in, int _out)
        {
            if(count>=size)
                return;
            input[count]=_in;
            output[count]=_out;
            count++;
        }
        public void Save()
        {
            using (FileStream fs = new FileStream(pars.FileName, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    for (int i = 0; i < count; i++)
                    {
                        bw.Write(input[i]);
                        bw.Write(output[i]);
                    }
                }
                count = 0;
            }
        }
        public void Dispose()
        {
            Save();
        }
        public static SaveInput Create(Save1730Pars _pars)
        {
            if (!_pars.IsSave)
                return (null);
            if(_pars.Size<=0)
                return (null);
            if(_pars.FileName==null)
                return (null);
            return(new SaveInput(_pars));
        }
    }
}