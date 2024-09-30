using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Query
    {
        public static BoundingBox Bounds (this Cluster cluster)
        {
            CompositeGeometry composite = new CompositeGeometry()
            {
                Elements = cluster.Neighbours
            };
            BoundingBox bbox1 = composite.Bounds();
            return bbox1;
        }
    }
}
