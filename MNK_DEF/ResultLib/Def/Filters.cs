using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using UPAR.Def;

namespace ResultLib.Def
{
    public class Filters
    {
        static public void Exec(FilterPars _pars, double _SampleRate, double[] _arr)
        {
            if (!_pars.IsFilter)
                return;
            try
            {
                switch (_pars.CurrentType)
                {
                    case 0:
                        Butterworth(
                            _arr,
                            _arr.Length,
                            _pars.Order,
                            _SampleRate,
                            _pars.CutoffFrequency,
                            _pars.CenterFrequency,
                            _pars.WidthFrequency,
                            _pars.CurrentSubType);
                        break;
                    case 1:
                        Chebyshev(
                            _arr,
                            _arr.Length,
                            _pars.Order,
                            _SampleRate,
                            _pars.CutoffFrequency,
                            _pars.CenterFrequency,
                            _pars.WidthFrequency,
                            _pars.RippleDb, //=======
                            _pars.CurrentSubType);
                        break;
                    case 2:
                        Elliptic(
                            _arr,
                            _arr.Length,
                            _pars.Order,
                            _SampleRate,
                            _pars.CutoffFrequency,
                            _pars.CenterFrequency,
                            _pars.WidthFrequency,
                            _pars.RippleDb, //--------
                            _pars.Rolloff, //------------
                            _pars.CurrentSubType);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не верные параметры фильтра: "+ex.Message);            
            }
        }
        [DllImport("filters.dll", EntryPoint = "Butterworth", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Butterworth(
            double[] _data,
            int _N,
            int _order,
            double _sampleRate,
            double _cutoffFrequency,
            double _centerFrequency,
            double _widthFrequency,
            int _CurrentSubType);

        [DllImport("filters.dll", EntryPoint = "ChebyshevI", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Chebyshev(
            double[] _data,
            int _N,
            int _order,
            double _sampleRate,
            double _cutoffFrequency,
            double _centerFrequency,
            double _widthFrequency,
            double _rippleDb,
            int _CurrentSubType);

        [DllImport("filters.dll", EntryPoint = "Elliptic", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void Elliptic(
            double[] _data,
            int _N,
            int _order,
            double _sampleRate,
            double _cutoffFrequency,
            double _centerFrequency,
            double _widthFrequency,
            double _rippleDb,
            double _rolloff,
            int _CurrentSubType);
    }
}
