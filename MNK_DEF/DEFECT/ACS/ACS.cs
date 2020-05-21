using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Protocol;
using Share;
using RS232;
using UPAR;
using ResultLib;

namespace Defect.ACS
{
    public class ACS : IDisposable
    {
        ComPortBase comPort = null;
        public bool verbose = true;
        bool IsLittleEndian;

        public ACS(bool _IsLittleEndian)
        {
            IsLittleEndian = _IsLittleEndian;
#if ACS_VIRTUAL
            comPort=new ComPortBase(ParAll.ST.Defect.ComPortASC, pr);
#else
            comPort = ComPort.Create(ParAll.ST.Defect.ComPortASC, pr);
#endif
        }
        public void Dispose()
        {
            if (comPort != null)
                comPort.Dispose();
            comPort = null;
        }
        void pr(string _msg)
        {
            if (verbose)
                ProtocolST.pr(_msg);
        }
        public string Test()
        {
            if (!comPort.Write(new PacketOut(IsLittleEndian) { command = 1 }.Serial))
                return ("Не смогли записать");
            PacketIn pIn = new PacketIn(IsLittleEndian);
            pIn.Serial = comPort.ReadSome(PacketIn.AdditionalSize);
            if (pIn.command != 1)
                return ("Не та команда в ответе");
            if (pIn.abonent != 2)
                return ("Не тот абонет в ответе");
            return (pIn.SState);
        }
        string TubeNum(int _command, out int _TubeNum)
        {
            _TubeNum = 0;
            if (!comPort.Write(new PacketOut(IsLittleEndian) { command = _command }.Serial))
                return ("Не смогли записать");
            PacketIn pIn = new PacketIn(IsLittleEndian);
            pIn.Serial = comPort.ReadSome(PacketIn.AdditionalSize + 9);
            if (pIn.State != Packet.EState.Ok)
                return (pIn.SState);
            if (pIn.command != _command)
                return ("Не та команда в ответе");
            if (pIn.abonent != 2)
                return ("Не тот абонет в ответе");
            _TubeNum = RByteConverter.ByteToInt(IsLittleEndian, pIn.data, 2, 3);
            return ("Ok");
        }
        public string TubeNumIn(out int _TubeNum)
        {
            return (TubeNum(2, out _TubeNum));
        }
        public string TubeNumOut(out int _TubeNum)
        {
            return (TubeNum(3, out _TubeNum));
        }
        public string SendResult(Result _Result)
        {
            if (_Result == null)
                return (SendResultTest());
            int size = 1024;
            byte[] data = new byte[size];
            RByteConverter.IntToByte(IsLittleEndian, _Result.IdTube, data, 0, 3);
            int x = 3;
            data[x++] = Convert.ToByte(Classer.ToIntACS(_Result.Sum.RClass));
            data[x++] = 0;
            data[x++] = Convert.ToByte(_Result.Sum.MClass.Count);
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = Convert.ToByte(Math.Round(ParAll.CTS.Cross.Border1));
            data[x++] = Convert.ToByte(Math.Round(ParAll.CTS.Cross.Border2));
            data[x++] = Convert.ToByte(Math.Round(ParAll.CTS.Line.Border1));
            data[x++] = Convert.ToByte(Math.Round(ParAll.CTS.Line.Border2));
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = Convert.ToByte(Math.Round(_Result.Thick.Border1 * 10));
            data[x++] = Convert.ToByte(Math.Round(_Result.Thick.Border2 * 10));
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = Convert.ToByte(SGToInt(_Result.SG.sgState.Group));
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = 0;
            for (int i = 0; i < 130; i++)
                data[x++] = (i >= _Result.Thick.MZone.Count) ? Convert.ToByte(128) : GetLevel(_Result.Thick.MZone[i].RClass, _Result.Thick.MZone[i].Level);
            for (int i = 0; i < 130; i++)
                data[x++] = (i >= _Result.Cross.MZone.Count) ? Convert.ToByte(128) : GetLevel(_Result.Cross.MZone[i].Class, _Result.Cross.MZone[i].GetMaxLevel());
            for (int i = 0; i < 130; i++)
                data[x++] = (i >= _Result.Line.MZone.Count) ? Convert.ToByte(128) : GetLevel(_Result.Line.MZone[i].Class, _Result.Line.MZone[i].GetMaxLevel());
            for (int i = 0; i < 130; i++)
                data[x++] = 128;
            Array.Resize(ref data, x);
            PacketOut pOut = new PacketOut(IsLittleEndian) { command = 5 };
            pOut.data = data;
            if (!comPort.Write(pOut.Serial))
                return ("Не смогли записать");
            return ("Ok");
        }
        public string SendResultTest()
        {
            int size = 1024;
            byte[] data = new byte[size];
            RByteConverter.IntToByte(IsLittleEndian, 123456, data, 0, 3);
            int x = 3;
            data[x++] = Convert.ToByte(Classer.ToIntACS(EClass.Class1));
            data[x++] = 0;
            data[x++] = Convert.ToByte(10);
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = Convert.ToByte(50);
            data[x++] = Convert.ToByte(60);
            data[x++] = Convert.ToByte(51);
            data[x++] = Convert.ToByte(61);
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = Convert.ToByte(75);
            data[x++] = Convert.ToByte(70);
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = Convert.ToByte(1);
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = 0;
            data[x++] = 0;
            for (int i = 0; i < 130; i++)
                data[x++] = (i >= 10) ? Convert.ToByte(128) : Convert.ToByte(80);
            for (int i = 0; i < 130; i++)
                data[x++] = (i >= 10) ? Convert.ToByte(128) : Convert.ToByte(15);
            for (int i = 0; i < 130; i++)
                data[x++] = (i >= 10) ? Convert.ToByte(128) : Convert.ToByte(17);
            for (int i = 0; i < 130; i++)
                data[x++] = 128;
            Array.Resize(ref data, x);
            PacketOut pOut = new PacketOut(IsLittleEndian) { command = 5 };
            pOut.data = data;
            if (!comPort.Write(pOut.Serial))
                return ("Не смогли записать");
            return ("Ok");
        }
        int SGToInt(string _sg)
        {
            if (_sg == "Д") return (1); // рус
            if (_sg == "К") return (2); // рус
            if (_sg == "Е") return (3); // рус
            if (_sg == "N80") return (4);
            if (_sg == "P") return (5); // P110
            if (_sg == "Q") return (6); // Q125
            if (_sg == "Л") return (7); // рус     /\
            if (_sg == "М") return (8); // рус
            if (_sg == "Р") return (9); // рус
            if (_sg == "J55") return (10); // J-55
            if (_sg == "K55") return (11); // K-55
            if (_sg == "C90") return (12);
            if (_sg == "T95") return (13);
            if (_sg == "H40") return (14);
            if (_sg == "L80") return (15);
            if (_sg == "C95") return (16);
            if (_sg == "M65") return (17);
            if (_sg == "NQ") return (18); // N80Q
            if (_sg == "К72") return (19);  // K72
            return (0);
        }
        byte GetLevel(EClass _rClass, double? _Level)
        {
            if (_rClass == EClass.Dead)
                return (128);
            if (_rClass == EClass.None || _Level == null)
                return (255);
            return (Convert.ToByte(Math.Round(_Level.Value * 10)));
        }
    }
}
