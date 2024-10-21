using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Modify
    {
        // Fisher-Yates Shuffle  
        public static IEnumerable<T> Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random(); // Consider using a better seed  
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
