using Accord.Math.Geometry;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Convert
    {
        public static Polyline ToPolyline(string brepString)
        {
            string result = brepString.Replace("[[", "").Replace("]]", "");
            List<Point> points = new List<Point>();
            
            string[] pairs = result.Split(new string[] { "],[" }, StringSplitOptions.None);
            foreach (var pair in pairs)
            {
                string[] xy = pair.Split(',');
                Point p = new Point()
                {
                    X = double.Parse(xy[0]),
                    Y = double.Parse(xy[1])
                };
                points.Add(p);
            }
            return new Polyline() { ControlPoints = points };
        }

        public static List<Polyline> ToPolylines(string brepString)
        {
            List < Polyline > converted = new List < Polyline >();  
            if (brepString.StartsWith("[[["))
            {
                string polylines = brepString.Replace("[[[", "").Replace("]]]", "");
                string[] results = polylines.Split(new string[] { "]],[[" }, StringSplitOptions.None);
                foreach (string line in results)
                {
                    converted.Add(ToPolyline(line));
                }
            }
            else
            {
                converted.Add(ToPolyline(brepString));
            }
            return converted;
        }
    }
}
