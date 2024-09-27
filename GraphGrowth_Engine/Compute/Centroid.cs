using BH.Engine.Geometry;
using BH.Engine.Geospatial;
using BH.oM.Geometry;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static BH.oM.Geometry.Point UTMCentroid(Feature feature, int utmGridZone)
        {
            IGeometry geo = feature.ToUTM(utmGridZone);
            return Compute.Centroid((geo as dynamic)); 
        }

        private static BH.oM.Geometry.Point ICentroid(this IGeometry geo)
        {
            return Compute.Centroid((geo as dynamic));
        }

        private static BH.oM.Geometry.Point Centroid(this IGeometry geo)
        {
            BH.Engine.Base.Compute.RecordError("No centroid method exists for "+geo.GetType().ToString());
            return null;
        }

        private static BH.oM.Geometry.Point Centroid(this BH.oM.Geometry.Point geo)
        {
            return geo;
        }

        private static BH.oM.Geometry.Point Centroid(this BH.oM.Geometry.Polyline geo)
        {
            return geo.ControlPoints.Average();
        }

        private static BH.oM.Geometry.Point Centroid(this CompositeGeometry geo)
        {
            List<BH.oM.Geometry.Point> points = new List<BH.oM.Geometry.Point>();
            foreach (IGeometry g in geo.Elements)
            {
                points.Add(Compute.Centroid(g as dynamic));
            }
            return points.Average();
        }
    }
}
