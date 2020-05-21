using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using Protocol;

namespace Defect.SG
{
    public abstract class BaseItem 
    {
        protected void pr(string _msg)
        {
            ProtocolST.pr(ToString() + " " + _msg);
        }
        abstract public bool Update();
        abstract public bool Delete();
        
        [Browsable(false)]
        abstract public BaseDBKey Key { get; }
        
        protected static double SD(object _val)
        {
            return (Convert.ToDouble(_val.ToString()));
        }
        protected static Single DS(object _val)
        {
            return (Convert.ToSingle(_val.ToString()));
        }
        public static string Schema = null;
    }
    public class BaseDBKey
    {
    }
}
