using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static Cluster RotateTranslate(Cluster cluster)
        {
            CompositeGeometry composite = new CompositeGeometry()
            {
                Elements = cluster.Neighbours
            };
            BoundingBox bbox1 = composite.Bounds();
            Point origin = bbox1.Centre();
            double rads = m_Random.NextDouble() * Math.PI;
            CompositeGeometry rotated = composite.Rotate(origin,Vector.ZAxis, rads);
            BoundingBox bbox2 = rotated.Bounds();
            Vector translate = new Point() - bbox2.Min;
            CompositeGeometry translated = rotated.Translate(translate);
            Cluster cluster1 = new Cluster()
            {
                Neighbours = translated.Elements,    
            };
            return cluster1;
        }
    }
}
