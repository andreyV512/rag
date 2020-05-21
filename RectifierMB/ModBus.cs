using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

using Protocol;
using RS232;
using UPAR_common;

namespace RectifierNS
{
    public class ModBus : IDisposable
    {
        ComPortBase comPort;
        RectifiersPars rectifiersPars;
        bool verbose;
        public ModBus(RectifiersPars _rectifiersPars, bool _verbose)
        {
            rectifiersPars = _rectifiersPars;
            comPort = ComPort.Create(rectifiersPars.ComPort, pr);
            verbose = _verbose;
        }
        void pr(string _msg)
        {
            if(verbose)
                ProtocolST.pr(_msg);
//            Application.DoEvents();
        }
        public void Dispose()
        {
            if (comPort != null)
                comPort.Dispose();
            comPort = null;
        }
        string ReadInputRegister(byte _cmd, int _abonent, int _pos, ref ushort _result)
        {

            byte[] query = new byte[] { Convert.ToByte(_abonent), _cmd, 0, Convert.ToByte(_pos), 0, 1, 0, 0 };
            Crc16.Add(query);
            if (!comPort.Write(query))
                return ("Не смогли записать");
            // 0 - абонент
            // 1 - ошибки
            // 2 - длина / код ошибки
            // 3 - данные
            // 4 - данные
            // 5 - crc
            // 6 - crc
            byte[] packet = comPort.Read(7);
            if (packet.Length != 7)
                return ("Не смогли прочитать");
            if (packet[0] != _abonent)
                return ("Не тот абонент");
            if ((packet[1] & 0x80) != 0)
                return ("Ошибка в ответе: " + packet[2].ToString());
            if (packet[2] != 2)
                return ("Не верная длина в ответе");
            if (!Crc16.Check(packet))
                return ("Не верная контрольная сумма");
            _result = BitConverter.ToUInt16(new byte[2] { packet[4], packet[3] }, 0);
            return (null);
        }
        string PresetSingleRegister(int _abonent, int _pos, ushort _data)
        {
            byte[] query = new byte[] { Convert.ToByte(_abonent), 6, 0, Convert.ToByte(_pos), Convert.ToByte((_data >> 8) & 0xff), Convert.ToByte(_data & 0xff), 0, 0 };
            Crc16.Add(query);
            if (!comPort.Write(query))
                return ("Не смогли записать");
            // 0 - абонент
            // 1 - ошибки
            // 2 - код ошиибки/регистр
            // 3 - регистр
            // 4 - 255
            // 5 - 255
            // 6 - crc
            // 7 - crc
            byte[] packet = comPort.Read(8);
            if (packet.Length != 8)
                return ("Не смогли прочитать");
            if (packet[0] != _abonent)
                return ("Не тот абонент");
            if ((packet[1] & 0x80) != 0)
                return ("Ошибка в ответе: " + packet[2].ToString());
            if (packet[2] != 0)
                return ("Не тот регистр");
            if (packet[3] != Convert.ToByte(_pos))
                return ("Не тот регистр");
            if (!Crc16.Check(packet))
                return ("Не верная контрольная сумма");
            return (null);
        }
        public string PresetSingleRegisterE(int _abonent, int _pos, ushort _data)
        {
            string ret = null;
            for (int i = 0; i < rectifiersPars.Iters; i++)
            {
                ret = PresetSingleRegister(_abonent, _pos, _data);
                if (ret == null)
                    break;
                pr(ret);
                Thread.Sleep(rectifiersPars.Timeout);
            }
            return (ret);
        }
        public string ReadRegisterE(byte _cmd, int _abonent, int _pos, ref ushort _result)
        {
            // cmd: input 4, holding 3
            string ret = null;
            for (int i = 0; i < rectifiersPars.Iters; i++)
            {
                ret = ReadInputRegister(_cmd, _abonent, _pos, ref _result);
                if (ret == null)
                    break;
                pr(ret);
                Thread.Sleep(rectifiersPars.Timeout);
            }
            return (ret);
        }
        public string ReadWriteReadE(int _abonent, int _pos, ushort _data)
        {
            ushort new_data = 0;
            string sret = ReadRegisterE(3, _abonent, _pos, ref new_data);
            if (sret != null)
                return (sret);
            if (_data == new_data)
                return (null);

            sret = PresetSingleRegisterE(_abonent, _pos, _data);
            if (sret != null)
                return (sret);

            sret = ReadRegisterE(3, _abonent, _pos, ref new_data);
            if (sret != null)
                return (sret);
            if (_data != new_data)
                return ("Чтение после записи не совпадает");
            return (null);
        }
        public string WriteCheck(int _abonent, int _pos, int _data)
        {
            return (ReadWriteReadE(_abonent, _pos, Convert.ToUInt16(_data)));
        }
        public string WriteCheck(int _abonent, int _pos, double _data)
        {
            return (ReadWriteReadE(_abonent, _pos, Convert.ToUInt16(_data * 10)));
        }
        public string WriteCheck(int _abonent, int _pos, bool _data)
        {
            return (ReadWriteReadE(_abonent, _pos, Convert.ToUInt16(_data ? 1 : 0)));
        }

        public string Write(int _abonent, int _pos, int _data)
        {
            return (PresetSingleRegisterE(_abonent, _pos, Convert.ToUInt16(_data)));
        }
        public string Write(int _abonent, int _pos, double _data)
        {
            return (PresetSingleRegisterE(_abonent, _pos, Convert.ToUInt16(_data * 10)));
        }
        public string Write(int _abonent, int _pos, bool _data)
        {
            return (PresetSingleRegisterE(_abonent, _pos, Convert.ToUInt16(_data ? 1 : 0)));
        }

        
        public string ReadInput(int _abonent, int _pos, ref int _result)
        {
            ushort res = 0;
            string ret = ReadRegisterE(4, _abonent, _pos, ref res);
            _result = Convert.ToInt32(res);
            return (ret);
        }
        public string ReadInput(int _abonent, int _pos, ref double _result)
        {
            ushort res = 0;
            string ret = ReadRegisterE(4, _abonent, _pos, ref res);
//            _result = Convert.ToInt32(Convert.ToDouble(res) / 10);
            _result = Convert.ToDouble(res) / 10;
            return (ret);
        }
        public string ReadHolding(int _abonent, int _pos, ref int _result)
        {
            ushort res = 0;
            string ret = ReadRegisterE(3, _abonent, _pos, ref res);
            _result = Convert.ToInt32(res);
            return (ret);
        }
        public string ReadHolding(int _abonent, int _pos, ref double _result)
        {
            ushort res = 0;
            string ret = ReadRegisterE(3, _abonent, _pos, ref res);
            _result = Convert.ToInt32(Convert.ToDouble(res) / 10);
            return (ret);
        }
        public string ReadHolding(int _abonent, int _pos, ref bool _result)
        {
            ushort res = 0;
            string ret = ReadRegisterE(3, _abonent, _pos, ref res);
            _result = res != 0 ? true : false;
            return (ret);
        }
    }
}