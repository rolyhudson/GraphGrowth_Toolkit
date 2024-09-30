using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth
{
    public class ClusterSettings : BHoMObject
    {
        public virtual double MaximumDistance { get; set; } = 25.0;
        public virtual double MaximumArea { get; set; } = 100;
        public virtual int MinimumMembers { get; set; } = 4;
        public virtual int NumberClusters { get; set; } = 10;
    }
}
