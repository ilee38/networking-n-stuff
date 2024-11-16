using System.Collections;

namespace DijkstraIPRouting;

public interface IDijkstra
{
    List<Vertex> ShortestPath(Vertex sourceIp, Vertex destinationIp);
}
