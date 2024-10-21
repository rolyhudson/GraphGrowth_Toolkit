using BH.oM.Base;
using BH.oM.SpaceSyntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth
{
    public class GrowthConfiguration : BHoMObject, IGrowthConfiguration
    {
        public virtual Node Start { get; set; } = new Node();

        public virtual double WalkingSpeed { get; set; } = 1.3;

        public virtual double Time { get; set; } = 60;

        public virtual int BranchesPerStep { get; set; } = 10;

        public virtual double SelfIntersectTolerance { get; set; } = 10.0;
    }
}
