using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.Engine.Spatial;
using BH.oM.Base;
using BH.oM.Data.Collections;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Create
    {
        public static Output<Line, List<Line>> GraphLink(List<Line> curves, Point point, double searchDistance)
        {
            
            double minDist = double.MaxValue;
            Line link = null;
            Line toSplit = null;
            List<Line> clone = curves.DeepClone();
            //DomainTree<Line> indexTree = Data.Create.DomainTree(clone, x => x.Bounds().DomainBox(), 16, 16, 60);
            DomainTree<Line> indexTree = Spatial.Create.DomainTree(clone);
            Point splitPt = new Point();
            foreach (Line candidate in Data.Query.ItemsInRange(indexTree, point.Bounds().Inflate(searchDistance).DomainBox()))
            {
                Point closest = candidate.IClosestPoint(point);
                double d = closest.SquareDistance(point);
                if (d < minDist)
                {
                    minDist = d;
                    link = Geometry.Create.Line(point, closest);
                    toSplit = candidate;
                    splitPt = closest;
                }
            }
            clone.Remove(toSplit);
            clone.AddRange(toSplit.SplitAtPoints(new List<Point>() { splitPt }));
            return new Output<Line, List<Line>> { Item1 = link, Item2 = clone };
        }

        public static Output<List<Line>, List<Line>> GraphLink(List<Line> curves, List<Point> points, double searchDistance)
        {
            List<Line> clone = curves.DeepClone();
            ConcurrentBag<Line> lines = new ConcurrentBag<Line>();
            foreach(Point point in points)
            {
                var result = GraphLink(clone, point, searchDistance);
                lines.Add(result.Item1);
                clone = result.Item2;
            }
            return new Output<List<Line>, List<Line>> { Item1 = lines.ToList(), Item2 = clone };
        }
    }
}
