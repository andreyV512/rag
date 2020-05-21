using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defect.GSPF052PCI
{
    interface IGSPF052: IDisposable
    {
//        string Start();
        string Stop();
    }
}
