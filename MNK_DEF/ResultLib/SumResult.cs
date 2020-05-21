using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UPAR;
using ResultLib.Def;
using ResultLib.Thick;
using Share;

namespace ResultLib
{
    public class SumResult
    {
        public List<EClass> MClass = new List<EClass>();
        //public void ComputeLine(ResultDef _line)
        //{
        //    MClass.Clear();
        //    MClass.Capacity = _line.MZone.Count();
        //    for (int i = 0; i < _line.MZone.Count(); i++)
        //        MClass.Add(_line.MZone[i].Class);
        //}

        public void Compute(ResultDef _cross, ResultDef _line, ResultThickLite _thick)
        {
            int size = 1000;
            if (_cross != null && _cross.MZone.Count > 0)
            {
                if (size > _cross.MZone.Count)
                    size = _cross.MZone.Count;
            }

            if (_line != null && _line.MZone.Count > 0)
            {
                if (size > _line.MZone.Count)
                    size = _line.MZone.Count;
            }

            if (_thick != null && _thick.MZone.Count > 0)
            {
                if (size > _thick.MZone.Count)
                    size = _thick.MZone.Count;
            }
            if (size == 1000)
                size = 0;
            MClass.Clear();
            MClass.Capacity = size;
            for (int i = 0; i < size; i++)
            {
                EClass c = EClass.None;
                if (_cross != null && i < _cross.MZone.Count())
                    c = Classer.Compare(c, _cross.MZone[i].Class);
                if (_line != null && i < _line.MZone.Count())
                    c = Classer.Compare(c, _line.MZone[i].Class);
                if (_thick != null && i < _thick.MZone.Count)
                    c = Classer.Compare(c, _thick.MZone[i].RClass);
                MClass.Add(c);
            }
            RClass=MaxGood != null ? EClass.Class1 : EClass.Brak;
        }
        public class PP
        {
            public int index = 0;
            public int count = 0;
        };
        public PP MaxGood
        {
            get
            {
                if (MClass.Count == 0)
                    return (null);
                int[] M = new int[MClass.Count];
                int L = 0;
                for (int i = 0; i < MClass.Count; i++)
                {
                    if (MClass[i] != EClass.Brak)
                        L++;
                    else
                        L = 0;
                    M[i] = L;
                }
                PP p = new PP();
                for (int i = 0; i < M.Length; i++)
                {
                    if (p.count < M[i])
                    {
                        p.count = M[i];
                        p.index = i;
                    }
                }
                if (p.count == 0)
                    return (null);
                PP pp = new PP() { index = p.index - p.count + 1, count = p.count };
                double diz = ParAll.CTS.MinGoodLength;
                diz /= ParAll.ST.ZoneSize;
                int iz = Convert.ToInt32(Math.Ceiling(diz));
                if (iz > pp.count)
                    return (null);
                return (pp);
            }
        }
        public EClass RClass=EClass.None;
    }
}

