using BH.Engine.Geometry;
using BH.oM.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static List<Point> ExponentialDistribution(this Line line, int numberOfPoints, double scaleParameter)
        {
            Random random = new Random();
            List<Point> points = new List<Point>();

            while (points.Count < numberOfPoints)
            {
                // Generate an exponential random value  
                double uniformRandomValue = random.NextDouble();
                double exponentialRandomValue = -scaleParameter * Math.Log(1 - uniformRandomValue);

                // Check if the point is within the line  
                if (exponentialRandomValue <= line.Length())
                {
                    points.Add(line.PointAtLength(exponentialRandomValue));
                }
            }

            return points;
        }

        public static List<Point> NormalDistribution(this Line line, int numberOfPoints, double mean = 0, double stdDev = 10.0)
        {
            Random random = new Random();
            List<Point> points = new List<Point>();

            while (points.Count < numberOfPoints)
            {
                // Generate a normally distributed random number (Box-Muller transform)  
                double u1 = 1.0 - random.NextDouble(); // Uniform(0,1] random doubles  
                double u2 = 1.0 - random.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); // Random normal(0,1)  
                double randNormal = mean + stdDev * randStdNormal; // Random normal(mean,stdDev^2)  

                // Truncate values that are outside the line length  
                while (randNormal < 0 || randNormal > line.Length())
                {
                    u1 = 1.0 - random.NextDouble();
                    u2 = 1.0 - random.NextDouble();
                    randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                    randNormal = mean + stdDev * randStdNormal;
                }
                points.Add(line.PointAtLength(randNormal));
            }

            return points;
        }
    }
}
