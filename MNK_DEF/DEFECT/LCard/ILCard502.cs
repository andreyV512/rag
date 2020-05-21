using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPAR;

namespace Defect.LCard
{
    public interface ILCard502
    {
        string LastError { get; }
        void Dispose();
        void Start(LCard502Pars _parsLCard502Pars, L502Ch[] _channels);
        void Stop();
        double[] Read();
        double GetValueV(L502Ch _channel, LCard502Pars _parsLCard502Pars, ref bool _ret);
        double GetValueP(double _val);
        double GetValueP(L502Ch _channel, LCard502Pars _parsLCard502Pars, ref bool _ret);
        double F_ACQ { get; }
    }
}
