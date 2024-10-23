using BH.oM.Analytical.Graph;
using BH.oM.Base;
using BH.oM.GraphGrowth;
using BH.oM.SpaceSyntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Query
    {
        public static List<Node> LeafNodes(this Graph graph, Dictionary<Guid, List<Guid>> adj, bool excludeBuildings = true)
        {
           
            List<Guid> leafIds = adj.Where(kvp => kvp.Value.Count == 1).Select(kvp => kvp.Key).ToList();
            HashSet<Guid> visited = new HashSet<Guid>(leafIds);
            List<Node> leafs = new List<Node>();
            foreach (KeyValuePair<Guid, IBHoMObject> kvp in graph.Entities)
            {
                if (visited.Contains(kvp.Key))
                {
                    if (!excludeBuildings)
                        leafs.Add(kvp.Value as Node);
                    else
                    {
                        if (!(kvp.Value is Building))
                            leafs.Add(kvp.Value as Node);
                    }
                }
            }
            return leafs;

        }
    }
}
