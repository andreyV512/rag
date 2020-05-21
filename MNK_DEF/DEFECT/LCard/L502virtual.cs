using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPAR;

namespace Defect.LCard
{
    public class L502virtual : ILCard502, IDisposable
    {
        public L502virtual() { }
        public void Dispose() { }
        public double GetValueP(double _ch) { return (0); }
        public string LastError { get; private set; }
        public void Start(LCard502Pars _pars, L502Ch[] _channels) { }
        public void Stop() { }
        public double[] Read() { return (null); }
        public double GetValueV(L502Ch _channel, LCard502Pars _parsLCard502Pars, ref bool _ret) { return (0); }
        public double GetValueP(L502Ch _channel, LCard502Pars _parsLCard502Pars, ref bool _ret) { return (0); }
        public double F_ACQ { get { return (0); } }
    }
}
