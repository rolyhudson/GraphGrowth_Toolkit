using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.Engine.Spatial;
using BH.oM.Data.Collections;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static List<Building> CheckRoadOverlaps(List<Building> proposedBuildings, List<ICurve> existingRoads)
        {
            if (proposedBuildings.Count == 0) return proposedBuildings;
            List<Building> nonOverlapping = proposedBuildings.DeepClone();
            DomainTree<Building> indexTree = Data.Create.DomainTree(nonOverlapping.Select((x, i) => Data.Create.DomainTreeLeaf(x, x.Boundary.Bounds().DomainBox())));

            for (int i = 0; i < existingRoads.Count; i++)
            {
                foreach (var b in Data.Query.ItemsInRange(indexTree, existingRoads[i].Bounds().DomainBox()))
                {
                    List<PolyCurve> overlaps = existingRoads[i].BooleanIntersection(b.Boundary);
                    if (overlaps != null)
                    {
                        nonOverlapping.Remove(b);
                        break;
                    }
                }
            }
            return nonOverlapping;
        }
    }
}
