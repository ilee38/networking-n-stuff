using System;
using System.Collections.Generic;

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
      var shortestPath = new List<Vertex>();
      var priorityQueue = InitializeQueue(sourceIp, routersGraph);

      while (priorityQueue.Count > 0)
      {
         var currentVertex = ExtractLowestPriorityVertex(priorityQueue);
         shortestPath.Add(currentVertex);
         // if (currentVertex.Name == destinationIp.Name)
         // {
         //    return shortestPath;
         // }

         foreach (Edge edge in routersGraph[currentVertex])
         {
            var adjacentVertex = routersGraph.Keys.Where(v => v.Name == edge.Destination.Name).First();
            if (Relax(currentVertex, adjacentVertex, edge.EdgeWeight))
            {
               UpdatePriorityInQueue(priorityQueue, adjacentVertex);
            }
         }
      }
      return shortestPath;
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
}
