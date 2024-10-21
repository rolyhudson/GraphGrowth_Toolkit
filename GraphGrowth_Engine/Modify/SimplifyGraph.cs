using BH.Engine.Base;
using BH.oM.Analytical.Graph;
using BH.Engine.Analytical;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Text;
using BH.oM.Base;
using BH.oM.Data.Library;
using System.Linq;
using BH.oM.Geometry;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Modify
    {
        public static Graph SimplifyGraph(this Graph graph)
        {
            Graph clone = graph.DeepClone();
            Dictionary<Guid, List<Guid>> adjacency = clone.Adjacency(RelationDirection.Both);
            Dictionary<Guid, List<Guid>> pairAdjacency = adjacency.Where(k => k.Value.Count == 2).ToDictionary(k => k.Key, k => k.Value);
            while (pairAdjacency.Count > 0)
            {
                
                KeyValuePair<Guid, List<Guid>> kvp = pairAdjacency.First();
                //direction 1 from 0 to 1
                List<IRelation> relations = new List<IRelation>();
                relations = clone.Relation(clone.Entities[kvp.Value[0]], clone.Entities[kvp.Key]);
                relations.AddRange(clone.Relation(clone.Entities[kvp.Key], clone.Entities[kvp.Value[1]]));

                if (relations.Count == 2)
                {
                    List<PolyCurve> curve = Geometry.Compute.IJoin(relations.Select(r => r.Curve).ToList());
                    Relation relation = Analytical.Create.Relation(clone.Entities[kvp.Value[0]], clone.Entities[kvp.Value[1]], curve: curve.First());
                    clone.Relations.Add(relation);
                }
                //direction 2 from 1 to 0
                relations = new List<IRelation>();
                relations = clone.Relation(clone.Entities[kvp.Value[1]], clone.Entities[kvp.Key]);
                relations.AddRange(clone.Relation(clone.Entities[kvp.Key], clone.Entities[kvp.Value[0]]));
                if (relations.Count == 2)
                {
                    List<PolyCurve> curve = Geometry.Compute.IJoin(relations.Select(r => r.Curve).ToList());
                    Relation relation = Analytical.Create.Relation(clone.Entities[kvp.Value[1]], clone.Entities[kvp.Value[0]], curve: curve.First());
                    clone.Relations.Add(relation);
                }

                //remove entity also removes relations
                clone.RemoveEntity(kvp.Key);
                //get the new adjacency
                adjacency = clone.Adjacency(RelationDirection.Both);
                pairAdjacency = adjacency.Where(k => k.Value.Count == 2).ToDictionary(k => k.Key, k => k.Value);
            }
            
            return clone;
        }
    }
}
