﻿using BH.Engine.Analytical;
using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Analytical.Graph;
using BH.oM.Data.Collections;
using BH.oM.Geometry;
using BH.oM.GraphGrowth.Results;
using BH.oM.SpaceSyntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Modify
    {
        public static Graph Branch(Graph graph, IRelation relationToSplit, double targetBranchLength, double maxChange, double selfIntersectTolerance)
        {
            
            Graph clone = graph.DeepClone();
            Node b1 = (Node)clone.Entity(relationToSplit.Target);
            Node b0 = (Node)clone.Entity(relationToSplit.Source);

            double segLength = b1.Position.Distance(b0.Position);
            Vector dir = b1.Position - b0.Position;
            dir = dir.Normalise();
            Vector perp = dir.CrossProduct(Vector.ZAxis);
            perp = perp.Normalise();
            dir = dir * (m_Random.NextDouble() * segLength);

            double side = m_Random.NextDouble() > 0.5 ? 1 : -1;
            // Generate a m_Random percentage between - maxChange and +  maxChange
            double percentageChange = 1 + ((m_Random.NextDouble() * maxChange*2) - maxChange);
            perp = perp * ( side * percentageChange * targetBranchLength);

            Point newP1 = b0.Position + perp + dir;
            Point newP0 = b0.Position + dir;

            List<IRelation> newRel = new List<IRelation>();

            //new link both directions
            Node newN1 = new Node() { Position = newP1 };
            Node newN0 = new Node() { Position = newP0 };
            Line newRoad = Geometry.Create.Line(newN1.Position, newN0.Position);
            
            //check self intersect
            Relation r = new Relation() { Source = newN1.BHoM_Guid, Target = newN0.BHoM_Guid, Curve = newRoad };
            if (clone.SelfIntersect(r, selfIntersectTolerance))
                return clone;

            clone.Entities.Add(newN1.BHoM_Guid, newN1);
            clone.Entities.Add(newN0.BHoM_Guid, newN0);
            Relation r1 = new Relation() { Source = newN0.BHoM_Guid, Target = newN1.BHoM_Guid, Curve = newRoad.Flip() };
            newRel.Add(r);
            newRel.Add(r1);

            Line newRoad1 = Geometry.Create.Line(b1.Position, newN0.Position);
            Relation r2 = new Relation() { Source = b1.BHoM_Guid, Target = newN1.BHoM_Guid, Curve = newRoad1 };
            Relation r3 = new Relation() { Source = newN1.BHoM_Guid, Target = b1.BHoM_Guid, Curve = newRoad1.Flip() };
            newRel.Add(r2);
            newRel.Add(r3);

            Line newRoad0 = Geometry.Create.Line(b0.Position, newN0.Position);
            Relation r4 = new Relation() { Source = b0.BHoM_Guid, Target = newN0.BHoM_Guid, Curve = newRoad0 };
            Relation r5 = new Relation() { Source = newN0.BHoM_Guid, Target = b0.BHoM_Guid, Curve = newRoad0.Flip() };
            newRel.Add(r4);
            newRel.Add(r5);

            clone.Relations.AddRange(newRel);

            //get the old relations here and remove
            List<IRelation> old = clone.Relation(b1, b0);
            old.AddRange(clone.Relation(b0, b1));
            old.ForEach(x => clone.Relations.Remove(x));
            return clone;
        }

        private static Random m_Random = new Random();
    }
}
