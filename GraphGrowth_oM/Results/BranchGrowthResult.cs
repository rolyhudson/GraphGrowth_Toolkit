using BH.oM.Analytical.Graph;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth.Results
{
    public class BranchGrowthResult : GraphGrowthResult
    {
        public virtual List<IRelation> Branches { get; set; } = new List<IRelation>();

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public BranchGrowthResult(IComparable objectId, IComparable resultCase, double timeStep, List<IRelation> branch) : base(objectId, resultCase, timeStep)
        {
            Branches = branch;
        }
    }
}
