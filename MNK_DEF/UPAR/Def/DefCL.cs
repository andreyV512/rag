using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;

namespace UPAR.Def
{
    public class DefCL
    {
        public EUnit Tp { get; private set; }

        public DefCL(EUnit _Tp)
        {
            Tp = _Tp;

        }
        public double Border1
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.CTS.Cross.Border1);
                if (Tp == EUnit.Line)
                    return (ParAll.CTS.Line.Border1);
                return (0);
            }
            set
            {
                if (Tp == EUnit.Cross)
                    ParAll.CTS.Cross.Border1 = value;
                if (Tp == EUnit.Line)
                    ParAll.CTS.Line.Border1 = value;
            }
        }
        public double Border2
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.CTS.Cross.Border2);
                if (Tp == EUnit.Line)
                    return (ParAll.CTS.Line.Border2);
                return (0);
            }
            set
            {
                if (Tp == EUnit.Cross)
                    ParAll.CTS.Cross.Border2 = value;
                if (Tp == EUnit.Line)
                    ParAll.CTS.Line.Border2 = value;
            }
        }
        public double Border1In
        {
            get
            {
                if (Tp == EUnit.Line)
                    return (ParAll.CTS.Line.Border1In);
                return (0);
            }
            set
            {
                if (Tp == EUnit.Line)
                    ParAll.CTS.Line.Border1In = value;
            }
        }
        public double Border2In
        {
            get
            {
                if (Tp == EUnit.Line)
                    return (ParAll.CTS.Line.Border2In);
                return (0);
            }
            set
            {
                if (Tp == EUnit.Line)
                    ParAll.CTS.Line.Border2In = value;
            }
        }
        public L_L502Ch LCh
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.CTS.Cross.L502Chs);
                if (Tp == EUnit.Line)
                    return (ParAll.CTS.Line.L502Chs);
                return (null);
            }
        }
        public double[] Borders
        {
            get
            {
                double[] ret = new double[2];
                ret[0] = Border1;
                ret[1] = Border2;
                return (ret);
            }
        }
        public double[] BordersIn
        {
            get
            {
                double[] ret = new double[2];
                ret[0] = Border1;
                ret[1] = Border2;
                return (ret);
            }
        }
        public FilterPars Filter
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.ST.Defect.Cross.Filter);
                if (Tp == EUnit.Line)
                    return (ParAll.ST.Defect.Line.Filter);
                return (null);
            }
            set
            {
                if (Tp == EUnit.Cross)
                    ParAll.ST.Defect.Cross.Filter = value;
                if (Tp == EUnit.Line)
                    ParAll.ST.Defect.Line.Filter = value;
            }
        }
        public FilterPars FilterIn
        {
            get
            {
                if (Tp == EUnit.Line)
                    return (ParAll.ST.Defect.Line.FilterIn);
                return (new FilterPars());
            }
            set
            {
                if (Tp == EUnit.Line)
                    ParAll.ST.Defect.Line.FilterIn = value;
            }
        }
        public bool IsFinterIn
        {
            get
            {
                if (Tp == EUnit.Line)
                    return (ParAll.ST.Defect.Line.FilterIn.IsFilter);
                return (false);
            }
        }
        public int DeadZoneStart
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.CTS.Cross.DeadZoneStart);
                if (Tp == EUnit.Line)
                    return (ParAll.CTS.Line.DeadZoneStart);
                return (0);
            }
            set
            {
                if (Tp == EUnit.Cross)
                    ParAll.CTS.Cross.DeadZoneStart = value;
                if (Tp == EUnit.Line)
                    ParAll.CTS.Line.DeadZoneStart = value;
            }
        }
        public int DeadZoneFinish
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.CTS.Cross.DeadZoneFinish);
                if (Tp == EUnit.Line)
                    return (ParAll.CTS.Line.DeadZoneFinish);
                return (0);
            }
            set
            {
                if (Tp == EUnit.Cross)
                    ParAll.CTS.Cross.DeadZoneFinish = value;
                if (Tp == EUnit.Line)
                    ParAll.CTS.Line.DeadZoneFinish = value;
            }
        }
        public TailPars Tails
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.ST.Defect.Cross.Tails);
                if (Tp == EUnit.Line)
                    return (ParAll.ST.Defect.Line.Tails);
                return (null);
            }
        }
        public LCard502Pars L502
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.ST.Defect.Cross.L502);
                if (Tp == EUnit.Line)
                    return (ParAll.ST.Defect.Line.L502);
                return (null);
            }
        }
        public int Buffer
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.ST.Defect.Cross.Buffer);
                if (Tp == EUnit.Line)
                    return (ParAll.ST.Defect.Line.Buffer);
                return (0);
            }
        }
        public double MultEnd
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.ST.Defect.Cross.Tails.MultEnd);
                if (Tp == EUnit.Line)
                    return (ParAll.ST.Defect.Line.Tails.MultEnd);
                return (0);
            }
        }
        public int SensorsPosition
        {
            get
            {
                if (Tp == EUnit.Cross)
                    return (ParAll.ST.Dimensions.SensorsC);
                if (Tp == EUnit.Line)
                    return (ParAll.ST.Dimensions.SensorsL);
                return (0);
            }
        }

    }
}
