using Accord.IO;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static Output<Cluster, IGeometry> InputTargetSplit(Cluster cluster)
        {
            Cluster input = BH.Engine.Base.Query.DeepClone(cluster);
            int r = m_Random.Next(0,cluster.Neighbours.Count);
            IGeometry target = input.Neighbours[r];
            input.Neighbours.RemoveAt(r);

            return new Output<Cluster, IGeometry>
            {
                Item1 = input,
                Item2 = target,
            };
        }
    }
}
