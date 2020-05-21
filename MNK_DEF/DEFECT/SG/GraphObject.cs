using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;

using UPAR.SG;

namespace Defect.SG
{
    public abstract class GraphObject : BaseItem
    {
        public GraphObject(string _TSName) { TSName = _TSName; }
        string TSName;

        [Browsable(false)]
        public string TypeSizeName { get { return (TSName); } }
        abstract public void UpdateSOP(SOPPars _sop);
        abstract public bool CalcPars(SOPPars _sop);
        abstract public SGHalfPeriod[] GetHalfPeriods(SOPPars _sop);
        protected abstract string GetImg();
        protected abstract bool SetImg(string _data);
        public bool LoadObjectFile(string _fname)
        {
            return (SetImg(new MIU(true, _fname).Img));
        }
        public bool SaveObjectFile(string _fname)
        {
            if (!GetMIU().Ok)
                return (false);
            return (GetMIU().SaveFile(_fname));
        }
        public MIU GetMIU()
        {
            return (new MIU(false, GetImg()));
        }
    }
}
