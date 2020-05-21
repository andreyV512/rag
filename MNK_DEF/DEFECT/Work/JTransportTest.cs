using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Share;
using UPAR;
using BankLib;
using SQL;

namespace Defect.Work
{
    class JTransportTest : IJob
    {
        bool complete = false;
        Bank bank;
        int testCount = 0;
        int StartTick = 0;
        public string LastError { get; private set; }
        public bool IsError { get { return (LastError != null); } }
        int tp_index = 0;

        public JTransportTest(Bank _bank)
        {
            bank = _bank;
            new ExecSQL("delete from TickPositions");
            new ExecSQL("update ThickWork set TubeLength = null");
        }
        public void Exec(int _tick)
        {
            if (complete)
                return;
            if (StartTick == 0)
                return;
            double zoneperiod = ParAll.ST.ZoneSize * 1000;
            zoneperiod /= ParAll.ST.Some.TestTubeSpeed;
            for (; ; testCount++)
            {
                int tick = StartTick + Convert.ToInt32(Math.Floor(testCount * zoneperiod));
                if (tick > _tick)
                    break;
                int position = testCount * ParAll.ST.ZoneSize;
                if (position >= ParAll.ST.Some.TestTubeLength)
                    bank.TubeLength = ParAll.ST.Some.TestTubeLength;
                bank.AddTickPosition(new TickPosition(tick, position));
                ExecSQL E=new ExecSQL(string.Format("insert into TickPositions values({0},{1},{2})",
                tp_index++.ToString(),
                tick.ToString(),
                position.ToString()));
                if (E.RowsAffected != 1)
                    throw new Exception("JTransportTest:Exec: Не могу записать в базу временную метку");
            }
        }
        public bool IsComplete
        {
            get
            {
                return (complete);
            }
        }
        public void Finish()
        {
            complete = true;
        }
        public DOnStatus OnStatus { get; set; }
        public void Start(int _tick)
        {
            StartTick = _tick;
            complete = false;
            testCount = 0;
        }
    }
}
