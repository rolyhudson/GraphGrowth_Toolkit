using BH.oM.Analytical.Graph;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth.Results
{
    public class FinalGraphGrowthResult : GraphGrowthResult
    {
        public virtual Graph FinalGraph { get; set; } = new Graph();

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public FinalGraphGrowthResult(IComparable objectId, IComparable resultCase, double timeStep, Graph finalGraph) : base(objectId, resultCase, timeStep)
        {
            FinalGraph = finalGraph;
        }
    }
}
