using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Signals;
using UPAR;
using PARLIB;
using Signals.Boards;
using System.ComponentModel;
using UPAR_common;

namespace Defect.Work
{
    public class SignalListDef : SignalListWork
    {
        [DName("Цепи управления"), SignalInput(true)]
        public Signal iCC;

        [DName("Цикл"), SignalInput(true)]
        public Signal iCYCLE;

        [DName("Готовность"), SignalInput(true)]
        public Signal iREADY;

        [DName("Контроль 1"), SignalInput(true)]
        public Signal iCONTROL1;

        [DName("Контроль 2"), SignalInput(true)]
        public Signal iCONTROL2;

        [DName("Контроль 3"), SignalInput(true)]
        public Signal iCONTROL3;

        [DName("Строб"), SignalInput(true)]
        public Signal iSTROBE;
        
        [DName("12В норма"), SignalInput(true)]
        public Signal i12V;

        [DName("Вход ГП"), SignalInput(true)]
        public Signal iSGIN;

        [DName("Выход ГП"), SignalInput(true)]
        public Signal iSGOUT;


        [DName("Работа 2"), SignalInput(false)]
        public Signal oWORK2;

        [DName("Работа 3"), SignalInput(false)]
        public Signal oWORK3;

        [DName("Общ. рез."), SignalInput(false)]
        public Signal oRESULT_COMMON;

        [DName("Строб вых."), SignalInput(false)]
        public Signal oSTROBE;

        [DName("Результат"), SignalInput(false)]
        public Signal oRESULT;

        SaveInput saveInput = null;

        public SignalListDef(PCIE1730pars _pars, SignalsPanelPars _SignalsPanel, Board.DOnPr _OnPr, bool _PCSide = true)
            : base(_pars, null, null, _SignalsPanel, _OnPr, true)
        {
            saveInput = SaveInput.Create(ParAll.ST.Defect.PCIE1730.Save1730);
        }
        public SignalListDef(PCIE1730pars _pars, ScrollableControl _Container, SignalsPanelPars _SignalsPanel, Board.DOnPr _OnPr, bool _PCSide = true)
            : base(_pars, null, null, _Container, _SignalsPanel, _OnPr, true)
        {
            saveInput = SaveInput.Create(ParAll.ST.Defect.PCIE1730.Save1730);
        }
        protected override void DoRun0()
        {
            if (saveInput != null)
            {
                if (iCYCLE.Val)
                    saveInput.Add(boards[0].ValuesIn, boards[0].ValuesOut);
            }
        }
        public new void Dispose()
        {
            if (saveInput != null)
                saveInput.Dispose();
            base.Dispose();
        }
    }
}
