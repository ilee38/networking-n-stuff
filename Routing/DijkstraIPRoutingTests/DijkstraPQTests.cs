using DijkstraIPRouting;
using DijkstraIPRouting.Tools;

namespace DijkstraIPRoutingTests;

public class DijkstraPQTests
{
    private const string RoutersFilePath = "routerinfo.json";
    private readonly DijkstraPQ _dijkstra = new DijkstraPQ(DijkstraTools.InitializeGraph(RoutersFilePath));
    
    
}