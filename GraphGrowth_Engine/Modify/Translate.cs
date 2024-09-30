using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial  class Modify
    {
        public static Cluster Translate(this Cluster cluster, Vector translation) 
        {
            Cluster clone = cluster.DeepClone();
            CompositeGeometry composite = new CompositeGeometry()
            {
                Elements = clone.Neighbours
            };
            CompositeGeometry translated = composite.Translate(translation);
            Cluster cluster1 = new Cluster()
            {
                Neighbours = translated.Elements,
            };
            return cluster1;
        }
    }
}
