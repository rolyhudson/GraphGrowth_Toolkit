using BH.Engine.Base;
using BH.oM.Adapters.AIServices;
using BH.oM.Analytical;
using BH.oM.Analytical.Graph;
using BH.Engine.Analytical;
using BH.oM.Data.Collections;
using BH.oM.Geometry;
using BH.Engine.Geometry;
using BH.oM.GraphGrowth;
using BH.oM.GraphGrowth.Results;
using BH.oM.SpaceSyntax;
using BH.Engine.SpaceSyntax;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Collections;
using BH.Engine.Geometry;
using System.Transactions;
using Accord;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static List<GraphGrowthResult> Simulate(Graph graph, IGrowthConfiguration configuration, int steps = 0, List<IGrowthRule> analytics = null, string resultCase = "resultCase", int tick = 1, bool run = false, bool runChecks = true)
        {
            ResetGlobals();
            //dymaic cast to other configs
            GrowthConfiguration config = configuration as GrowthConfiguration;
            m_Results = new List<GraphGrowthResult>();
            if (!run)
                return m_Results;

            double isoDist = config.WalkingSpeed * config.Time;
            m_Graph = graph.DeepClone();
            m_Tick = tick;

            while (m_Timestep < steps)
            {
                //get relations to attempt branch
                List<IRelation> candidates = m_Graph.RelationsToBranch(config);
                
                int bCount = 0;
                Dictionary<Guid, List<Guid>> adj = Analytical.Query.Adjacency(m_Graph);
                foreach (IRelation relation in candidates )
                {
                    List<Node> junctions = m_Graph.JunctionNodes(adj);

                    var r =  Modify.Branch(m_Graph, relation, config.TargetBranchLength, config.PercentageLengthChange, junctions);
                    if(r != null )
                    {
                        m_Graph = r.Item1;
                        m_Results.Add(new BranchGrowthResult(m_Graph.BHoM_Guid, m_Timestep+"_" + bCount, m_Timestep, r.Item2));
                        bCount++;
                    }
                    //update adjacency
                    adj = Analytical.Query.Adjacency(m_Graph);
                }

                List<Node> toExtend = m_Graph.NodesToBranch(config, adj);
                foreach(Node node in toExtend)
                {
                    var n = m_Graph.Extend(node, config.TargetExtensionLength, config.PercentageLengthChange);
                    if (n != null)
                    {
                        m_Graph = n.Item1;
                        m_Results.Add(new ExtendGrowthResult(m_Graph.BHoM_Guid, m_Timestep + "_" + bCount, m_Timestep, n.Item2));
                        bCount++;
                    }
                }

                m_Timestep += m_Tick;
                m_Results.Add(new FinalGraphGrowthResult(m_Graph.BHoM_Guid, "", m_Timestep, m_Graph));
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
        //SS LLM method
            ////filter building nodes
            //List<Building> buildings1 = new List<Building>();
            //m_Graph.Entities.Values.ToList().ForEach(e => 
            //{
            //if (e is Building)
            //buildings1.Add((Building)e);
            //});
            ////find clusters
            //List<Cluster> clusters = FindClusters(clusterSettings, buildings1);
            ////grow clusters
            //ConcurrentBag<Building> buildings = new ConcurrentBag<Building>();

            //Parallel.ForEach(clusters, cluster =>
            //{
            //Polyline polyline = GrowClusterLLM(cluster, aIServicesConfig);
            //if (polyline.ControlPoints.Count == 0)
            //return;
            //Building building = new Building()
            //{
            //Boundary = polyline,
            //Position = polyline.Centroid()
            //};
            //buildings.Add(building);
            //});

            ////validate no overlaps
            //List<Building> valid = CheckRoadOverlaps(buildings.ToList(), m_Graph.Relations.Select(r => r.Curve).ToList());
            //valid = CheckBuildingOverlaps(valid, buildings1);
            ////add validated to graph
            //valid.ForEach(b => m_Graph.Entities.Add(b.BHoM_Guid, b));
            ////track step results
            //m_Results.Add(new LLMGrowthResult(m_Graph.BHoM_Guid, 1, m_Timestep, m_Graph.DeepClone()));

            //valid.ForEach(b => m_Results.Add(new ClusterGrowthResult(b.BHoM_Guid, 1, m_Timestep, b.Boundary)));
    }


}
