using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;

using PARLIB;
using UPAR.TS;
using UPAR.Def;
using UPAR.SG;

namespace UPAR
{
    public class ParAll : ParMain
    {
        [DisplayName("Типоразмер"), Browsable(true), De]
        public L_TypeSize TSSet { get; set; }

        [DisplayName("Дефектоскоп"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DefectPars Defect { get; set; }

        //[DisplayName("Толщиномер"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public ThickPars Thickness { get; set; }

        //[DisplayName("Просмотровщик"), Browsable(true), De]
        //[TypeConverter(typeof(ExpandableObjectConverter))]
        //public ViewerPars Viewer { get; set; }

        [DisplayName("Цвета"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public CColors Colors { get; set; }

        [DisplayName("Максимальное количество зон"), DefaultValue(60), Browsable(true), De]
        public int MaxZones { get; set; }

        [DisplayName("Размер зоны, мм"), DefaultValue(200), Browsable(true), De]
        public int ZoneSize { get; set; }

        [DisplayName("Разное"), Browsable(true), De]
        public SomePars Some { get; set; }

        [DisplayName("Размеры"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public DimensionsPars Dimensions { get; set; }
        //        public DimNRSpars Dimensions { get; set; }


        //[DisplayName("Номер партии"), Browsable(true), De]
        //public string Batch { get; set; }

        [Browsable(false), De]
        public L_Client Clients { get; set; }

        //[Browsable(true), De]
        //public PipeServerPars PipeServer { get; set; }

        ParAll(ESource _Source, string _file, string _schema, string _tname, string _file_desc)
            : base(_Source, "ParAll.ST", _file, _schema, _tname, _file_desc)
        {
            Schema = _schema;
        }
        private static ParAll Instance = null;
        public static void Create(string _tname)
        {
            ESource Source = ESource.File;
            string fname = Path.ChangeExtension(Application.ExecutablePath, "file");
            if (!File.Exists(fname))
            {
                fname = Path.ChangeExtension(Application.ExecutablePath, "udl");
                if (File.Exists(fname))
                    Source = ESource.SQL;
            }
            Instance = new ParAll(Source, Path.ChangeExtension(Application.ExecutablePath, "tree"), "dbo", _tname, Path.ChangeExtension(Application.ExecutablePath, "xml"));
            Instance.Defect.IsDBS = Source == ESource.SQL;
        }
        public static void Create(string _tname, ESource _Source)
        {
            Instance = new ParAll(_Source, Path.ChangeExtension(Application.ExecutablePath, "tree"), "dbo", _tname, Path.ChangeExtension(Application.ExecutablePath, "xml"));
            Instance.Defect.IsDBS = _Source == ESource.SQL;
        }
        public static ParAll ST { get { return (Instance); } }

        [Browsable(false)]
        public string Schema { get; private set; }

        [Browsable(false)]
        public static TypeSize CTS { get { return (ST.TSSet.Current); } }

        [Browsable(false)]
        public static SolidGroupPars SG { get { return (ST.Defect.Cross.SolidGroup); } }
    }
}
