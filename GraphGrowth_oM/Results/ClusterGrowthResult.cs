using BH.oM.Analytical.Graph;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth.Results
{
    public class ClusterGrowthResult : GraphGrowthResult
    {
        public virtual IGeometry NewNeighbour {  get; set; }

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public ClusterGrowthResult(IComparable objectId, IComparable resultCase, double timeStep, IGeometry geometry) : base(objectId, resultCase, timeStep)
        {
            NewNeighbour = geometry;
        }
    }
}
