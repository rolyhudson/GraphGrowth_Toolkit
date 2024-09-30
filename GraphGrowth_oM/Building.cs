using BH.oM.Geometry;
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
