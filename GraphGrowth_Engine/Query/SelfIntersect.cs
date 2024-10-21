using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Analytical.Graph;
using BH.oM.Data.Collections;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Query
    {
        public static bool SelfIntersect(this Graph graph, Relation candidate, double tolerance)
        {
            List<ICurve> links = graph.Relations.Select(i => i.Curve).ToList();
            List<ICurve> clone = links.DeepClone();
            DomainTree<ICurve> indexTree = Spatial.Create.DomainTree(clone);

            foreach (Line check in Data.Query.ItemsInRange(indexTree, candidate.Curve.IBounds().Inflate(tolerance).DomainBox()))
            {
                List<Point> inter = check.ICurveIntersections(candidate.Curve);
                foreach (Point point in inter)
                {
                    //check if the point is the start or end 
                    if (point.SquareDistance(candidate.Curve.IStartPoint()) < tolerance || point.SquareDistance(candidate.Curve.IEndPoint()) < tolerance)
                        continue;
                    else
                        return true;
                }
            }
            return false;
        }
    }
}
