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
        //TODO convert to dynamic cast method
        public static string To2dBrepString(IGeometry geometry)
        {
            if (geometry is Polyline)
            {
                
                Polyline poly = (Polyline)geometry;
                List<string> list = new List<string>();
                foreach (Point p in poly.ControlPoints)
                {
                    list.Add($"[{Math.Round(p.X, 1).ToString()},{Math.Round(p.Y, 1).ToString()}]");
                }
                return $"[{string.Join(",", list)}]";
            }
            return null;
        }

        public static string To2dBrepString(CompositeGeometry compositeGeometry)
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

        public static string To2dBrepString(Cluster cluster)
        {
            
            List<string> list = new List<string>();
            foreach (IGeometry geom in cluster.Neighbours) 
            {
                if(geom is CompositeGeometry)
                {
                    CompositeGeometry compositeGeometry = (CompositeGeometry)geom;
                    list.Add(To2dBrepString(compositeGeometry));
                }
                
            }
            string coordSets = $"[{string.Join(",", list)}]";
            //string s = $"{{\"Buildings\": {coordSets} }}";
            return coordSets;
        }
    }
}
