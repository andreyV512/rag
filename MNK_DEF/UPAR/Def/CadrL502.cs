using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;

namespace UPAR.Def
{
    public class CadrL502
    {
        public class RItem
        {
            public int index { get; private set; }
            public int size { get; private set; }
            public EUnit Tp { get; private set; }

            public RItem(int _index, int _size, EUnit _Tp)
            {
                index = _index;
                size = _size;
                Tp = _Tp;
            }
        }
        List<RItem> L;
        public CadrL502()
        {
            L = new List<RItem>();
            if (ParAll.ST.Defect.Cross.IsWork)
            {
                L.Add(new RItem(0, ParAll.CTS.Cross.L502Chs.Count, EUnit.Cross));
                if (ParAll.SG.IsWork)
                    L.Add(new RItem(ParAll.CTS.Cross.L502Chs.Count, 2, EUnit.SG));
            }
            else
            {
                if (ParAll.SG.IsWork)
                    L.Add(new RItem(0, 2, EUnit.SG));
            }
            if (ParAll.ST.Defect.Line.IsWork)
                L.Add(new RItem(0, ParAll.CTS.Line.L502Chs.Count, EUnit.Cross));
        }
        public RItem this[EUnit _Tp]
        {
            get
            {
                foreach (RItem it in L)
                {
                    if (it.Tp == _Tp)
                        return (it);
                }
                return (null);
            }
        }
        int SizeTp(EUnit _Tp)
        {
            RItem it = this[_Tp];
            return (it == null ? 0 : it.size);
        }
        public int Size(EUnit _Tp)
        {
            if (_Tp == EUnit.Cross)
                return(this[EUnit.Cross].size+this[EUnit.SG].size);
            if(_Tp == EUnit.Line)
                return(this[EUnit.Line].size);
            return (0);
        }
    }
}
