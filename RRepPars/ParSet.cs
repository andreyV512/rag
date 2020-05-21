using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using ParamSettings;

namespace RRepPars
{
    [Serializable]
    public class ParSet
    {
        public ParSet()
        {
            LWinPars = new L_WinPars();
            Selection = new SelectionPars();
        }

        public L_WinPars LWinPars { get; set; }
        public SelectionPars Selection { get; set; }
    }
}
