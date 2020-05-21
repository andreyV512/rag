using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Protocol
{
    public class ProtocolST
    {
        static FProtocol protocol = null;
        static object parent = null;
        public static Form Instance(object _parent, int _Period, bool _isFile = false, bool _isSave = false)
        {
            if (protocol == null)
            {
                protocol = new FProtocol(_Period, _isFile, _isSave);
                parent = _parent;
                return (protocol);
            }
            else
            {
                if (_parent != parent)
                    return (null);
                return (protocol);
            }
        }
        public static Form Instance(object _parent)
        {
            if (_parent != parent)
                return (null);
            return (protocol);
        }
        public static void pr(string _msg)
        {
            if (protocol == null)
                return;
            if (!protocol.Visible)
                return;
            protocol.pr(_msg);
        }
        public static void Show()
        {
            if (protocol == null)
                return;
            if (protocol.Visible)
                return;
            protocol.Show();
        }
        public static bool IsFile
        {
            get
            {
                return (protocol == null ? false : protocol.IsFile);
            }
        }
        public static bool IsSave
        {
            get
            {
                return (protocol == null ? false : protocol.IsSave);
            }
        }
    }
}
