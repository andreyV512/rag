using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defect.ACS
{
    public static class RByteConverter
    {
        static bool Check(byte[] _data, int _start, int _size)
        {
            if (_data == null)
                return (false);
            if (_start < 0)
                return (false);
            if (_start >= _data.Length - 1)
                return (false);
            if (_size <= 0)
                return (false);
            if (_start + _size > _data.Length)
                return (false);
            return (true);
        }
        static void Fill(bool _direct, byte[] _src, byte[] _trg)
        {
            if (_src == null || _trg == null)
                return;
            for (int i = 0; i < _trg.Length; i++)
            {
                if (i < _src.Length)
                {
                    if (_direct)
                        _trg[i] = _src[i];
                    else
                        _trg[i] = _src[_src.Length - i - 1];
                }
                else
                    _trg[i] = 0;
            }
        }
        public static byte[] IntToByte(bool _direct, int _isrc, int _size)
        {
            if (_size < 0)
                return (null);
            byte[] ret = new byte[_size];
            Fill(_direct, BitConverter.GetBytes(_isrc), ret);
            return (ret);
        }
        public static byte[] UIntToByte(bool _direct, uint _uisrc, int _size)
        {
            if (_size < 0)
                return (null);
            byte[] ret = new byte[_size];
            Fill(_direct, BitConverter.GetBytes(_uisrc), ret);
            return (ret);
        }
        public static bool IntToByte(bool _direct, int _isrc, byte[] _trg, int _start, int _size)
        {
            if (!Check(_trg, _start, _size))
                return (false);
            byte[] trg0 = IntToByte(_direct, _isrc, _size);
            for (int i = 0; i < _size; i++)
                _trg[_start + i] = trg0[i];
            return (true);
        }
        public static bool UIntToByte(bool _direct, uint _isrc, byte[] _trg, int _start, int _size)
        {
            if (!Check(_trg, _start, _size))
                return (false);
            byte[] trg0 = UIntToByte(_direct, _isrc, _size);
            for (int i = 0; i < _size; i++)
                _trg[_start + i] = trg0[i];
            return (true);
        }
        public static int ByteToInt(bool _direct, byte[] _src)
        {
            if (_src==null||_src.Length==0)
                return (0);
            byte[] trg = new byte[sizeof(int)];
            Fill(_direct, _src, trg);
            return (BitConverter.ToInt32(trg,0));
        }
        public static uint ByteToUInt(bool _direct, byte[] _src)
        {
            if (_src == null || _src.Length == 0)
                return (0);
            byte[] trg = new byte[sizeof(int)];
            Fill(_direct, _src, trg);
            return (BitConverter.ToUInt32(trg, 0));
        }
        public static int ByteToInt(bool _direct, byte[] _src, int _start, int _size)
        {
            if (!Check(_src, _start, _size))
                return (0);
            byte[] trg = new byte[_size];
            for (int i = 0; i < _size; i++)
                trg[i] = _src[_start+i];
            return (ByteToInt(_direct, trg));
        }
        public static uint ByteToUInt(bool _direct, byte[] _src, int _start, int _size)
        {
            if (!Check(_src, _start, _size))
                return (0);
            byte[] trg = new byte[_size];
            for (int i = 0; i < _size; i++)
                trg[i] = _src[_start + i];
            return (ByteToUInt(_direct, trg));
        }
    }
}
