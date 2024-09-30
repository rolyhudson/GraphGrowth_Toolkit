using BH.Engine.Base;
using BH.oM.Adapters.AIServices;
using BH.oM.Analytical;
using BH.oM.Analytical.Graph;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using BH.oM.GraphGrowth.Results;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static List<GraphGrowthResult> Simulate(Graph graph, AIServicesConfig aIServicesConfig, ClusterSettings clusterSettings, int steps = 0, List<IRule> analytics = null, string resultCase = "resultCase", int tick = 1, bool run = false, bool runChecks = true)
        {
           

            ResetGlobals();
            m_Results = new List<GraphGrowthResult>();
            if (!run)
                return m_Results;

            m_Graph = graph.DeepClone();
            m_Tick = tick;

            while (m_Timestep < steps)
            {
                //filter building nodes
                List<Building> buildings1 = new List<Building>();
                m_Graph.Entities.Values.ToList().ForEach(e => 
                { 
                    if(e is  Building)
                        buildings1.Add((Building)e);
                });
                //find clusters
                List<Cluster> clusters = FindClusters(clusterSettings, buildings1);
                //grow clusters
                ConcurrentBag<Building> buildings = new ConcurrentBag<Building>();

                Parallel.ForEach(clusters, cluster =>
                {
                    Polyline polyline = GrowCluster(cluster, aIServicesConfig);
                    if (polyline.ControlPoints.Count == 0)
                        return;
                    Building building = new Building()
                    {
                        Boundary = polyline,
                        Position = polyline.Centroid()
                    };
                    buildings.Add(building);
                });

                //validate no overlaps
                List<Building> valid = CheckRoadOverlaps(buildings.ToList(), m_Graph.Relations.Select(r => r.Curve).ToList());
                valid = CheckBuildingOverlaps(valid, buildings1);
                //add validated to graph
                valid.ForEach(b => m_Graph.Entities.Add(b.BHoM_Guid,b));
                //track step results
                m_Results.Add(new LLMGrowthResult(m_Graph.BHoM_Guid, 1, m_Timestep, m_Graph.DeepClone()));

                valid.ForEach(b => m_Results.Add(new ClusterGrowthResult(b.BHoM_Guid, 1, m_Timestep, b.Boundary)));
                
                m_Timestep += m_Tick;

            }

            return m_Results;
        }

        /***************************************************/
        /**** Private Methods                           ****/
        /***************************************************/
        private static void ResetGlobals()
        {
            m_Timestep = 0;
        }

        /***************************************************/
        /****           Private Fields                  ****/
        /***************************************************/

        private static Random m_Random = new Random();

        private static int m_Timestep = 0;

        private static int m_Tick = 1;

        private static List<GraphGrowthResult> m_Results = new List<GraphGrowthResult>();

        private static List<Node> m_Nodes = new List<Node>();

        private static Graph m_Graph;

        /***************************************************/
    }


}
