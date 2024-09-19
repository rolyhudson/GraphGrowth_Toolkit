using BH.Engine.Geometry;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Modify 
    {
        public static List<Line> SplitByLength(this Line line, double maxLength)
        {
            List<Line> result = new List<Line>();
            if (line.Length() < maxLength)
            {
                result.Add(line);
                return result;
            }

            int parts = (int)Math.Ceiling(line.Length() / maxLength);
            double partLength = line.Length() / parts;

            List<Point> points = new List<Point>();
            Vector d = line.Direction();
            d = d.Normalise(); 
            d = d * partLength;
            Point p1 = line.StartPoint();
            for (int i = 0; i < parts; i++)
            {
                Point next  = p1 + d;
                points.Add(next);
                p1 = next;
            }
            result = line.SplitAtPoints(points);
            return result;
        }
    }
}
