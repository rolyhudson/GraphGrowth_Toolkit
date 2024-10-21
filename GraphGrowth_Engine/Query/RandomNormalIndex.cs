using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Query
    {
        public static int RandomNormalIndex(int min, int max, double stdDev, double mean)
        {

            // Generate a normally distributed random number (Box-Muller transform)  
            double u1 = 1.0 - m_Random.NextDouble(); // Uniform(0,1] random doubles  
            double u2 = 1.0 - m_Random.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); // Random normal(0,1)  
            double randNormal = mean + stdDev * randStdNormal;

            // Convert to an integer in the desired range  
            int selectedInteger = (int)Math.Round(randNormal, MidpointRounding.AwayFromZero);

            // Clamp within min and max  
            selectedInteger = Math.Max(min, Math.Min(selectedInteger, max));

            return selectedInteger;
        }

        public static List<int> RandomNormalIndex(int count, int min, int max, double stdDev, double mean)
        {
            List<int> indexes = new List<int>();
            int maxTries = 100;
            int attempt = 0;
            while (indexes.Count < count)
            {
                int next = RandomNormalIndex(min, max, stdDev, mean);
                if (indexes.Contains(next))
                {
                    attempt++;
                    if (attempt == maxTries)
                    {
                        BH.Engine.Base.Compute.RecordWarning("Too many attempts to generate an index, check inputs.");
                        break;
                    }

                }
                else
                {
                    indexes.Add(next);
                }
            }
            return indexes;
        }

    }
}
