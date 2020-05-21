using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UPAR;
using UPAR.Def;
using UPAR.TS;
using UPAR.TS.TSDef;
using Share;


namespace ResultLib.Def
{
    public class Sensor
    {
        public EUnit Tp { get; private set; }
        public Meas[] MMeas = null;
        public EClass Class = EClass.None;
//        public double Level { get; private set; }

        public Sensor(EUnit _Tp, int _size)
        {
            Tp = _Tp;
            MMeas = new Meas[_size];
            for (int im = 0; im < MMeas.Length; im++)
                MMeas[im] = new Meas();
        }

        public void Load(BinaryReader _br, int _version)
        {
            for (int im = 0; im < MMeas.Length; im++)
            {
                double aaa = _br.ReadDouble();
                MMeas[im].Source = aaa;
            }
        }
        public void Save(BinaryWriter _bw)
        {
            _bw.Write(Classer.ToDouble(Class));
            _bw.Write(MMeas.Length);
            foreach (Meas m in MMeas)
                _bw.Write(m.Source);
        }
        public void Calc(int _sensor, Sensor _prev)
        {
            CalcMedianFilterPrev(_prev == null ? null : _prev.MMeas);
            double lGain = new DefCL(Tp).LCh[_sensor].Gain;
            CalcClassGain(lGain);
        }
        void CalcMedianFilterPrev(Meas[] _prev)
        {
            int width = ParAll.ST.Defect.Some.WidthMedianFilter;
            if (!ParAll.ST.Defect.Some.IsMedianFilter)
                width = 0;
            Medianfilter(width);
            // TODO: ParAll.ST.Defect.Line.FilterIn null or empty?
            if (Tp == EUnit.Line)
                ComputeFilterPrev(ParAll.ST.Defect.Line.Filter, ParAll.ST.Defect.Line.FilterIn, ParAll.ST.Defect.Line.L502.FrequencyPerChannel, _prev);
            else
                ComputeFilterPrev(ParAll.ST.Defect.Cross.Filter, null, ParAll.ST.Defect.Cross.L502.FrequencyPerChannel, _prev);
        }
        public void CalcClassGain(double _gain)
        {
            TypeSize ts = ParAll.ST.TSSet.Current;
            if (Tp == EUnit.Line)
            {
                TSLine tsl = ParAll.CTS.Line;
                Class = EClass.None;
                foreach (Meas m in MMeas)
                {
                    m.Class = Classer.GetDefClass(m.FilterABC * _gain, tsl.Borders, m.Dead);
                    m.ClassIn = Classer.GetDefClass(m.FilterInABC * _gain, tsl.BordersIn, m.Dead);
                    Class = Classer.Compare(Class, m.Class);
                }
            }
            else
            {
                TSCross tsc = ParAll.CTS.Cross;
                Class = EClass.None;
                foreach (Meas m in MMeas)
                {
                    m.Class = Classer.GetDefClass(m.FilterABC * _gain, tsc.Borders, m.Dead);
                    Class = Classer.Compare(Class, m.Class);
                }
            }
        }
        void Medianfilter(int _Width)
        {
            int half = (_Width - 1) / 2;
            if (half < 1 || MMeas.Count() <= half)
            {
                foreach (Meas m in MMeas)
                    m.Median = m.Source;
                return;
            }
            int size_e = MMeas.Count() + half * 2;
            double[] extension = new double[size_e];
            for (int m = 0; m < MMeas.Count(); m++)
                extension[half + m] = MMeas[m].Source;
            for (int i = 0; i < half; i++)
            {
                extension[i] = MMeas[i].Source;
                extension[size_e - half + i] = MMeas[MMeas.Count() - half + i].Source;
            }
            _medianfilter(_Width, extension, half);
        }
        void _medianfilter(int _Width, double[] _extension, int _half)
        {
            double[] window = new double[_Width];
            for (int i = 0; i < MMeas.Count(); i++)
            {
                for (int j = 0; j < _Width; j++)
                    window[j] = _extension[i + j];
                bubbleSort(window);
                MMeas[i].Median = window[_half];
            }
        }
        void bubbleSort(double[] _arr)
        {
            double tmp;
            for (int i = 0; i < _arr.Length - 1; ++i) // i - номер прохода
            {
                for (int j = 0; j < _arr.Length - 1; ++j) // внутренний цикл прохода
                {
                    if (_arr[j + 1] < _arr[j])
                    {
                        tmp = _arr[j + 1];
                        _arr[j + 1] = _arr[j];
                        _arr[j] = tmp;
                    }
                }
            }
        }
        public double GetMaxLevel()
        {
            double Level = -1;
            foreach (Meas m in MMeas)
            {
                if (Level < m.FilterABC)
                    Level = m.FilterABC;
            }
            return (Level);
        }

        void ComputeFilterPrev(FilterPars _fpars, FilterPars _fparsIn, double _SampleRate, Meas[] _prev)
        {
            double[] M;
            double[] MIn = null;
            if (_prev == null)
            {
                M = new double[MMeas.Length];
                for (int i = 0; i < MMeas.Length; i++)
                    M[i] = MMeas[i].Median;
            }
            else
            {
                M = new double[_prev.Length + MMeas.Length];
                int j = 0;
                for (int i = 0; i < _prev.Length; i++)
                    M[j++] = _prev[i].Median;
                for (int i = 0; i < MMeas.Length; i++)
                    M[j++] = MMeas[i].Median;
            }
            if (_fparsIn != null)
                MIn = M.Clone() as double[];
            Filters.Exec(_fpars, _SampleRate, M);
            if (_fparsIn != null)
                Filters.Exec(_fparsIn, _SampleRate, MIn);
            if (_prev == null)
            {
                for (int i = 0; i < MMeas.Length; i++)
                    MMeas[i].Filter = M[i];
                if (_fparsIn != null)
                {
                    for (int i = 0; i < MMeas.Length; i++)
                        MMeas[i].FilterIn = MIn[i];
                }
            }
            else
            {
                int j = _prev.Length;
                for (int i = 0; i < MMeas.Length; i++)
                    MMeas[i].Filter = M[j++];
                if (_fparsIn != null)
                {
                    j = _prev.Length;
                    for (int i = 0; i < MMeas.Length; i++)
                        MMeas[i].FilterIn = MIn[j++];
                }
            }
        }
    }
}
