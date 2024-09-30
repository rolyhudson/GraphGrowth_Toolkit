using BH.Engine.Geospatial;
using BH.oM.Geometry;
using BH.oM.Geospatial;
using System;
using System.Collections.Generic;
using System.Text;
using Accord.Collections;
using System.Linq;
using BH.Engine.Geometry;
using BH.oM.GraphGrowth;
using Accord.Math.Distances;

namespace BH.Engine.Adapters.GraphGrowth
{
    public static partial class Compute
    {
        public static List<Cluster> FindClusters(ClusterSettings clusterSettings, int utmGridZone, List<Feature> features)
        {
            Dictionary<BH.oM.Geometry.Point, IGeometry> dict  = new Dictionary<oM.Geometry.Point, IGeometry> ();
            foreach (var feature in features)
            {
                IGeometry geo = feature.ToUTM(utmGridZone);
                double area = 0;
                if (geo is CompositeGeometry)
                {
                    CompositeGeometry composite = (CompositeGeometry)geo;
                    area = composite.Area();
                }
                else
                {
                    area = (geo as dynamic).Area();
                }
                
                if (area > clusterSettings.MaximumArea) 
                    continue;
                BH.oM.Geometry.Point utmcentroid = Compute.ICentroid(geo);
                dict.Add(utmcentroid, geo);
            }
            return Cluster(dict, clusterSettings);
            
        }

        public static List<Cluster> FindClusters(ClusterSettings clusterSettings, List<Building> buildings)
        {
            Dictionary<BH.oM.Geometry.Point, IGeometry> dict = new Dictionary<oM.Geometry.Point, IGeometry>();
            buildings.ForEach(b => dict.Add(b.Position, b.Boundary));
            return Cluster(dict, clusterSettings);
        }

        private static List<Cluster> Cluster(Dictionary<BH.oM.Geometry.Point, IGeometry> dict, ClusterSettings clusterSettings)
        {
            List<double[]> kdpoints = new List<double[]>();
            List<BH.oM.Geometry.Point> points = dict.Keys.ToList();
            List<IGeometry> geometries = dict.Values.ToList();
            foreach (BH.oM.Geometry.Point p in points)
            {
                if (p == null)
                {
                    //utmDict.Remove()
                    continue;
                }
                double[] pt = new double[] { p.X, p.Y };
                kdpoints.Add(pt);
            }
            m_KDTree = KDTree.FromData<IGeometry>(kdpoints.ToArray(), geometries.ToArray(), true);

            List<Cluster> clusters = new List<Cluster>();
            List<int> visited = new List<int>();
            int attempts = 0;
            while (clusters.Count < clusterSettings.NumberClusters)
            {
                int i = m_Random.Next(dict.Count);
                if (visited.Contains(i))
                {
                    attempts++;
                    if (attempts > 100)
                        break;
                }
                visited.Add(i);
                var p = points[i];
                double[] query = { points[i].X, points[i].Y };
                //first get the neighbourhood around the current spec
                var neighbours = m_KDTree.Nearest(query, radius: clusterSettings.MaximumDistance);
                if (neighbours.Count < clusterSettings.MinimumMembers)
                    continue;
                var ordered = neighbours.OrderBy(x => x.Distance).ToList();
                //ordered.RemoveAt(0);
                List<IGeometry> n = ordered.Select(x => x.Node.Value).ToList();
                Cluster cluster = new Cluster()
                {
                    Neighbours = n,
                };
                clusters.Add(cluster);
            }
            return clusters;
        }
        private static KDTree<IGeometry> m_KDTree;
    }
}
