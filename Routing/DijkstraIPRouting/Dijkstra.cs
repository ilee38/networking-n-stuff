using System;
using System.Collections.Generic;

namespace DijkstraIPRouting;

public class Dijkstra
{
   public Dijkstra()
   {
   }

   public List<Vertex> DijkstraShortestPath(
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
         }

         foreach (Edge edge in routersGraph[currentVertex])
         {
            if (Relax(currentVertex, edge.Destination, edge.EdgeWeight))
            {
               priorityQueue.Enqueue(edge.Destination, edge.Destination.Weight);
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
