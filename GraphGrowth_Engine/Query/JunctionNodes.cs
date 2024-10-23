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
        public static List<Node> JunctionNodes(this Graph graph, Dictionary<Guid, List<Guid>> adj, bool excludeBuildings = true)
        {
            
            List<Guid> junctionIds = adj.Where(kvp => kvp.Value.Count > 2).Select(kvp => kvp.Key).ToList();
            HashSet<Guid> visited = new HashSet<Guid>(junctionIds);
            List<Node> junctions = new List<Node>();
            foreach (KeyValuePair<Guid, IBHoMObject> kvp in graph.Entities)
            {
                if(visited.Contains(kvp.Key))
                {
                    if (!excludeBuildings)
                        junctions.Add(kvp.Value as Node);
                    else
                    {
                        if (!(kvp.Value is Building))
                            junctions.Add(kvp.Value as Node);
                    }
                    
                }
            }
            return junctions;

        }
    }
}
