using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InverterNS
{
    public class Reply
    {
        public enum EType { C, D, E, E1, E2, E3, None }
        public string result;
        public EType TP;
        public int DataSize { get; private set; }

        string abonent;
        public Reply(EType _type, int _DataSize)
        {
            DataSize = _DataSize;
            result = null;
            TP = _type;
        }
        public virtual bool parse(int _abonent, string _packet) 
        {
            abonent = _abonent.ToString("X2");
            switch (TP)
            {
                case EType.C:
                    return (parseC(_packet));
                case EType.D:
                    return (parseD(_packet));
                case EType.E:
                    return (parseE(_packet));
                case EType.E1:
                    return (parseE1(_packet));
                case EType.E2:
                    return (parseE2(_packet));
                case EType.E3:
                    return (parseE3(_packet));
                case EType.None:
                    return (true);
                default:
                    return (false);
            }
        }
        protected string CRC(string _packet)
        {
            int crc = 0;
            for (int i = 0; i < _packet.Length; i++)
            {
                crc += _packet[i];
            }
            crc &= 0xFF;
            return (crc.ToString("X2"));
        }
        bool parseE(string _l)
        {
            if (_l.Length < 11)
                return (false);
            int pos = _l.IndexOf(MitCOM.STX);
            if (pos < 0)
                return (false);
            string l = _l.Substring(pos);
            if (l.Length < 11)
                return (false);
            int x = 0;
            x += 1;
            if (l.Substring(x, 2) != abonent)
                return (false);
            x += 2;
            x += 4;
            if (l[x] != MitCOM.ETX)
                return (false);
            x += 1;
            if (l.Substring(x, 2) != CRC(l.Substring(1, 2 + 4)))
                return (false);
            x += 2;
            if (l[x] != MitCOM.CR)
                return (false);
            result = l.Substring(3, 4);
            return (true);
        }
        bool parseE1(string _l)
        {
            if (_l.Length < 9)
                return (false);
            int pos = _l.IndexOf(MitCOM.STX);
            if (pos < 0)
                return (false);
            string l = _l.Substring(pos);
            if (l.Length < 9)
                return (false);
            int x = 0;
            x += 1;
            if (l.Substring(x, 2) != abonent)
                return (false);
            x += 2;
            x += 2;
            if (l[x] != MitCOM.ETX)
                return (false);
            x += 1;
            if (l.Substring(x, 2) != CRC(l.Substring(1, 2 + 2)))
                return (false);
            x += 2;
            if (l[x] != MitCOM.CR)
                return (false);
            result = l.Substring(3, 2);
            return (true);
        }
        bool parseE2(string _l)
        {
            if (_l.Length < 13)
                return (false);
            int pos = _l.IndexOf(MitCOM.STX);
            if (pos < 0)
                return (false);
            string l = _l.Substring(pos);
            if (l.Length < 13)
                return (false);
            int x = 0;
            x += 1;
            if (l.Substring(x, 2) != abonent)
                return (false);
            x += 2;
            x += 6;
            if (l[x] != MitCOM.ETX)
                return (false);
            x += 1;
            if (l.Substring(x, 2) != CRC(l.Substring(1, 2 + 6)))
                return (false);
            x += 2;
            if (l[x] != MitCOM.CR)
                return (false);
            result = l.Substring(3, 6);
            return (true);
        }
        bool parseE3(string _l)
        {
            if (_l.Length < 7 + DataSize)
                return (false);
            int pos = _l.IndexOf(MitCOM.STX);
            if (pos < 0)
                return (false);
            string l = _l.Substring(pos);
            if (l.Length < 7 + DataSize)
                return (false);
            int x = 0;
            x += 1;
            if (l.Substring(x, 2) != abonent)
                return (false);
            x += 2;
            x += DataSize;
            if (l[x] != MitCOM.ETX)
                return (false);
            x += 1;
            if (l.Substring(x, 2) != CRC(l.Substring(1, 2 + DataSize)))
                return (false);
            x += 2;
            if (l[x] != MitCOM.CR)
                return (false);
            result = l.Substring(3, DataSize);
            return (true);
        }
        bool parseC(string _l)
        {
            if (_l.Length < 4)
                return (false);
            int pos = _l.IndexOf(MitCOM.ACK);
            if (pos < 0)
                return (false);
            string l = _l.Substring(pos);
            if (l.Length < 4)
                return (false);
            int x = 0;
            x += 1;
            if (l.Substring(x, 2) != abonent)
                return (false);
            x += 2;
            if (l[x] != MitCOM.CR)
                return (false);
            result = "ACK";
            return (true);
        }
        bool parseD(string _l)
        {
            if (_l.Length < 5)
                return (false);
            int pos = _l.IndexOf(MitCOM.NAK);
            if (pos < 0)
                return (false);
            string l = _l.Substring(pos);
            if (l.Length < 5)
                return (false);
            int x = 0;
            x += 1;
            if (l.Substring(x, 2) != abonent)
                return (false);
            x += 2;
            x += 1;
            if (l[x] != MitCOM.CR)
                return (false);

            result = l.Substring(3, 1);
            return (true);
        }
    }
}
