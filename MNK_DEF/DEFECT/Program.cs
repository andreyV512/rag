using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using UPAR;

namespace Defect
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
//            Application.Run(new FMain());
            ParAll.Create("ParametersCross", FMain.IsLocal());
            FMain f = new FMain();
            if (f.Login())
            {
                ParAll.ST.LoadDesc();
                Application.Run(f);
                ParAll.ST.Save();
            }
        }
    }
}
