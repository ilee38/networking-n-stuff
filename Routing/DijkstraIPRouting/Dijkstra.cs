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
      var priorityQueue = new PriorityQueue<Vertex, int>();

      sourceIp.Weight = 0;
      priorityQueue.Enqueue(sourceIp, sourceIp.Weight);
      while (priorityQueue.Count > 0)
      {
         var currentVertex = priorityQueue.Dequeue();
         if (!shortestPath.Contains(currentVertex))
         {
            shortestPath.Add(currentVertex);
            if (currentVertex.Name == destinationIp.Name)
            {
               return shortestPath;
            }
         }

         foreach (Edge edge in routersGraph[currentVertex])
         {
            var adjacentVertex = routersGraph.Keys.Where(v => v.Name == edge.Destination.Name).First();
            if (Relax(currentVertex, adjacentVertex, edge.EdgeWeight))
            {
               priorityQueue.Enqueue(adjacentVertex, adjacentVertex.Weight);
            }
         }
      }
      return shortestPath;
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
}
