using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Defect.SG
{
    public delegate void DOnPrs(string _msg);
    public delegate void DOnCurrent(BaseDBKey _key);
    interface IDGV
    {
        void RLoad(BaseDBKey _Key);
        DOnPrs OnPrs { set; }
        event DOnCurrent OnCurrent;
        string Schema { get; set; }
        DataGridView DG { get; }
    }
}
