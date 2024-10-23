using BH.Engine.Geometry;
using BH.oM.Geometry;
using BH.oM.SpaceSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Query
    {
        public static List<Node> ClosestJunctionsDirect(Point point, List<Node> junctions)
        {
            List<Node> sorted = junctions.OrderBy(j => j.Position.SquareDistance(point)).ToList();
            return sorted;
        }
    }
}
