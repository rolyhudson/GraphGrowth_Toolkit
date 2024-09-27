using BH.oM.Base;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.GraphGrowth
{
    public class Cluster : BHoMObject
    {
        public virtual List<IGeometry> Neighbours { get; set; } = new List<IGeometry>();
    }
}
