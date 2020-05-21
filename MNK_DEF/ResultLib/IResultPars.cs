using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ResultLib
{
    public interface IResultPars
    {
        void SaveSettings();
        void Serialize(FileStream _stream);
    }
}
