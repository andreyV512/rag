using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InverterNS
{
    public class Request
    {
        private int data_size;
        private string data;
        string cmd;
        public enum Etype { A, A1, B }

        public Request(Etype _type, string _cmd) : this(_type, _cmd, null) { }
        public Request(Etype _type, string _cmd, string _data)
        {
            cmd = _cmd;
            switch (_type)
            {
                case Etype.B:
                    data_size = 0;
                    break;
                case Etype.A:
                    data_size = 4;
                    break;
                case Etype.A1:
                    data_size = 2;
                    break;
                default:
                    data_size = 0;
                    break;
            }
            data = _data;
        }
        public int DataSize
        {
            get
            {
                return (data_size);
            }
        }
        protected string CRC1(string _packet)
        {
            int crc = 0;
            for (int i = 1; i < _packet.Length; i++)
            {
                crc += _packet[i];
            }
            crc &= 0xFF;
            return (crc.ToString("X2"));
        }
        public byte[] Get(int _abonent, int _timeout)
        {
            string packet = "";
            packet += MitCOM.ENQ;
            packet += _abonent.ToString("X2");
            packet += cmd;
            packet += char.Parse(_timeout.ToString("X"));
            if (data_size != 0)
                packet += data;
            packet += CRC1(packet);
            packet += MitCOM.CR;
            return (System.Text.Encoding.ASCII.GetBytes(packet));
        }
    }
}
