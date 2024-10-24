using System;
using System.Collections.Generic;
using NetFunctions;

namespace DijkstraIPRouting;

public class Dijkstra
{
   public Dijkstra()
   {
   }

   public static List<Vertex> DijkstraShortestPath(
      Dictionary<Vertex, List<Edge>> routersGraph,
      Vertex sourceIp,
      Vertex destinationIp)
   {
      if (SameNetwork(sourceIp, destinationIp))
      {
         return [];
      }

      Vertex? sourceRouter = GetRouterFromIp(sourceIp, routersGraph);
      Vertex? destinationRouter = GetRouterFromIp(destinationIp, routersGraph);

      var allShortestPaths = new List<Vertex>();
      var priorityQueue = InitializeQueue(sourceRouter!, routersGraph);

      while (priorityQueue.Count > 0)
      {
         var currentVertex = ExtractLowestPriorityVertex(priorityQueue);
         allShortestPaths.Add(currentVertex);
         if (currentVertex.Name == destinationRouter!.Name)
         {
            break;
         }

         foreach (Edge edge in routersGraph[currentVertex])
         {
            var adjacentVertex = routersGraph.Keys.Where(v => v.Name == edge.Destination.Name).First();
            if (Relax(currentVertex, adjacentVertex, edge.EdgeWeight))
            {
               UpdatePriorityInQueue(priorityQueue, adjacentVertex);
            }
         }
      }
      List<Vertex> path = GetPathFromTree(destinationRouter!);

      return path;
   }

   /// <summary>
   /// Initializes the priority queue with initial wieghts for each vertex in the graph.
   /// Ideally, the priority queue should be implemented as a min-heap where the priorities can be updated
   /// as the algorithm progresses. However, the ProirityQueue class in C# does not support updating priorities.
   /// Therefore, we will use a List<Vertex> to store the vertices with their weight estimates (priorities).
   /// </summary>
   /// <param name="sourceIp"></param>
   /// <param name="routersGraph"></param>
   /// <returns></returns>
   private static List<Vertex> InitializeQueue(Vertex sourceIp, Dictionary<Vertex, List<Edge>> routersGraph)
   {
      var Q = new List<Vertex>();
      foreach (Vertex vertex in routersGraph.Keys)
      {
         if (vertex.Name.Equals(sourceIp.Name))
         {
            vertex.Weight = 0;
         }
         else
         {
            vertex.Weight = int.MaxValue;
         }
         Q.Add(vertex);
      }
      return Q;
   }

   private static Vertex ExtractLowestPriorityVertex(List<Vertex> priorityQueue)
   {
      Vertex minVertex = priorityQueue[0];
      foreach (Vertex vertex in priorityQueue)
      {
         if (vertex.Weight < minVertex.Weight)
         {
            minVertex = vertex;
         }
      }
      priorityQueue.Remove(minVertex);
      return minVertex;
   }

   private static bool Relax(Vertex currentVertex, Vertex adjacentVertex, int edgeWeight)
   {
      if (adjacentVertex.Weight > currentVertex.Weight + edgeWeight)
      {
         adjacentVertex.Weight = currentVertex.Weight + edgeWeight;
         adjacentVertex.Parent = currentVertex;
         return true;
      }
      return false;
   }

   private static void UpdatePriorityInQueue(List<Vertex> priorityQueue, Vertex vertex)
   {
      foreach (Vertex v in priorityQueue)
      {
         if (v.Name == vertex.Name)
         {
            v.Weight = vertex.Weight;
         }
      }
   }

   /// <summary>
   /// Dijkstra's algorithm creates a tree of shortest path from the destination vertex to the source vertex
   /// by assigning a parent corresponding to the vertex that leads to the shortest path.
   /// By traversing the tree in reverse order, we can obtain the shortest path from the source to the destination.
   /// </summary>
   /// <param name="destinationIp"></param>
   /// <returns></returns>
   private static List<Vertex> GetPathFromTree(Vertex destinationIp)
   {
      var shortestPathFromSourceToDestination = new List<Vertex>();

      // If the destinationIp vertex has no parent after running Dijkstra's, then
      // there's no path from the source to the destination
      if (destinationIp.Parent == null)
      {
         return shortestPathFromSourceToDestination;
      }

      Vertex currentVertex = destinationIp;
      while (currentVertex.Parent != null)
      {
         shortestPathFromSourceToDestination.Add(currentVertex);
         currentVertex = currentVertex.Parent;
      }
      // Add the source vertex to the path
      shortestPathFromSourceToDestination.Add(currentVertex);

      return shortestPathFromSourceToDestination.Reverse<Vertex>().ToList();
   }

   private static bool SameNetwork(Vertex sourceIp, Vertex destinationIp)
   {
      var sourceIpValue = NetFunctionsTools.Ipv4ToValue(sourceIp.Name);
      var sourceIpMask = NetFunctionsTools.GetSubnetMaskValue(sourceIp.NetMask);
      var destinationIpValue = NetFunctionsTools.Ipv4ToValue(destinationIp.Name);
      var destinationIpMask = NetFunctionsTools.GetSubnetMaskValue(destinationIp.NetMask);

      return (sourceIpValue & sourceIpMask) == (destinationIpValue & destinationIpMask);
   }

   private static Vertex? GetRouterFromIp(Vertex ip, Dictionary<Vertex, List<Edge>> routersGraph)
   {
      foreach (Vertex router in routersGraph.Keys)
      {
         if (NetFunctionsTools.IPsSameSubnet(ip.Name, router.Name, router.NetMask))
         {
            return router;
         }
      }
      return null;
   }
}
