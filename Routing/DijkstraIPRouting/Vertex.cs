using System;

namespace DijkstraIPRouting;

public class Vertex
{
   /// <summary>
   /// The label of the vertex.
   /// </summary>
   public string Name { get; set; }

   /// <summary>
   /// The weight of the vertex after crossing an edge to it. Initially set to int.MaxValue.
   /// </summary>
   public int Weight { get; set; } = int.MaxValue;

   public Vertex(string name, int weight=int.MaxValue)
   {
      Name = name;
      Weight = weight;
   }

}
