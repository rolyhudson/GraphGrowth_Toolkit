using BH.oM.Analytical.Graph;
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
        public static List<Node> NodesToBranch(this Graph graph, GrowthConfiguration config, Dictionary<Guid, List<Guid>> adj)
        {
            Dictionary<Guid, double> entityDepth = SpaceSyntax.Compute.EntityDepth(graph, config.Start.BHoM_Guid);
            List<Node> leafs = graph.LeafNodes(adj);
            Dictionary<Guid, double> leafDepth = new Dictionary<Guid, double>();
            foreach (Node node in leafs)
            {
                leafDepth[node.BHoM_Guid] = entityDepth[node.BHoM_Guid];
            }
            List<KeyValuePair<Guid, double>> sorted = leafDepth.ToList();
            sorted.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
            List<Guid> keys = sorted.Select(s => s.Key).ToList();
            //random selection based on exp PDF
            List<int> indices = Query.RandomExponentialIndex(config.ExtensionsPerStep, 0, keys.Count() - 1, config.LambdaPDF);
            List<Node> candidates = new List<Node>();
            indices.ForEach(i => candidates.Add(graph.Entities[keys[i]] as Node));
            return candidates;
        }
    }
}
