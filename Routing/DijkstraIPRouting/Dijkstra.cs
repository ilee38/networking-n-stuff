using System;
using System.Collections.Generic;
using DijkstraIPRouting.Tools;

namespace DijkstraIPRouting;

public class Dijkstra : IDijkstra
{
    private readonly Dictionary<Vertex, List<Edge>> _routersGraph;

    public Dijkstra(Dictionary<Vertex, List<Edge>> routersGraph)
    {
        _routersGraph = routersGraph;
    }

   public List<Vertex> ShortestPath(Vertex sourceIp, Vertex destinationIp)
   {
      if (DijkstraTools.SameNetwork(sourceIp, destinationIp))
      {
         return [];
      }

      Vertex? sourceRouter = DijkstraTools.GetRouterFromIp(sourceIp, _routersGraph);
      Vertex? destinationRouter = DijkstraTools.GetRouterFromIp(destinationIp, _routersGraph);

      var allShortestPaths = new List<Vertex>();
      var priorityQueue = InitializeQueue(sourceRouter!);

      while (priorityQueue.Count > 0)
      {
         var currentVertex = ExtractLowestPriorityVertex(priorityQueue);
         allShortestPaths.Add(currentVertex);
         if (currentVertex.Name == destinationRouter!.Name)
         {
            break;
         }

         foreach (Edge edge in _routersGraph[currentVertex])
         {
            var adjacentVertex = _routersGraph.Keys.Where(v => v.Name == edge.Destination.Name).First();
            if (Relax(currentVertex, adjacentVertex, edge.EdgeWeight))
            {
               UpdatePriorityInQueue(priorityQueue, adjacentVertex);
            }
         }
      }
      List<Vertex> path = DijkstraTools.GetPathFromTree(destinationRouter!);

      return path;
   }

   /// <summary>
   /// Initializes the priority queue with initial wieghts for each vertex in the graph.
   /// Ideally, the priority queue should be implemented as a min-heap where the priorities can be updated
   /// as the algorithm progresses. However, the ProirityQueue class in C# does not support updating priorities.
   /// Therefore, we will use a List<Vertex> to store the vertices with their weight estimates (priorities).
   /// </summary>
   /// <param name="sourceIp"></param>
   /// <returns></returns>
   private List<Vertex> InitializeQueue(Vertex sourceIp)
   {
      var Q = new List<Vertex>();
      foreach (Vertex vertex in _routersGraph.Keys)
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
}
