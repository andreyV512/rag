using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using SQL;
using ParamSettings;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Drawing.Design;

namespace RRepPars
{
    public class SelectionPars
    {
        [DisplayName("Установка")]
        public string Unit { get; set; }

        [DisplayName("Начальная дата")]
        [Editor(typeof(DateTimePickerEditor), typeof(UITypeEditor))]
        public DateTime DT0 { get; set; }

        [DisplayName("Конечная дата")]
        [Editor(typeof(DateTimePickerEditor), typeof(UITypeEditor))]
        public DateTime DT1 { get; set; }

        [DisplayName("Дефектоскопист")]
        [TypeConverter(typeof(RConverter))]
        public string User { get; set; }

        [DisplayName("Типоразмер")]
        [TypeConverter(typeof(RConverter))]
        public string TypeSize { get; set; }

        [DisplayName("Заказчик")]
        [TypeConverter(typeof(RConverter))]
        public string Client { get; set; }

        [DisplayName("Группа прочности")]
        [TypeConverter(typeof(RConverter))]
        public string SG { get; set; }

        [DisplayName("Результат")]
        [TypeConverter(typeof(RConverter))]
        public string Result { get; set; }

        [DisplayName("Длина зоны, мм")]
        public int ZoneSize { get; set; }

        class RConverter : StringConverter
        {
            public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
            public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
            public override StandardValuesCollection GetStandardValues(
              ITypeDescriptorContext context)
            {
                string name = context.PropertyDescriptor.Name;
                string field = null;
                if (name == "User") field = "RUser";
                else if (name == "TypeSize") field = "TypeSize";
                else if (name == "Client") field = "Client";
                else if (name == "SG") field = "SolidGroup";
                else if (name == "Result") field = "Result";
                List<string> L = new List<string>();
                SelectionPars sp = context.Instance as SelectionPars;
                L.Add(null);
                Select S = new Select(string.Format("SELECT DISTINCT {0} FROM Tubes where DT >= @DT0 and DT <= @DT1 order by {0}", field));
                S.AddParam("@DT0", System.Data.SqlDbType.DateTime, sp.DT0);
                S.AddParam("@DT1", System.Data.SqlDbType.DateTime, sp.DT1);
                while (S.Read())
                    L.Add(Convert.ToString(S[0]));
                S.Dispose();
                return (new StandardValuesCollection(L));
            }
        }
        public void NonZero()
        {
            if (Unit == null) Unit = "";
            if (User == null) User = "";
            if (TypeSize == null) TypeSize = "";
            if (Client == null) Client = "";
            if (SG == null) SG = "";
            if (Result == null) Result = "";
        }
        public string Conditions()
        {
            string SQL = "";
            if (User != null && User.Length != 0)
                SQL += string.Format(" and RUser='{0}'", User);

            if (TypeSize != null && TypeSize.Length != 0)
                SQL += string.Format(" and TypeSize='{0}'", TypeSize);

            if (Client != null && Client.Length != 0)
                SQL += string.Format(" and Client='{0}'", Client);

            if (SG != null && SG.Length != 0)
                SQL += string.Format(" and SolidGroup='{0}'", SG);

            if (Result != null && Result.Length != 0)
                SQL += string.Format(" and Result='{0}'", Result);


            if (Client != null && Client.Length != 0)
                SQL += string.Format(" and Client='{0}'", Client);

            return (SQL);
        }
        public class DateTimePickerEditor : UITypeEditor
        {

            IWindowsFormsEditorService editorService;
            DateTimePicker picker = new DateTimePicker();

            public DateTimePickerEditor()
            {
                picker.Format = DateTimePickerFormat.Custom;
                picker.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            }

            public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
            {
                return UITypeEditorEditStyle.DropDown;
            }

            public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
            {
                if (provider != null)
                {
                    this.editorService = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
                }

                if (this.editorService != null)
                {
                    DateTime val = Convert.ToDateTime(value);
                    if (val < picker.MinDate)
                        val = DateTime.Now;
                    if (val > picker.MaxDate)
                        val = DateTime.Now;
                    picker.Value = val;
                    this.editorService.DropDownControl(picker);
                    value = picker.Value;
                }

                return value;
            }
        }
    }
}
