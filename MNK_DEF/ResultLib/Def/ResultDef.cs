using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using UPAR;
using UPAR.TS;
using UPAR.Def;
using UPAR.TS.TSDef;
using UPAR_common;
//using SQL;
using Share;
using Protocol;
using BankLib;

namespace ResultLib.Def
{
    public class ResultDef
    {
        public EUnit Tp { get; private set; }
        public List<Zone> MZone = new List<Zone>();
        System.Globalization.NumberFormatInfo NFI = new System.Globalization.NumberFormatInfo();

        public ResultDef(EUnit _Tp)
        {
            Tp = _Tp;
            NFI.NumberDecimalSeparator = ".";
        }
        public void Load(BinaryReader _br, int _version)
        {
            MZone.Clear();
            MZone.Capacity = _br.ReadInt16();
            int lsen = _br.ReadInt16();
            L_L502Ch lch = new DefCL(Tp).LCh;
            if (lch.Count != lsen)
                throw (new Exception("Количество датчиков в настройках не соответствует количеству датчиков в файле данных"));
            for (int iz = 0; iz < MZone.Capacity; iz++)
            {
                Zone z = new Zone(Tp);
                z.Load(_br, lsen, _version);
                MZone.Add(z);
            }
        }
        public void Save(BinaryWriter _bw)
        {
            _bw.Write(Convert.ToInt16(MZone.Count));
            _bw.Write(Convert.ToInt16(new DefCL(Tp).LCh.Count));
            foreach (Zone z in MZone)
                z.Save(_bw);
        }
        protected void CalcXStart(bool _dead)
        {
            for (int z = 0; z < MZone.Count(); z++)
            {
                if (!CalcXStart(z, _dead))
                    break;
            }
        }
        protected bool CalcXStart(int _zone, bool _dead)
        {
            DefCL dcl = new DefCL(Tp);
            int lLength;
            if (_dead)
                lLength = dcl.DeadZoneFinish;
            else
                lLength = dcl.Tails.LenghtEnd;
            if (lLength == 0)
                return (false);
            if (!_dead && dcl.Tails.MultStart == 1)
                return (false);
            Zone Z = MZone[_zone];

            int pos_start = _zone * ParAll.ST.ZoneSize;
            int pos_end = pos_start + ParAll.ST.ZoneSize;
            if (pos_start > lLength)
                return (false);
            if (pos_end <= lLength)
            {
                for (int s = 0; s < Z.MSensor.Length; s++)
                {
                    Sensor S = Z.MSensor[s];
                    for (int m = 0; m < S.MMeas.Count(); m++)
                    {
                        if (_dead)
                            S.MMeas[m].Dead = true;
                        else
                            S.MMeas[m].Source *= dcl.Tails.MultStart;
                    }
                }
                return (true);
            }
            for (int s = 0; s < Z.MSensor.Count(); s++)
            {
                Sensor S = Z.MSensor[s];
                double delta = lLength - pos_start;
                double K = delta / ParAll.ST.ZoneSize;
                double X = K * S.MMeas.Count();
                int x = (int)X;
                for (int m = 0; m < x; m++)
                {
                    if (_dead)
                        S.MMeas[m].Dead = true;
                    else
                        S.MMeas[m].Source *= dcl.Tails.MultStart;
                }
            }
            return (true);
        }
        void CalcXEnd(bool _dead)
        {
            DefCL dcl = new DefCL(Tp);
            int lLength;
            if (_dead)
                lLength = dcl.DeadZoneFinish;
            else
                lLength = dcl.Tails.LenghtEnd;
            if (lLength == 0)
                return;
            if (!_dead && dcl.Tails.MultStart == 1)
                return;

            int pos_start = 0;
            for (int z = MZone.Count() - 1; z >= 0; z--)
            {
                Zone Z = MZone[z];
                int pos_end = pos_start + Z.VZoneLength;
                if (pos_end <= lLength)
                {
                    for (int s = 0; s < Z.MSensor.Count(); s++)
                    {
                        Sensor S = Z.MSensor[s];
                        for (int m = 0; m < S.MMeas.Count(); m++)
                        {
                            if (_dead)
                                S.MMeas[m].Dead = true;
                            else
                                S.MMeas[m].Source *= dcl.Tails.MultStart;
                        }
                    }
                }
                else
                {
                    double delta = lLength - pos_start;
                    double K = delta / Z.VZoneLength;
                    for (int s = 0; s < Z.MSensor.Count(); s++)
                    {
                        Sensor S = Z.MSensor[s];
                        double X = K * S.MMeas.Count();
                        int x = S.MMeas.Count() - (int)X;
                        for (int m = S.MMeas.Count() - 1; m >= x; m--)
                        {
                            if (_dead)
                                S.MMeas[m].Dead = true;
                            else
                                S.MMeas[m].Source *= dcl.Tails.MultStart;
                        }
                    }
                    break;
                }
                pos_start += Z.VZoneLength;
            }
        }

        public void Compute()
        {
            CalcXStart(false);
            CalcXEnd(false);
            CalcXStart(true);
            CalcXEnd(true);
            ComputeZero();
        }
        public void ComputeZero()
        {
            for (int iz = 0; iz < MZone.Count; iz++)
                MZone[iz].Calc(iz == 0 ? null : MZone[iz - 1]);
        }
        public double MMByMeas(int _zone, int _sensor, int _meas)
        {
            double ret = 0;
            for (int iZ = 0; iZ < _zone; iZ++)
                ret += MZone[iZ].VZoneLength;
            double v = _meas;
            v *= ParAll.ST.ZoneSize;
            v /= MZone[_zone].MSensor[_sensor].MMeas.Length;
            ret += v;
            return (ret);
        }
        string sparb(string _name, string _v, bool _comma)
        {
            return (_name + "=" + _v + (_comma ? "," : null));
        }
        string spar(string _name, string _v, bool _comma)
        {
            return (sparb(_name, "'" + _v + "'", _comma));
        }
        string spar(string _name, int _v, bool _comma)
        {
            return (sparb(_name, _v.ToString(), _comma));
        }
        string spar(string _name, double _v, bool _comma)
        {
            return (sparb(_name, _v.ToString("F2").Replace(",", "."), _comma));
        }
        //public void SaveToDB(double? _revolutions = null)
        //{
        //    if (tubePars == null)
        //        return;
        //    if (tubePars.Local == true)
        //        return;
        //    string fieldPath = null;
        //    string fieldtresh = null;
        //    string tableResult = null;
        //    switch (Share.Current.UnitType)
        //    {
        //        case EUnit.Cross:
        //            fieldPath = "pathFileNameCross";
        //            fieldtresh = "thresholdC";
        //            tableResult = "resultCross";
        //            break;
        //        case EUnit.Line:
        //            fieldPath = "pathFileNameLong";
        //            fieldtresh = "thresholdL";
        //            tableResult = "resultLong";
        //            break;
        //    }

        //    string head = "update resultTubeShort set ";

        //    string where = string.Format(" where numFusion={0} and numTube={1}",
        //        tubePars.iFusion.ToString(),
        //        tubePars.iTube.ToString());

        //    string SQL = head;
        //    SQL += spar(fieldPath, FileName, true);
        //    SQL += spar("countZones", MZone.Count, true);

        //    DefCL dcl = new DefCL(Tp);
        //    SQL += spar(fieldtresh + "1", dcl.Border1, true);
        //    SQL += spar(fieldtresh + "2", dcl.Border2, false);

        //    SQL += where;
        //    ExecSQL E = new ExecSQL(SQL);
        //    if (E.RowsAffected != 1)
        //        throw new Exception("ResultDef.SaveToDB: Не смогли записать: " + SQL);

        //    head = string.Format("update {0} set ", tableResult);
        //    where = string.Format(" where numFusion={0} and numTube={1}",
        //        tubePars.iFusion.ToString(),
        //        tubePars.iTube.ToString());
        //    if (MZone.Count == 0)
        //        return;
        //    if (MZone[0].MSensor.Length == 0)
        //        return;
        //    int lsensors = MZone[0].MSensor.Length;
        //    L_L502Ch lch = dcl.LCh;
        //    for (int iS = 0; iS < lsensors; iS++)
        //    {
        //        SQL = head;
        //        for (int i = 0; i < MZone.Count; i++)
        //            //                    SQL += spar("Z" + (i + 1).ToString(), Classer.ToInt(MZone[i].MSensor[iS].Class), i != MZone.Count - 1);
        //            SQL += spar("Z" + (i + 1).ToString(), MZone[i].MSensor[iS].Level * lch[iS].Gain, i != MZone.Count - 1);
        //        SQL += where;
        //        SQL += " and sensorNum=" + (iS + 1).ToString();
        //        pr(SQL);
        //        E = new ExecSQL(SQL);
        //        if (E.RowsAffected != 1)
        //            throw new Exception("ResultDef.SaveToDB: Не смогли записать: " + SQL);
        //    }
        //    RDPars1 rt = new RDPars1(ParAll.ST.TSSet.Current, Tp);
        //    rt.Revolutions = _revolutions;
        //    rt.SaveToDB(tubePars);

        //    using (ExecSQL EE = new ExecSQL("update flags set isDataSendCompleet='true'"))
        //    {
        //        if (E.RowsAffected != 1)
        //            throw new Exception("ResultDef.SaveToDB: Не смогли записать во flags: " + SQL);
        //    }

        //}

        void pr(string _msg)
        {
            ProtocolST.pr("ResultDef: " + _msg);
        }
        public string FileName
        {
            get
            {
                return (null);
                //if (tubePars.Local)
                //{
                //    DateTime dt = DateTime.Now;
                //    return (string.Format("{0}\\Tube{1}{2}{3}_{4}{5}{6}.bindkb2",
                //        ParAll.ST.Defect.Some.SaveFile.Path,
                //        dt.Year.ToString().Substring(2),
                //        dt.Month.ToString("00"),
                //        dt.Day.ToString("00"),
                //        dt.Hour.ToString("00"),
                //        dt.Minute.ToString("00"),
                //        dt.Second.ToString("00")));
                //}
                //else
                //{
                //    return (string.Format("{0}\\Tube_{1}_{2}.bindkb2",
                //    ParAll.ST.Defect.Some.SaveFile.Path,
                //    tubePars.iFusion.ToString(),
                //    tubePars.iTube.ToString()));
                //}
            }
        }
        //string SQLZoneList(int _iis, double _gain)
        //{
        //    string ret = null;
        //    for (int iz = 0; iz < MZone.Count; iz++)
        //    {
        //        ret += string.Format("{0}Z{1}={2}",
        //            iz == 0 ? "" : ",",
        //            (iz + 1).ToString(),
        //            Math.Round(MZone[iz].MSensor[_iis].Level * _gain, 5).ToString(NFI));
        //    }
        //    return (ret);
        //}
        public void SaveBINDKB2_Msg()
        {
            SaveFilePars spars = ParAll.ST.Defect.Some.SaveFile;
            RemoveFiles.RemoveMsg(spars, "bindkb2");
            if (spars.Path == null || spars.Path.Length == 0)
                return;
            try
            {
                SaveBINDKB2(FileName);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Не могу записать файл: " + FileName + " " + e.Message);
            }
        }
        void SaveBINDKB2()
        {
            if (ParAll.ST.Defect.Some.SaveFile.Path == null || ParAll.ST.Defect.Some.SaveFile.Path.Length == 0)
                return;
            SaveBINDKB2(FileName);
        }
        public void SaveBINDKB2(string _fname)
        {
            if (_fname == null)
                return;
            //using (FileStream s = new FileStream(Path.ChangeExtension(_fname, ".xml"), FileMode.Create))
            //    Pars.Serialize(s);
            using (FileStream fs = new FileStream(_fname, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs, Encoding.GetEncoding(1251)))
                    Save(bw);
            }
        }
        public void LoadBINDKB2(string _fname)
        {
            //string fname_pars = Path.ChangeExtension(_fname, ".xml");
            //if (File.Exists(fname_pars))
            //{
            //    using (FileStream s = new FileStream(fname_pars, FileMode.Open))
            //        Pars = Result.DeSerialize(typeof(RDPars), s) as RDPars;
            //}
            using (FileStream fs = new FileStream(_fname, FileMode.Open))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.GetEncoding(1251)))
                    Load(br, 0);
            }
        }
        Sensor AddZoneData(double[] _data, BankZoneData _zd, int _sensor, int _Sensors)
        {
            int packets = _zd.size / _Sensors;
            if (packets * _Sensors != _zd.size)
                throw (new Exception("Result::AddZoneData: packets*Sensors!=size"));
            Sensor ret = new Sensor(Tp, packets);
            for (int packet = 0; packet < packets; packet++)
            {
                Meas m = ret.MMeas[packet];
                m.Source = _data[_zd.idata + packet * _Sensors + _sensor];
                //                    m.index = _idata1 + packet * lsensors + s;
            }
            return (ret);
        }
        public void AddZoneA(double[] _data, BankZoneDataA _zA)
        {
            Zone z = new Zone(Tp);
            MZone.Add(z);
            z.VZoneLength = _zA.zone_length;
            z.MSensor = new Sensor[_zA.length];
            for (int i = 0; i < _zA.length; i++)
                z.MSensor[i] = AddZoneData(_data, _zA.MZones[i], i, _zA.length);
            CalcXStart(MZone.Count() - 1, false);
            CalcXStart(MZone.Count() - 1, true);
            z.Calc(MZone.Count() == 1 ? null : MZone[MZone.Count() - 2]);
            pr("AddZone: " + z.ToString(MZone.Count() - 1));
        }
        public void AddZoneACalibr(double[] _data, BankZoneDataA _zA)
        {
            Zone z = new Zone(EUnit.Line);
            MZone.Add(z);
            z.VZoneLength = _zA.zone_length;
            z.MSensor = new Sensor[_zA.length];
            for (int i = 0; i < _zA.length; i++)
                z.MSensor[i] = AddZoneData(_data, _zA.MZones[i], i, _zA.length);
            z.Calc(null);
            pr("AddZone: " + z.ToString(MZone.Count() - 1));
        }
    }
}
