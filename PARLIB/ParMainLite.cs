using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PARLIB
{
    public enum ESource { File, SQL }
    public class ParMainLite : ParBase
    {
        [DisplayName("Окна"), Browsable(true), De]
        public L_WindowLPars Wins { get; set; }

        [DisplayName("Протокол настроек"), Browsable(true), De]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public ProtocolPar Protocol { get; set; }

        [DisplayName("Архивировать настройки"), Browsable(true), De]
        [TypeConverter(typeof(BooleanconverterRUS))]
        public bool ArchivePars { get; set; }

        [Browsable(false)]
        public MetaPar MP { get; private set; }

        string file;
        string schema;
        string Unit;
        string file_desc;

        [Browsable(false)]
        public ESource Source { get; private set; }

        protected ParMainLite(ESource _Source, string _path, string _file, string _schema, string _Unit, string _file_desc)
        {
            Source = _Source;
            Parent = null;
            PropertyIndex = -1;
            PropertyName = _path;
            file_desc = _file_desc;
            file = _file;
            schema = _schema;
            Unit = _Unit;
            MP = new MetaPar(_Source, this);
            MP.LoadTree(file, schema, Unit);
        }
        public void LoadDesc()
        {
            MP.LoadDesc(file_desc);
        }
        public void SaveToSQL()
        {
            MP.SaveToSQL(schema, Unit);
        }
        public void SaveToFile()
        {
            MP.SaveToFile(file);
        }
        public void Save()
        {
            MP.Save(file, schema, Unit, file_desc);
        }
    }
}
