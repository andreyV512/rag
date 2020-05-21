using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UPAR_common;
using UPAR;
using Share;
using Protocol;

namespace BankLib
{
    public class BankSGUnit: BankDataUnit, ILoadSettings
    {
        public BankSGUnit()
            : base(EUnit.SG)
        {
            Clear();
        }

        public new void Clear()
        {
            LastData = false;
            base.Clear();
        }
        public void LoadSettings()
        {
            Sensors =  2;
            ReSize(ParAll.SG.Buffer * 1024 * 1024 / sizeof(double));
            Clear();
        }
        public BankZoneData GetZone()
        {
            if (!isStarted)
                return (null);
            if(!LastData)
                return (null);
            BankZoneData z = new BankZoneData(BankZone.EType.SG);
            z.idata = 0;
            z.index = 0;
            z.last = true;
            z.length = 0;
            z.size = Count;
            complete = true;
            return (z);
        }
        void pr(string _msg)
        {
            ProtocolST.pr(_msg);
        }
        public new void Add(double[] _data, int _offset, int _size)
        {
            if (!isStarted)
                return;
            if (LastData)
                return;
            int packets = _data.Length / _size;
            if (packets * _size != _data.Length)
                throw (new Exception(string.Format("Bank::AddGroup: Размер пакета не кратен количеству датчиков: Cross={0} size={1}", Sensors.ToString(), _data.Length.ToString())));
            base.Add(_data, _offset, _size);
        }
        public bool LastData { get; set; }
    }
}
