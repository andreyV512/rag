using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ResultLib.Def;
using ResultLib.Thick;
using Share;

namespace ResultLib
{
    public class RK
    {
        public CThick Thick { get; private set; }
        public CDef Cross { get; private set; }
        public CDef Line { get; private set; }

        RK()
        {
            Thick = new CThick();
            Cross = new CDef();
            Line = new CDef();
        }
        static RK Instance = null;
        public static RK ST { get { if (Instance == null) Instance = new RK(); return (Instance); } }
        Result lresult = null;
        public Result result
        {
            get { return lresult; }
            set
            {
                lresult = value;
                Cross.result = lresult.Cross;
                Cross.Link();
                Line.result = lresult.Line;
                Line.Link();

            }
        }
        public string ResultError { get { if (lresult == null) return (null); return (lresult.Error); } }

        public CDef cDef(EUnit _type)
        {
            if (_type == EUnit.Cross)
                return (Cross);
            if (_type == EUnit.Line)
                return (Line);
            return (null);
        }

        public class CThick
        {
            public ResultThickLite result = null;
            int? lZone = null;

            public int? Zone
            {
                get { return (lZone); }
                set
                {
                    lZone = value;
                    LinkZone();
                }
            }
            public override string ToString()
            {
                return (string.Format("Thick: Zone={1}", Zone.ToString()));
            }

            void LinkZone()
            {
                if (result == null)
                {
                    lZone = null;
                    return;
                }
                if (result.MZone.Count == 0)
                {
                    lZone = null;
                    return;
                }
                if (lZone == null)
                    return;
                if (lZone.Value < 0)
                    lZone = 0;
                else if (lZone.Value > result.MZone.Count - 1)
                    lZone = result.MZone.Count - 1;
            }
            public void Link()
            {
                LinkZone();
            }
        }
        public class CDef
        {
            public ResultDef result = null;
            int? lZone = null;
            int? lSensor = null;
            int? lMeasure = null;

            public int? Zone
            {
                get { return (lZone); }
                set
                {
                    lZone = value;
                    LinkZone();
                    LinkSensor();
                    LinkMeasure();
                }
            }
            public int? Sensor
            {
                get { return (lSensor); }
                set
                {
                    lSensor = value;
                    LinkSensor();
                    LinkMeasure();
                }
            }
            public int? Measure
            {
                get { return (lMeasure); }
                set
                {
                    lMeasure = value;
                    LinkMeasure();
                }
            }
            public override string ToString()
            {
                return (string.Format("Line: Sensor={0} Zone={1} Meas={2}", Sensor.ToString(), Zone.ToString(), Measure.ToString()));
            }

            void LinkZone()
            {
                if (result == null)
                {
                    lZone = null;
                    return;
                }
                if (result.MZone.Count == 0)
                {
                    lZone = null;
                    return;
                }
                if (lZone == null)
                    return;
                if (lZone.Value < 0)
                    lZone = 0;
                else if (lZone.Value > result.MZone.Count - 1)
                    lZone = result.MZone.Count - 1;
            }
            void LinkSensor()
            {
                if (lZone == null)
                {
                    lSensor = null;
                    return;
                }
                Zone Z = result.MZone[lZone.Value];
                if (Z.MSensor == null)
                {
                    lSensor = null;
                    return;
                }
                if (Z.MSensor.Length == 0)
                {
                    lSensor = null;
                    return;
                }
                if (lSensor == null)
                    return;
                if (lSensor.Value < 0)
                    lSensor = 0;
                else if (lSensor > Z.MSensor.Length - 1)
                    lSensor = Z.MSensor.Length - 1;
            }
            void LinkMeasure()
            {
                if (lSensor == null)
                {
                    lMeasure = null;
                    return;
                }
                Sensor S = result.MZone[lZone.Value].MSensor[lSensor.Value];
                if (S.MMeas == null)
                {
                    lMeasure = null;
                    return;
                }
                if (S.MMeas.Length == 0)
                {
                    lMeasure = null;
                    return;
                }
                if(lMeasure==null)
                    return;
                if (lMeasure.Value < 0)
                    lMeasure = 0;
                else if (lMeasure.Value > S.MMeas.Length - 1)
                    lMeasure = S.MMeas.Length - 1;
            }
            public void Link()
            {
                LinkZone();
                LinkSensor();
                LinkMeasure();
            }
        }
    }
}