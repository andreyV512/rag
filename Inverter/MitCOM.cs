using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using RS232;
using UPAR_common;

namespace InverterNS
{

    public class MitCOM
    {
        public const char ENQ = (char)0x05;
        public const char STX = (char)0x02;
        public const char ETX = (char)0x03;
        public const char ACK = (char)0x06;
        public const char NAK = (char)0x15;
        public const char CR = (char)0x0D;
        public const char LF = (char)0x0A;

        ComPortBase comPort;
        Request request = null;
        List<Reply> Lreply = new List<Reply>();
        object SyncObj;
        public MitCOM(ComPortPars _parCOM, ComPort.DOnPr _OnPr) : this(_parCOM, _OnPr, new object()) { }
        public MitCOM(ComPortPars _parCOM, ComPort.DOnPr _OnPr, object _SyncObj)
        {
            comPort = ComPort.Create(_parCOM, _OnPr);
            SyncObj = _SyncObj;
        }
        public void Dispose()
        {
            lock (SyncObj)
            {
                comPort.Dispose();
            }
        }
        public void Clear()
        {
            lock (SyncObj)
            {
                request = null;
                Lreply.Clear();
            }
        }
        public void AddReply(Reply.EType _replyType)
        {
            AddReply(_replyType, 0);
        }
        public void AddReply(Reply.EType _replyType, int _replySize)
        {
            lock (SyncObj)
            {
                Lreply.Add(new Reply(_replyType, _replySize));
            }
        }

        public void Exec(int _abonent, int _timeout, string _cmd, Request.Etype _request, string request_data, out Reply.EType _replyType, out string _replyData)
        {
            lock (SyncObj)
            {
                request = new Request(_request, _cmd, request_data);
                _replyType = Reply.EType.None;
                _replyData = null;
                if (!comPort.Write(request.Get(_abonent, _timeout)))
                {
                    _replyData = "Не смогли послать запрос";
                    return;
                }
                string packet = string.Empty;
                for (; ; )
                {
                    byte[] c = comPort.ReadSome(1);
                    if (c.Length != 1)
                    {
                        if (packet.Length == 0)
                            _replyData = "Нет ответа";
                        else
                            _replyData = "Ответ не разобран";
                        return;
                    }
                    packet += Encoding.ASCII.GetString(c);
                    foreach (Reply p in Lreply)
                    {
                        if (p.parse(_abonent, packet))
                        {
                            _replyType = p.TP;
                            _replyData = p.result;
                            return;
                        }
                    }
                }
            }
        }
    }
}
