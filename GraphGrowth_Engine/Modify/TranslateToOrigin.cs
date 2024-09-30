using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Modify
    {
        public static Cluster TranslateToOrigin(Cluster cluster)
        {
            Cluster clone = cluster.DeepClone();
            BoundingBox bbox1 = cluster.Bounds();
            Vector translate = new Point() - bbox1.Min;
            Cluster cluster1 = clone.Translate(translate);
            return cluster1;
        }
    }
}
