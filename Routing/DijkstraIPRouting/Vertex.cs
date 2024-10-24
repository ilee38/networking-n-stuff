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
   public int Weight { get; set; }

   /// <summary>
   ///  The parent vertex of the current vertex in the shortest path.
   /// </summary>
   public Vertex? Parent { get; set; }

   /// <summary>
   /// The subnet mask of the vertex representing a router node.
   /// </summary>
   public string NetMask { get; set; }

   public Vertex(string name, int weight=int.MaxValue, Vertex? parent=null, string netMask="")
   {
      Name = name;
      Weight = weight;
      Parent = parent;
      NetMask = netMask;
   }
}
