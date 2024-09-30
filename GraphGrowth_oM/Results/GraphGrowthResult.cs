using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BH.oM.Analytical.Results;

namespace BH.oM.GraphGrowth.Results
{
    public class GraphGrowthResult : IResult
    {
        [Description("Name of the node that this result belongs to.")]
        public virtual IComparable ObjectId { get; }

        [Description("Identifier for the flow simulation that the result belongs to.")]
        public virtual IComparable ResultCase { get; }

        [Description("Time step for time history results.")]
        public virtual double TimeStep { get; }

        /***************************************************/
        /**** Constructors                              ****/
        /***************************************************/

        public GraphGrowthResult(IComparable objectId, IComparable resultCase, double timeStep)
        {
            ObjectId = objectId;
            ResultCase = resultCase;
            TimeStep = timeStep;
        }

        /***************************************************/
        /**** IComparable Interface                     ****/
        /***************************************************/

        public int CompareTo(IResult other)
        {
            GraphGrowthResult otherRes = other as GraphGrowthResult;

            if (otherRes == null)
                return this.GetType().Name.CompareTo(other.GetType().Name);

            int n = this.ObjectId.CompareTo(otherRes.ObjectId);
            if (n == 0)
            {
                int l = this.ResultCase.CompareTo(otherRes.ResultCase);
                return l == 0 ? this.TimeStep.CompareTo(otherRes.TimeStep) : l;
            }
            else
            {
                return n;
            }
        }
    }
}
