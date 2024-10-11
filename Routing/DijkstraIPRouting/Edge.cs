using System;

namespace DijkstraIPRouting;

public class Edge
{
   public Vertex Source { get; set; }
   public Vertex Destination { get; set; }
   public int EdgeWeight { get; set; }

   public Edge(Vertex source, Vertex destination, int edgeWeight)
   {
      Source = source;
      Destination = destination;
      EdgeWeight = edgeWeight;
   }
}
