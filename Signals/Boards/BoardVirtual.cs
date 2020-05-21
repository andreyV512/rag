using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UPAR_common;

namespace Signals.Boards
{
    public class BoardVirtual : Board
    {
        public BoardVirtual(PCIE1730pars _pars, DOnPr _OnPr) : base(_pars, _OnPr) { }
        public override int Read() { return (values_in); }
        public override int ReadOut() { return (values_out); }
        public override void Write(int _values_out) { }
        public override void WriteIn(int _values_in) { }
    }
}
