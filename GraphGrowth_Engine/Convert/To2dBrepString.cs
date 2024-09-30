using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Convert
    {
        public static string ITo2dBrepString(this IGeometry geometry)
        {
            return To2dBrepString(geometry as dynamic);
        }

        /***************************************************/

        public static string To2dBrepString(this IGeometry geometry)
        {
            return null;
        }

        /***************************************************/

        public static string To2dBrepString(this Polyline geometry)
        {
            List<string> list = new List<string>();
            foreach (Point p in geometry.ControlPoints)
            {
                list.Add($"[{Math.Round(p.X, 1).ToString()},{Math.Round(p.Y, 1).ToString()}]");
            }
            return $"[{string.Join(",", list)}]";
        }

        /***************************************************/

        public static string To2dBrepString(this CompositeGeometry compositeGeometry)
        {
            if (compositeGeometry.Elements.All(x => x is Polyline))
            {
                List<Polyline> polylines = new List<Polyline>();
                foreach (var element in compositeGeometry.Elements)
                {
                    polylines.Add((Polyline)element);
                }
                List<Polyline> joined = polylines.Join(tolerance: 1);
                if (joined.Count > 0 && joined[0].IsClosed())
                    return To2dBrepString(joined[0]);
            }
            return null;
        }

        /***************************************************/

        public static string To2dBrepString(this Cluster cluster)
        {
            
            List<string> list = new List<string>();
            foreach (IGeometry geom in cluster.Neighbours) 
            {
                list.Add(geom.ITo2dBrepString());
            }
            string coordSets = $"[{string.Join(",", list)}]";
            //string s = $"{{\"Buildings\": {coordSets} }}";
            return coordSets;
        }
    }
}
