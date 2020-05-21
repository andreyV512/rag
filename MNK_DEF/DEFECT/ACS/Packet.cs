using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defect.ACS
{
    public class Packet
    {
        protected bool IsLittleEndian;
        public Packet(bool _IsLittleEndian)
        {
            IsLittleEndian = _IsLittleEndian;
            Clear();
        }

        public int abonent = 2;
        public int command = 1;
        public byte[] data = null;
        public enum EState { None, ErrorSize, NoData, ErrorCRC, Ok }
        public EState State { get; protected set; }

        protected void Clear()
        {
            abonent = 0;
            command = 0;
            data = null;
            State = EState.None;
        }
        public void AddCRC(byte[] _data)
        {
            RByteConverter.UIntToByte(IsLittleEndian, CRC(_data, _data.Length - 2), _data, _data.Length - 2, 2);
        }
        public uint CRC(byte[] _data, int _sz)
        {
            if (_data.Length <= 0)
                return (0);
            uint CRC = 0x7FFFFFFF;
            for (int i = 0; i < _sz; i++)
            {
                CRC ^= _data[i];
                for (byte j = 0; j < 8; j++)
                {
                    bool testbit = (CRC & 0x0001) > 0;
                    CRC = (CRC >> 1) & 0x7FFF;
                    if (testbit)
                        CRC ^= 0xA001;
                }
            }
            return (CRC);
        }
        public string SState
        {
            get
            {
                switch (State)
                {
                    case EState.ErrorCRC:
                        return ("Ошибка CRC");
                    case EState.ErrorSize:
                        return ("Ошибка Размера");
                    case EState.NoData:
                        return ("Нет данных");
                    case EState.Ok:
                        return ("Ok");
                    default:
                        return ("Неопределено");
                }
            }
        }
    }
    public class PacketOut : Packet
    {
        public PacketOut(bool _IsLittleEndian) : base(_IsLittleEndian) { }
        public byte[] Serial
        {
            get
            {
                int dl = data == null ? 0 : data.Length;
                byte[] ret = new byte[dl + AdditionalSize];
                RByteConverter.IntToByte(IsLittleEndian, dl + AdditionalSize, ret, 0, 2);
                int x = 2;
                ret[x++] = Convert.ToByte(abonent & 0xFF);
                ret[x++] = Convert.ToByte(command & 0xFF);
                for(int i=0;i<dl;i++)
                    ret[x++] = data[i];
                AddCRC(ret);
                State = EState.Ok;
                return (ret);
            }
        }
        public static int AdditionalSize { get { return (6); } }
    }
    public class PacketIn : Packet
    {
        public PacketIn(bool _IsLittleEndian) : base(_IsLittleEndian) { }
        public byte[] Serial
        {
            set
            {
                if (value.Length < AdditionalSize)
                {
                    Clear();
                    State = EState.ErrorSize;
                    return;
                }
                int size = Convert.ToInt32(0);
                if (size > value.Length)
                {
                    Clear();
                    State = EState.ErrorSize;
                    return;
                }
                uint crc = CRC(value, size - 2);
                uint crc_packet = RByteConverter.ByteToUInt(IsLittleEndian, value, size - 2, 2);
                if(crc!=crc_packet)
                {
                    Clear();
                    State = EState.ErrorCRC;
                    return;
                }
                abonent = Convert.ToInt32(value[1]);
                command = Convert.ToInt32(value[2]);
                data = new byte[size - AdditionalSize];
                for (int i = 0; i < size - AdditionalSize; i++)
                    data[i] = value[i + 3];
                State = EState.Ok;
            }
        }
        public static int AdditionalSize { get { return (5); } }
    }
}
