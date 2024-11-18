using DijkstraIPRouting;
using DijkstraIPRouting.Tools;

namespace DijkstraIPRoutingTests;

public class DijkstraPQTests
{
    private const string RoutersFilePath = "routerinfo.json";
    private readonly DijkstraPQ _dijkstra = new DijkstraPQ(DijkstraTools.InitializeGraph(RoutersFilePath));
    
    [Fact]
    public void ShortestPathTests()
    {
        // Arrange
        var sourceIp = new Vertex("10.34.209.229", netMask: "/24");
        var destinationIp = new Vertex("10.34.166.26", netMask: "/24");

        // Act
        var shortestPath = _dijkstra.ShortestPath(sourceIp, destinationIp);

        // Assert
        Assert.True(shortestPath.Count == 5);
        Assert.True(shortestPath[0].Name == "10.34.209.1");
        Assert.True(shortestPath[1].Name == "10.34.91.1");
        Assert.True(shortestPath[2].Name == "10.34.46.1");
        Assert.True(shortestPath[3].Name == "10.34.98.1");
        Assert.True(shortestPath[4].Name == "10.34.166.1");
    }
}