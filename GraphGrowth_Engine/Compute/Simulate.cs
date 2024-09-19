using BH.Engine.Base;
using BH.oM.Analytical;
using BH.oM.Analytical.Graph;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static List<IResult> Simulate(Graph graph, int steps = 0, List<IRule> analytics = null, string resultCase = "resultCase", int tick = 1, bool run = false, bool runChecks = true)
        {
            m_Results = new List<IResult>();
            m_Graph = graph.DeepClone();
            m_Tick = tick;

            while (m_Timestep < steps)
            {

                m_Timestep += m_Tick;

            }

            return new List<IResult>();
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

        private static List<IResult> m_Results = new List<IResult>();

        private static List<Node> m_Nodes = new List<Node>();

        private static Graph m_Graph;

        /***************************************************/
    }


}
