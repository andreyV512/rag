using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalclSGPars
{
    public enum EValIU
    {
        U,
        I,
        UmI,
        ImU
    }
    public enum EStartPoint
    {
        UH,
        UL,
        IH,
        IL
    }

    public class IU
    {
        public double I = 0;
        public double U = 0;
        public IU() { }
        public IU(double _I, double _U)
        {
            I = _I;
            U = _U;
        }
        public IU(IU _iu)
        {
            I = _iu.I;
            U = _iu.U;
        }
        public double Val(EValIU _ValIU)
        {
            switch (_ValIU)
            {
                case EValIU.I:
                    return (I);
                case EValIU.ImU:
                    return (I - U);
                case EValIU.UmI:
                    return (U - I);
                default:
                    return (U);
            }
        }
        public static IU[] StrToIUfloat(string _data)
        {
            DateTime dt0 = DateTime.Now;
            if (_data == null)
                return (null);
            if (_data.Length == 0)
                return (null);
            string[] M = _data.Split(';');
            int packets = Convert.ToInt32(M[0]);
            IU[] iu = new IU[packets];
            for (int p = 0; p < packets; p++)
            {
                string[] mm = M[p + 1].Split(' ');
                iu[p] = new IU(Convert.ToDouble(mm[0]), Convert.ToDouble(mm[1]));
            }
            int ms = (DateTime.Now - dt0).Milliseconds;
            return (iu);
        }
        public static IU[] StrToIU(string _data)
        {
            DateTime dt0 = DateTime.Now;
            if (_data == null)
                return (null);
            if (_data.Length == 0)
                return (null);
            int pos = 0;
            int count = Convert.ToInt32(_data.Substring(pos, 8));
            pos += 9;
            IU[] iu = new IU[count];
            for (int i = 0; i < count; i++)
            {
                //                string aaa = _data.Substring(pos, 7).Replace(".",",");
                iu[i] = new IU();
                iu[i].I = Convert.ToDouble(_data.Substring(pos, 7).Replace(".", ","));
                pos += 8;
                iu[i].U = Convert.ToDouble(_data.Substring(pos, 7).Replace(".", ","));
                pos += 8;
            }
            DateTime dt1 = DateTime.Now;
            int ms = (dt1 - dt0).Milliseconds;
            return (iu);
        }
        static string IUToStr(IU[] iu)
        {
            int psize = 7 + 1 + 7 + 1;
            char[] cc = new char[iu.Length * psize];
            for (int i = 0; i < iu.Length; i++)
            {
                string s = (iu[i].I.ToString(" 000.00;-000.00") + " " + iu[i].U.ToString(" 000.00;-000.00") + ";").Replace(",", ".");
                for (int j = 0; j < psize; j++)
                    cc[i * psize + j] = s[j];
            }
            string data = iu.Length.ToString("00000000") + ";" + new string(cc, 0, cc.Length);
            return (data);
        }
        static public bool Check(IU _iu, EStartPoint _sp)
        {
            switch (_sp)
            {
                case EStartPoint.IH:
                    return (_iu.I > 0);
                case EStartPoint.IL:
                    return (_iu.I < 0);
                case EStartPoint.UH:
                    return (_iu.U > 0);
                case EStartPoint.UL:
                    return (_iu.U < 0);
            }
            return (false);
        }
    }
}
