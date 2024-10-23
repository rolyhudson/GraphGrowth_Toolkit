using BH.Engine.Analytical;
using BH.oM.Analytical.Graph;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Query
    {
        public static bool IsRelationToBuilding(this Graph graph, IRelation relation)
        {
            if(graph.Entity(relation.Source) is Building || graph.Entity(relation.Target) is Building)
                return true;
            return false;
        }

    }
}
