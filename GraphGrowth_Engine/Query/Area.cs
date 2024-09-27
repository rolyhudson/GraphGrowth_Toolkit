using BH.Engine.Geometry;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Geometry
{
    public static partial class Query
    {
        public static double Area(this CompositeGeometry compositeGeometry, double tolerance = Tolerance.Distance)
        {
            if (compositeGeometry.Elements[0] is Polyline)
            {
                //risky but assume the outer is the first in elements
                Polyline polyline = (Polyline)compositeGeometry.Elements[0];
                if (polyline.IsClosed())
                {
                    return polyline.IArea();
                }
            }
            if(compositeGeometry.Elements[0] is CompositeGeometry)
            {
                CompositeGeometry element = (CompositeGeometry)compositeGeometry.Elements[0];
                return element.Area();
            }
            if (compositeGeometry.Elements.All(x => x is Polyline))
            {
                List<Polyline> polylines = new List<Polyline>();    
                foreach (var element in compositeGeometry.Elements)
                {
                    polylines.Add((Polyline)element);
                }
                List<Polyline> joined = polylines.Join(tolerance: 1);
                if(joined.Count > 0 && joined[0].IsClosed())
                    return joined[0].Area();
            }
            return 0;
        }
    }
}
