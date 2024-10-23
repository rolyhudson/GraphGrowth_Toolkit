using BH.oM.Analytical.Graph;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth.Results
{
    public class ExtendGrowthResult : GraphGrowthResult
    {
        public virtual List<IRelation> Extension { get; set; } = new List<IRelation>();

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public ExtendGrowthResult(IComparable objectId, IComparable resultCase, double timeStep, List<IRelation> extend) : base(objectId, resultCase, timeStep)
        {
            Extension = extend;
        }
    }
}
