using BH.oM.Analytical.Graph;
using BH.oM.Data.Collections;
using BH.oM.GraphGrowth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Query
    {
        public static List<IRelation> RelationsToBranch(this Graph graph, GrowthConfiguration config)
        {
            Dictionary<Guid, double> relDepth = SpaceSyntax.Compute.RelationDepth(graph, config.Start.BHoM_Guid);
            HashSet<double> known = new HashSet<double>();
            Dictionary<Guid, double> relUnique = new Dictionary<Guid, double>();
            foreach (KeyValuePair<Guid, double> kvp in relDepth)
            {
                //ignore less than min
                if (kvp.Value < config.BranchExclusionDistance / 2 )
                    continue;
                //ignore links to buildings
                IRelation relation = graph.Relations.Find(r => r.BHoM_Guid.Equals(kvp.Key));
                if (graph.IsRelationToBuilding(relation))
                    continue;
                //try add to hashset
                if(known.Add(Math.Round(kvp.Value,1)))
                {
                    relUnique.Add(kvp.Key, kvp.Value);  
                }
            }
            List<KeyValuePair<Guid, double>> sorted = relUnique.ToList();

            sorted.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));

            List<Guid> keys = sorted.Select(s => s.Key).ToList();
            //random selection based on exp PDF
            List<int> indices = Query.RandomExponentialIndex(config.BranchesPerStep, 0, keys.Count() - 1, config.LambdaPDF);
            List<IRelation> candidates = new List<IRelation>();
            indices.ForEach(i => candidates.Add(graph.Relations.Find(r => r.BHoM_Guid.Equals(keys[i]))));
            return candidates; 
        }
    }
}
