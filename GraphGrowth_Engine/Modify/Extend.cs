using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Analytical.Graph;
using BH.oM.Base;
using BH.oM.Geometry;
using BH.oM.SpaceSyntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Modify
    {
        public static Output<Graph, List<IRelation>> Extend(this Graph graph, Node nodeToExtend, double targetExtendLength, double maxChange)
        {
            Graph clone = graph.DeepClone();
            IRelation outgoing = graph.Relations.Find(x => x.Source.Equals(nodeToExtend.BHoM_Guid));
            Vector d1 = outgoing.Curve.IEndPoint() - outgoing.Curve.IStartPoint();    
            d1 = d1.Normalise();
            double side = m_Random.NextDouble() > 0.5 ? 1 : -1;
            double rot = m_Random.NextDouble() > 0.5 ? Math.PI : Math.PI / 2 * side;
            d1 = d1.Rotate(rot, Vector.ZAxis);
            double percentageChange = 1 + ((m_Random.NextDouble() * maxChange * 2) - maxChange);

            d1 = d1 * (percentageChange * targetExtendLength);
            Point newP0 = nodeToExtend.Position + d1;
            Node newN0 = new Node() { Position = newP0 };
            Line newRoad = Geometry.Create.Line(nodeToExtend.Position, newN0.Position);
            Relation r = new Relation() { Source = nodeToExtend.BHoM_Guid, Target = newN0.BHoM_Guid, Curve = newRoad };

            clone.Entities.Add(newN0.BHoM_Guid, newN0);
            Relation r1 = new Relation() { Source = newN0.BHoM_Guid, Target = nodeToExtend.BHoM_Guid, Curve = newRoad.Flip() };

            List<IRelation> newRel = new List<IRelation>();
            newRel.Add(r);
            newRel.Add(r1);
            clone.Relations.AddRange(newRel);
            return new Output<Graph, List<IRelation>>
            {
                Item1 = clone,
                Item2 = newRel,
            };
        }
    }
}
