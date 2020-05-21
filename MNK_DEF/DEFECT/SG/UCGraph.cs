using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using SQL;
using Protocol;

namespace Defect.SG
{ 
    public partial class UCGraph : UserControl
    {
        public UCGraph()
        {
            InitializeComponent();
        }
        public string Schema { get; set; }
        public void Exec(string _typeSize, Int64 _TubeId,string _GroupName, int _EtalonId)
        {
            chart1.Series.Clear();
            if (_typeSize != null)
            {
                string SQL = string.Format("SELECT e.sGroup, g.color, p.etalon, p.par, p.val"
                 + " FROM {0}.SGroups g"
                 + " INNER JOIN {0}.SGEtalons e ON g.name = e.sGroup AND g.typeSize = e.typeSize and e.IsOn='true'"
                 + " INNER JOIN {0}.SGEtalonPars p ON g.name = p.sGroup AND g.typeSize = p.typeSize and e.id=p.etalon"
                 + " where g.typeSize='{1}' and g.IsOn='true'"
                 + " order by e.sGroup,p.etalon,p.par",
                 Schema,_typeSize);
                Select S = new Select(SQL);
                string lg = "";
                int le = -1;
                Series ps = null;
                DataPointCollection pd = null;
                int x = 0;
                while (S.Read())
                {
                    if (S["sGroup"] as string != lg || Convert.ToInt32(S["etalon"]) != le)
                    {
                        lg = S["sGroup"] as string;
                        le = Convert.ToInt32(S["etalon"]);
                        ps = new Series();
                        ps.ChartType = SeriesChartType.Line;
                        ps.Color = Color.FromArgb(Convert.ToInt32(S["color"]));
                        if(le==_EtalonId && lg==_GroupName)
                            ps.BorderWidth = 3;
                        ps.Tag = "Гр: "+lg + " Эт: " + le.ToString();
                        chart1.Series.Add(ps);
                        pd = ps.Points;
                        x = 0;
                    }
                    pd[pd.AddXY(x, S["val"])].Tag = "Пар: " + x.ToString() + " Зн: " + ((Single)S["val"]).ToString();
                    x++;
                }
                S.Dispose();
            }
            if(_TubeId>=0)
            {
                Series ps = null;
                ps = new Series();
                ps.ChartType = SeriesChartType.Line;
                ps.Color = Color.Black;
                ps.BorderDashStyle = ChartDashStyle.Dash;
                ps.BorderWidth = 2;
                ps.Tag = "Тр: " + _TubeId.ToString();
                chart1.Series.Add(ps);
                DataPointCollection pd = ps.Points;

                Select S = new Select(string.Format("SELECT val from {0}.SGTubePars where tube_id={1} order by par",
                Schema,
                _TubeId
                ));
                int i = 0;
                while (S.Read())
                {
                    pd[pd.AddXY(i, S["val"])].Tag = "Пар: " + i.ToString() + " Зн: " + ((Single)S["val"]).ToString();
                    i++;
                }
                S.Dispose();
            }
        }
        DataPoint dp = null;
        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult htr=chart1.HitTest(e.X, e.Y);
            if (htr == null)
                return;
            if(htr.Object==null)
                return;
            if (htr.Series == null)
                return;
            if(htr.Object.GetType()!=typeof(DataPoint))
                return;
//            pr(htr.Series.Name + " " + htr.Series.Tag as string+" "+htr.Object.GetType().ToString());
            if (dp != null)
            {
                dp.Label = null;
                dp=null;
            }
            dp = htr.Object as DataPoint;
            dp.Label = htr.Series.Tag as string+" "+ dp.Tag as string;
        }
        protected void pr(string _msg)
        {
            ProtocolST.pr(ToString() + " " + _msg);
        }
    }
}
