using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UPAR.SG;
using System.IO;

namespace Defect.SG
{
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
        public double Val(SGPars.EValIU _ValIU)
        {
            switch (_ValIU)
            {
                case SGPars.EValIU.I:
                    return (I);
                case SGPars.EValIU.ImU:
                    return (I - U);
                case SGPars.EValIU.UmI:
                    return (U - I);
                default:
                    return (U);
            }
        }
    }
    public class MIU
    {
        IU[] m_iu = new IU[0];

        public MIU(bool _fromFile, string _s)
        {
            if (_fromFile)
                LoadFile(_s);
            else
                LoadString(_s);
        }
        public MIU(int _size, double[] _data)
        {
            int packets = _size / 2;
            m_iu = new IU[packets];
            for (int p = 0; p < packets; p++)
                m_iu[p] = new IU() { I = _data[p * 2], U = _data[p * 2 + 1] };
        }
        void LoadString(string _data)
        {
            if (_data == null)
                return;
            if (_data.Length == 0)
                return;
            string[] M = _data.Split(';');
            int packets = Convert.ToInt32(M[0]);
            m_iu = new IU[packets];
            for (int p = 0; p < packets; p++)
            {
                string[] mm = M[p + 1].Split(' ');
                m_iu[p] = new IU(Convert.ToDouble(mm[0]), Convert.ToDouble(mm[1]));
            }
        }
        public string Img
        {
            get
            {
                if (m_iu.Length == 0)
                    return (null);
                StringBuilder sb = new StringBuilder();
                sb.Append(m_iu.Length.ToString() + ";");
                for (int p = 0; p < m_iu.Length; p++)
                    sb.Append(m_iu[p].I.ToString("F2") + " " + m_iu[p].U.ToString("F2") + ";");
                return (sb.ToString());
            }
        }
        public void ZeroI()
        {
            if (m_iu.Length == 0)
                return;
            double sm = 0;
            foreach (IU p in m_iu)
                sm += p.I;
            sm /= m_iu.Length;
            foreach (IU p in m_iu)
                p.I -= sm;
        }
        public bool SaveFile(string _fname)
        {
            if (_fname == null || m_iu.Length == 0)
                return (false);
            StreamWriter file = new StreamWriter(_fname, false, Encoding.Default);
            file.WriteLine(m_iu.Length.ToString());
            file.WriteLine();
            foreach (IU p in m_iu)
                file.WriteLine(p.I.ToString("F3") + " " + p.U.ToString("F3"));
            file.Close();
            return (true);
        }
        bool LoadFile(string _fname)
        {
            using (StreamReader file = new StreamReader(_fname, Encoding.Default))
            {
                string line;
                line = file.ReadLine();
                if (line == null)
                    return (false);
                int count = Convert.ToInt32(line);
                m_iu = new IU[count];
                line = file.ReadLine();
                for (int i = 0; i < count; i++)
                {
                    line = file.ReadLine();
                    if (line == null)
                        return (false);
                    string[] mm = line.Split(' ');
                    m_iu[i] = new IU() { I = Convert.ToDouble(mm[0]), U = Convert.ToDouble(mm[1]) };
                }
                file.Close();
            }
            return (true);
        }
        public bool Ok { get { return (m_iu.Length > 0); } }
        public int Length { get { return (m_iu.Length); } }
        public IU this[int _index] { get { return (m_iu[_index]); } }
    }
}
