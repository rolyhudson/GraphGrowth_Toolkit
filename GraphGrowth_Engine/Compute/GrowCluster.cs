using BH.Adapter.AIServices;
using BH.Engine.Base;
using BH.Engine.Geometry;
using BH.oM.Adapters.AIServices;
using BH.oM.Adapters.AIServices.Requests;
using BH.oM.Adapters.AIServices.Results;
using BH.oM.Geometry;
using BH.oM.GraphGrowth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static Polyline GrowCluster(Cluster cluster, AIServicesConfig aIServicesConfig)
        {
            Cluster clone = cluster.DeepClone();
            Vector returnTranslate = BH.Engine.Geometry.Create.Vector(clone.Bounds().Min);

            clone = Modify.TranslateToOrigin(clone);

            string brepstring = Convert.To2dBrepString(clone);

            DialogueMessage message1 = new DialogueMessage()
            {
                Content = m_SystemPrompt,
                DialogueRole = DialogueRole.System
            };
            DialogueMessage message2 = new DialogueMessage()
            {
                Content = brepstring,
                DialogueRole = DialogueRole.User
            };

            DialogueCompletion dialogueCompletion = new DialogueCompletion()
            {
                Messages = new List<DialogueMessage>() { message1, message2 }
            };
            Polyline b = new Polyline();
            try
            {
                AIServicesAdapter adapter = new AIServicesAdapter();
                var result = adapter.Pull(dialogueCompletion, actionConfig: aIServicesConfig);
                if (result.First() is OpenAIResult)
                {
                    OpenAIResult openAIResult = (OpenAIResult)result.First();
                    b = Convert.ToPolyline(openAIResult.Completions[0]);
                    b = b.Translate(returnTranslate);
                    //implement wait and retry
                }
            }
            catch (Exception ex)
            {
                Base.Compute.RecordError($"The LLM threw an error {ex.Message}. {ex.InnerException}.");
            }

            
            return b;
        }
        private static string m_SystemPrompt = "Users will provide context geometry as an input. The input format is a list of lists, where each inner list is a sequence of coordinate pairs defining a closed polyline, with the first and last coordinates being the same to indicate closure. The provided data represents a collection of closed polylines, which are described using nested arrays of coordinate pairs. Here's a detailed breakdown of the format: Outer Array: The outermost array contains multiple inner arrays, each representing a separate closed polyline. Polyline Arrays: Each inner array represents a single closed polyline. A closed polyline is a series of points (coordinates) where the first and last points are the same, forming a closed loop. Coordinate Pairs: Within each polyline array, there are pairs of numbers. Each pair [x, y] represents a point in a 2D space, where x is the horizontal coordinate and y is the vertical coordinate. Closure of Polylines: The first and last coordinate pairs in each polyline array are identical, indicating that the polyline is closed. Your task is to predict a single new polyline that adheres to the geometric pattern defined by the input data. The new polyline should: Match the Geometric Pattern: Follow the general orientation, shape and style of the existing polylines. Fit in Available Space: Be placed in an unoccupied region within the 2D space. Avoid Overlap: Not be inside or overlap with any existing context polylines. Points per polyline: The new polyline should have a similar number of points as the input polylines. The output format is [[x1,y1],[x2,y2],[x3,y3],[x4,y4],[xn,yn]]";
    }

    
}
