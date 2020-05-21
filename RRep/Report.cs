using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;

using RRepPars;
using SQL;

namespace RRep
{
    class Row
    {
        public Row()
        {
            IdTube = 0;
            DateTime_ = DateTime.Now;
            TypeSize = "?";
            Result = "?";
            SolidGroup = "?";
            MinThick = 0;
            NumOfZones = 0;
            Defectoscoper = "?";
            DCross = 0;
            DThick = 0;
            DLine = 0;
            Client = "?";
            Length = 0;
        }
        public Int64 IdTube { get; set; }
        public DateTime DateTime_ { get; set; }
        public string TypeSize { get; set; }
        public string Result { get; set; }
        public string SolidGroup { get; set; }
        public decimal MinThick { get; set; }
        public decimal NumOfZones { get; set; }
        public string Defectoscoper { get; set; }
        public int DCross { get; set; }
        public int DThick { get; set; }
        public int DLine { get; set; }
        public string Client { get; set; }
        public decimal Length { get; set; }

        public object oIdTube { set { if (value == null) return; if (value == DBNull.Value) return; IdTube = Convert.ToInt64(value); } }
        public object oDateTime_ { set { if (value == null) return; if (value == DBNull.Value) return; DateTime_ = Convert.ToDateTime(value); } }
        public object oTypeSize { set { if (value == null) return; if (value == DBNull.Value) return; TypeSize = Convert.ToString(value); } }
        public object oResult
        {
            set
            {
                if (value == null) return; if (value == DBNull.Value) return;
                string v = Convert.ToString(value);
                if (v == "б")
                    Result = "Брак";
                else if(v == "г")
                    Result = "Годно";
                else if(v=="2")
                    Result = "Класс 2";
                else
                    Result = "?";
            }
        }
        public object oSolidGroup { set { if (value == null) return; if (value == DBNull.Value) return; SolidGroup = Convert.ToString(value); } }
        public object oMinThick { set { if (value == null) return; if (value == DBNull.Value) return; MinThick = Convert.ToDecimal(value); } }
        public object oNumOfZones { set { if (value == null) return; if (value == DBNull.Value) return; NumOfZones = Convert.ToInt32(value); } }
        public object oDefectoscoper { set { if (value == null) return; if (value == DBNull.Value) return; Defectoscoper = Convert.ToString(value); } }
        public object oDCross { set { if (value == null) return; if (value == DBNull.Value) return; DCross = Convert.ToInt32(value) > 0 ? Convert.ToInt32(value) : 0; } }
        public object oDThick { set { if (value == null) return; if (value == DBNull.Value) return; DThick = Convert.ToInt32(value) > 0 ? Convert.ToInt32(value) : 0; } }
        public object oDLine { set { if (value == null) return; if (value == DBNull.Value) return; DLine = Convert.ToInt32(value) > 0 ? Convert.ToInt32(value) : 0; } }
        public object oClient { set { if (value == null) return; if (value == DBNull.Value) return; Client = Convert.ToString(value); } }
        public object oLength { set { if (value == null) return; if (value == DBNull.Value) return; Length = Convert.ToDecimal(value); } }
    }
    class RowPars
    {
        public DateTime DT0 = DateTime.Now;
        public DateTime DT1 = DateTime.Now;
        public DateTime DTReport = DateTime.Now;
        public int allTubes = 0;
        public int okTubes = 0;
        public decimal allLength = 0;
        public decimal okLength = 0;
        public int ZoneSize = 0;
    }
    class Report
    {
        public static bool Exec(SelectionPars _selection, ReportViewer _reportViewer)
        {
            _selection.NonZero();
            string SQL = "select * from Tubes where DT >= @DT0 and DT <= @DT1";
            SQL += _selection.Conditions();
            SQL += " order by DT, ID";

            Select S = new Select(SQL);
            S.AddParam("@DT0", System.Data.SqlDbType.DateTime, _selection.DT0);
            S.AddParam("@DT1", System.Data.SqlDbType.DateTime, _selection.DT1);

            List<Row> L = new List<Row>();
            RowPars rp = new RowPars();
            while (S.Read())
            {
                Row r = new Row();
                r.oIdTube = S["Id"];
                r.oDateTime_ = S["DT"];
                r.oTypeSize = S["TypeSize"];
                r.oResult = S["Result"];
                r.oSolidGroup = S["SolidGroup"];
                r.oNumOfZones = S["Zones"];
                r.oDefectoscoper = S["RUser"];
                r.oClient = S["Client"];
                r.oLength = S["Length"];
                r.oMinThick = S["MinThickness"];

                rp.allLength += r.Length;
                if (r.Result != "Брак")
                {
                    rp.okTubes++;
                    rp.okLength += r.Length;
                }


                L.Add(r);
            }
            S.Dispose();
            rp.DT0 = _selection.DT0;
            rp.DT1 = _selection.DT1;
            rp.allTubes = L.Count;
            List<ReportParameter> param = new List<ReportParameter>();
            param.Add(new ReportParameter("from", rp.DT0.ToString()));
            param.Add(new ReportParameter("to", rp.DT1.ToString()));
            param.Add(new ReportParameter("orderTime", _selection.Unit + "      Дата: " + rp.DTReport.ToString("d MMMM yyyy г")));
            param.Add(new ReportParameter("allTubes", rp.allTubes.ToString()));
            param.Add(new ReportParameter("okTubes", rp.okTubes.ToString()));
            param.Add(new ReportParameter("allZones", rp.allLength.ToString()));
            param.Add(new ReportParameter("okZones", rp.okLength.ToString()));

            //SQL = "select SolidGroup, count(SolidGroup) sm from TubesStat where DateTime >= @DT0 and DateTime <= @DT1";
            //SQL += Conditions(_selection);
            //SQL += " group by SolidGroup order by sm desc";
            //S = new SelectP(SQL);
            //S.AddParam("@DT0", System.Data.SqlDbType.DateTime, _selection.DT0);
            //S.AddParam("@DT1", System.Data.SqlDbType.DateTime, _selection.DT1);
            //string sSG = " ";
            //while (S.Read())
            //{
            //    string s = S["SolidGroup"].ToString();
            //    if(s.Length==0)
            //        s="' '";
            //    sSG += s + "(" + S["sm"].ToString() + ") ";
            //}
            //S.Dispose();

            //param.Add(new ReportParameter("SG", sSG));

            _reportViewer.LocalReport.DataSources.Clear();
            _reportViewer.LocalReport.ReportPath = "ReportTubes.rdlc";
            _reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", L));
            _reportViewer.LocalReport.SetParameters(param);
            PageSettings ps = new PageSettings();
            ps.Margins.Left = 10;
            ps.Margins.Right = 0;
            ps.Margins.Top = 0;
            ps.Margins.Bottom = 0;
            //ps.Landscape = true;
            _reportViewer.SetPageSettings(ps);
            _reportViewer.LocalReport.Refresh();
            _reportViewer.RefreshReport();
            return (true);
        }
    }
}
