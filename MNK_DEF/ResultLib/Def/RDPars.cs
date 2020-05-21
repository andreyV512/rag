using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Xml.Serialization;

using UPAR;
using UPAR.Def;
using UPAR.TS;
using UPAR.TS.TSDef;
using Share;
using SQL;

namespace ResultLib.Def
{
    public class RDPars1 
    {
        [DisplayName("Множители датчиков")]
        public double[] Gains { get; set; }

        [DisplayName("Использовать внутренние дефекты")]
        public bool IsIn { get; set; }

        [DisplayName("Пороги")]
        public double[] borders { get; set; }

        [DisplayName("Внутренние пороги")]
        public double[] bordersIn { get; set; }

        [DisplayName("Длина в начале")]
        public int LenghtStart { get; set; }

        [DisplayName("Длина в конце")]
        public int LenghtEnd { get; set; }

        [DisplayName("Множитель в начале")]
        public double MultStart { get; set; }

        [DisplayName("Множитель в конце")]
        public double MultEnd { get; set; }

        [DisplayName("Фильтр")]
        public FilterPars filterPars { get; set; }

        [DisplayName("Фильтр внутренний")]
        public FilterPars filterParsIn { get; set; }

        [DisplayName("Опорная частота")]
        public double SampleRate { get; set; }

        [DisplayName("Мертвая зона в начале")]
        public int DeadZoneStart { get; set; }

        [DisplayName("Мертвая зона в конце")]
        public int DeadZoneFinish { get; set; }

        [DisplayName("Размер зоны")]
        public int ZoneSize { get; set; }

        [XmlIgnore]
        [DisplayName("Количество датчиков"), Browsable(false)]
        public int Sensors { get { return (Gains.Length); } }

        [DisplayName("Ширина медианного фильтра")]
        public int WidthMedianFilter { get; set; }

        [DisplayName("Включен медианный фильтр")]
        public bool IsMedianFilter { get; set; }

        [DisplayName("Фактическая скорость вращения, об/c")]
        public double? Revolutions { get; set; }

        [XmlIgnore]
        public bool Changed = false;

        [XmlIgnore]
        static string Unit
        {
            get
            {
                switch (Share.Current.UnitType)
                {
                    case EUnit.Cross:
                        return ("PCross");
                    case EUnit.Line:
                        return ("PLine");
                    case EUnit.Thick:
                        return ("PThick");
                }
                return (null);
            }
        }
        [XmlIgnore]
        EUnit Tp;

        RDPars1() { }
        public RDPars1(TypeSize _typeSize, EUnit _Tp)
        {
            LoadSettings(_typeSize);
            Tp = _Tp;
            Revolutions = null;
        }


        public static RDPars1 Create(string _src)
        {
            byte[] m = Encoding.GetEncoding(1251).GetBytes(_src);
            MemoryStream ms = new MemoryStream(m);
            return (new XmlSerializer(typeof(RDPars1)).Deserialize(ms) as RDPars1);
        }
        public void Serialize(FileStream _stream)
        {
            new XmlSerializer(typeof(RDPars1)).Serialize(_stream, this);
        }
        public string S_Serialize()
        {
            MemoryStream ms = new MemoryStream();
            new XmlSerializer(typeof(RDPars1)).Serialize(ms, this);
            return (Encoding.GetEncoding(1251).GetString(ms.ToArray()));
        }

        void LoadSettings(TypeSize _p)
        {
            DefCL dlc = new DefCL(Tp);
            L_L502Ch lch = dlc.LCh;
            borders = new double[2];
            bordersIn = new double[2];
            filterPars = null;
            filterParsIn = null;
            Gains = new double[lch.Count];
            for (int i_s = 0; i_s < Sensors; i_s++)
                Gains[i_s] = lch[i_s].Gain;

            borders[0] = dlc.Border1;
            borders[1] = dlc.Border2;
            bordersIn[0] = dlc.Border1In;
            bordersIn[1] = dlc.Border2In;
            DeadZoneStart = dlc.DeadZoneStart;
            DeadZoneFinish = dlc.DeadZoneFinish;
            filterPars = dlc.Filter.Clone();
            IsIn = dlc.IsFinterIn;
            filterParsIn = dlc.FilterIn.Clone();

            LenghtStart = dlc.Tails.LenghtStart;
            LenghtEnd = dlc.Tails.LenghtEnd;
            MultStart = dlc.Tails.MultStart;
            MultEnd = dlc.Tails.MultEnd;
            SampleRate = dlc.L502.FrequencyPerChannel;
            ZoneSize = ParAll.ST.ZoneSize;
            WidthMedianFilter = ParAll.ST.Defect.Some.WidthMedianFilter;
            IsMedianFilter = ParAll.ST.Defect.Some.IsMedianFilter;
        }
        public void SaveSettings(TypeSize _p)
        {
            DefCL dcl = new DefCL(Tp);
            L_L502Ch lch = dcl.LCh;
            if (Sensors != lch.Count)
                throw (new Exception("RDPars.SaveSettings: Не соответствует количество датчиков в текущем типоразмере"));
            for (int i_s = 0; i_s < Sensors; i_s++)
                lch[i_s].Gain = Gains[i_s];
            dcl.Border1 = borders[0];
            dcl.Border2 = borders[1];
            dcl.Border1In = bordersIn[0];
            dcl.Border2In = bordersIn[1];
            dcl.DeadZoneStart = DeadZoneStart;
            dcl.DeadZoneFinish = DeadZoneFinish;

            dcl.Filter = filterPars.Clone();
            if (filterParsIn != null)
                dcl.FilterIn = filterParsIn.Clone();

            dcl.Tails.LenghtStart = LenghtStart;
            dcl.Tails.LenghtEnd = LenghtEnd;
            dcl.Tails.MultStart = MultStart;
            dcl.Tails.MultEnd = MultEnd;
            dcl.L502.FrequencyPerChannel = SampleRate;
            ParAll.ST.ZoneSize = ZoneSize;
            ParAll.ST.Defect.Some.WidthMedianFilter = WidthMedianFilter;
            ParAll.ST.Defect.Some.IsMedianFilter = IsMedianFilter;
        }
    }
}
