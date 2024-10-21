using BH.oM.Analytical.Graph;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth.Results
{
    public class RoadGrowthResult : GraphGrowthResult
    {
        public virtual Line Line { get; set; } = new Line();

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public RoadGrowthResult(IComparable objectId, IComparable resultCase, double timeStep, Line newLine) : base(objectId, resultCase, timeStep)
        {
             Line = newLine;
        }
    }
}
