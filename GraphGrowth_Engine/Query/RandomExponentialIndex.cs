using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Query
    {
        public static int RandomExponentialIndex(int min, int max, double lambda = 0.1)
        {
            // Generate an exponential random value  
            double exponentialRandomValue = -Math.Log(m_Random.NextDouble()) / lambda;

            // Scale and truncate to get an integer in the desired range  
            int selectedInteger = (int)Math.Floor(exponentialRandomValue) + 1;
            selectedInteger = Math.Max(min, Math.Min(selectedInteger, max));

            return selectedInteger;
        }

        public static List<int> RandomExponentialIndex(int count, int min, int max, double lambda = 0.1)
        {
            List<int> indexes = new List<int>();
            int maxTries = 100;
            int attempt = 0;
            while (indexes.Count < count)
            {
                int next = RandomExponentialIndex(min, max, lambda);
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

        public static Random m_Random = new Random();
    }

    

}
