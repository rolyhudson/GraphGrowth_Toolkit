using BH.oM.Analytical.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth.Results
{
    public class LLMGrowthResult : GraphGrowthResult
    {
        public virtual Graph LLMGraph { get; set; } =  new Graph();

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public LLMGrowthResult(IComparable objectId, IComparable resultCase, double timeStep, Graph graph) : base(objectId, resultCase, timeStep)
        {
            LLMGraph = graph;
        }
    }
}
