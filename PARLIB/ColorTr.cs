using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PARLIB
{
    public static class ColorTr
    {
        public static Color From(string _s)
        {
            if (_s == null)
                return (Color.Black);
            try
            {
                return(ColorTranslator.FromWin32(Convert.ToInt32(_s)));
            }
            catch
            {
            }
            try
            {
                return(ColorTranslator.FromHtml(_s));
            }
            catch
            {
            }
            return (Color.Black);
        }
        public static string ToHtml(Color _c)
        {
            return(ColorTranslator.ToHtml(_c));
        }
        public static string ToWin32(Color _c)
        {
            return(ColorTranslator.ToWin32(_c).ToString());
        }
    }
}
