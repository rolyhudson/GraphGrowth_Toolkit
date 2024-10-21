using BH.oM.Geometry;
using BH.oM.SpaceSyntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth
{
    public class Building : Node
    {
        public virtual Polyline Boundary { get; set; }  = new Polyline();
    }
}
