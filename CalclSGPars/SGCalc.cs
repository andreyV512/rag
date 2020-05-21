using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using CalclSGPars;

public class SGCalc
{
    EStartPoint StartPoint;
    EValIU ValIU;
    IU[] iu;
    int periodMin;
    int periodMax;
    int Start = 0;
    int Stop = 0;
    bool FullPeriod;
    int win = 0;

    [SqlFunction()]
    static public int RGetTickCount()
    {
        return (Environment.TickCount);
    }

    [SqlFunction(FillRowMethodName = "ExecFill", TableDefinition = "par INT, val real, msg varchar")]
    static public IEnumerator Calc(
        SqlBoolean _pars,
        SqlString _img,
        SqlString _stresh,
        SqlInt32 _HalfPeriod,
        SqlInt32 _HalfPeriodDif,
        SqlBoolean _FullPeriod,
        SqlString _ByU,
        SqlString _ValIU,
        SqlInt32 _BorderStart,
        SqlInt32 _BorderStop,
        SqlInt32 _SOPLenght,
        SqlInt32 _SOPStart,
        SqlInt32 _SOPStop
        )
    {
        SGCalc Instance = new SGCalc();
        IEnumerator en = Instance.Calc0(
            _pars,
            _img,
            _stresh,
            _HalfPeriod,
            _HalfPeriodDif,
            _FullPeriod,
            _ByU,
            _ValIU,
            _BorderStart,
            _BorderStop,
            _SOPLenght,
            _SOPStart,
            _SOPStop
        );
        while (en.MoveNext())
        {
            yield return (en.Current);
        }
        yield break;
    }
    IEnumerator Calc0(
        SqlBoolean _pars,
        SqlString _img,
        SqlString _stresh,
        SqlInt32 _HalfPeriod,
        SqlInt32 _HalfPeriodDif,
        SqlBoolean _FullPeriod,
        SqlString _ByU,
        SqlString _ValIU,
        SqlInt32 _BorderStart,
        SqlInt32 _BorderStop,
        SqlInt32 _SOPLenght,
        SqlInt32 _SOPStart,
        SqlInt32 _SOPStop
        )
    {
        if (_img.IsNull)
            yield break;

        StartPoint = ParceEStartPoint(_ByU.Value);
        ValIU = ParceEValIU(_ValIU.Value);
        iu = IU.StrToIUfloat(_img.Value);
        periodMin = periodMin = _HalfPeriod.Value - _HalfPeriodDif.Value;
        if (periodMin < 0)
            periodMin = 0;
        periodMax = _HalfPeriod.Value + _HalfPeriodDif.Value;
        if (_SOPLenght.IsNull)
        {
            if (_BorderStart.IsNull)
                Start = 0;
            else
                Start = Convert.ToInt32(Math.Floor(Convert.ToDouble(iu.Length) * _BorderStart.Value / 100));
            if (_BorderStop.IsNull)
                Stop = iu.Length - 1;
            else
                Stop = iu.Length - 1 - Convert.ToInt32(Math.Floor(Convert.ToDouble(iu.Length) * _BorderStop.Value / 100));
        }
        else
        {
            double per_mm = iu.Length;
            per_mm /= _SOPLenght.Value;
            Start = Convert.ToInt32(Math.Ceiling(per_mm * (_SOPStart.IsNull ? 0 : _SOPStart.Value)));
            Stop = Convert.ToInt32(Math.Ceiling(per_mm * (_SOPStop.IsNull ? 0 : _SOPStop.Value)));
            if (Start < 0)
                Start = 0;
            if (Stop < Start)
                Stop = Start;
            if (Stop > iu.Length - 1)
                Stop = iu.Length - 1;
        }
        FullPeriod = _FullPeriod.Value;
        win = Convert.ToInt32(Math.Ceiling(_HalfPeriod.Value * 0.05));

        int[] tresh;
        int treshMax;
        if (_stresh.IsNull)
        {
            tresh = new int[0];
            treshMax = 0;
        }
        else
        {
            string[] mtresh = _stresh.Value.Split(';');
            tresh = new int[mtresh.Length];
            double k = _HalfPeriod.Value;
            k /= 100;
            treshMax = 0;
            for (int i = 0; i < mtresh.Length; i++)
            {
                tresh[i] = (int)(k * Convert.ToDouble(mtresh[i].Replace('.', ',')));
                if (treshMax < tresh[i])
                    treshMax = tresh[i];
            }
        }
        double[] coords = new double[tresh.Length];
        for (int i = 0; i < tresh.Length; i++)
            coords[i] = 0;
        List<SGHalfPeriod> Lsghp = new List<SGHalfPeriod>();

        int nn = 0;
        SGPeriod prev_period = new SGPeriod() { start = Start, size = 0 };
        for (; ; )
        {
            SGPeriod period = GetNextPeriod(prev_period);
            if (period == null)
                break;
            if (period.start + treshMax >= iu.Length)
                break;
            if (period.size > periodMax || period.size < periodMin)
            {
                prev_period = period;
                continue;
            }
            Lsghp.Add(new SGHalfPeriod(nn, period.start, period.size));
            for (int i = 0; i < coords.Length; i++)
                coords[i] += iu[period.start + tresh[i]].Val(ValIU);
            nn++;
            prev_period = period;
        }
        for (int i = 0; i < coords.Length; i++)
        {
            coords[i] = Math.Round(coords[i] / nn, 2);
            if (double.IsNaN(coords[i]))
                coords[i] = 0;
        }

        if (_pars.Value)
        {
            for (int i = 0; i < coords.Length; i++)
                yield return new Result(i, coords[i], "");
        }
        else
        {
            for (int i = 0; i < Lsghp.Count; i++)
                yield return new Result(Lsghp[i].start, Lsghp[i].size, "");
        }
    }
    EStartPoint ParceEStartPoint(string _s)
    {
        if (!Enum.IsDefined(typeof(EStartPoint), _s))
            return EStartPoint.IL;
        return (EStartPoint)Enum.Parse(typeof(EStartPoint), _s);
    }
    EValIU ParceEValIU(string _s)
    {
        if (!Enum.IsDefined(typeof(EValIU), _s))
            return EValIU.U;
        return (EValIU)Enum.Parse(typeof(EValIU), _s);
    }
    public class Result
    {
        public int par;
        public double val;
        public string msg;
        public Result(int _par, double _val, string _msg)
        {
            par = _par;
            val = _val;
            msg = _msg;
        }
    }
    static public void ExecFill(object _result, out SqlInt32 _par, out SqlSingle _val, out SqlString _msg)
    {
        Result r = _result as Result;

        _par = r.par;
        _val = Convert.ToSingle(r.val);
        _msg = r.msg;
    }
    bool Check(int _index)
    {
        return (Check(_index, StartPoint));
    }
    bool Check(int _index, EStartPoint _StartPoint)
    {
        return (IU.Check(iu[_index], _StartPoint));
    }
    bool CheckWin(int _index, EStartPoint _StartPoint)
    {
        int lstart = _index - win;
        int lstop = _index + win;
        if (lstart < Start || lstop > Stop)
            return (false);
        if (Check(lstart, _StartPoint))
            return (false);
        if (!Check(lstop, _StartPoint))
            return (false);
        return (true);
    }
    SGPeriod GetNextPeriod(SGPeriod _prev)
    {
        SGPeriod period = new SGPeriod(_prev);
        int start = FindPoint(period.start, true);
        if (start < 0)
            return (null);
        int middle = FindPoint(start, false);
        if (middle < 0)
            return (null);
        if (FullPeriod)
        {
            int stop = FindPoint(middle, true);
            if (stop < 0)
                return (null);
            period.start = start;
            period.size = stop - start;
        }
        else
        {
            period.start = start;
            period.size = middle - start;
        }
        return (period);
    }
    SGPeriod GetNextPeriodHalf(SGPeriod _prev)
    {
        SGPeriod period = new SGPeriod(_prev);
        int start = FindPoint(period.start, true);
        if (start < 0)
            return (null);
        int stop = FindPoint(start, false);
        if (stop < 0)
            return (null);
        period.start = start;
        period.size = stop - start;
        return (period);
    }
    int FindPoint(int _start, bool _positive)
    {
        if (_positive)
        {
            for (int i = _start; i <= Stop; i++)
            {
                if (!Check(i, StartPoint))
                    continue;
                if (!CheckWin(i, StartPoint))
                    continue;
                return (i);
            }
        }
        else
        {
            EStartPoint sp = Reverse(StartPoint);
            for (int i = _start; i <= Stop; i++)
            {
                IU iui = iu[i];
                if (!Check(i, sp))
                    continue;
                if (!CheckWin(i, sp))
                    continue;
                return (i);
            }
        }
        return (-1);
    }
    EStartPoint Reverse(EStartPoint _sp)
    {
        switch (_sp)
        {
            case EStartPoint.IH:
                return (EStartPoint.IL);
            case EStartPoint.IL:
                return (EStartPoint.IH);
            case EStartPoint.UH:
                return (EStartPoint.UL);
            case EStartPoint.UL:
                return (EStartPoint.UH);
        }
        return (EStartPoint.IH);
    }
}
