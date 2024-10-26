using DijkstraIPRouting;
using DijkstraIPRouting.Tools;

namespace DijkstraIPRoutingTests;

public class DijkstraIPRoutingTests
{
    private const string RoutersFilePath = "routerinfo.json";
    private readonly Dictionary<Vertex, List<Edge>> _graph = DijkstraTools.InitializeGraph(RoutersFilePath);

    [Fact]
    public void DijkstraShortestPathTest()
    {
        // Arrange
        var sourceIp = new Vertex("10.34.209.229", netMask: "/24");
        var destinationIp = new Vertex("10.34.166.26", netMask: "/24");

        // Act
        var shortestPath = Dijkstra.ShortestPath(_graph, sourceIp, destinationIp);

        // Assert
        Assert.True(shortestPath.Count == 5);
        Assert.True(shortestPath[0].Name == "10.34.209.1");
        Assert.True(shortestPath[1].Name == "10.34.91.1");
        Assert.True(shortestPath[2].Name == "10.34.46.1");
        Assert.True(shortestPath[3].Name == "10.34.98.1");
        Assert.True(shortestPath[4].Name == "10.34.166.1");
    }

    [Fact]
    public void DijkstraShortestPathNoPathTest()
    {
        // Arrange
        var sourceIp = new Vertex("10.34.52.187", netMask: "/24");
        var destinationIp = new Vertex("10.34.52.244", netMask: "/24");

        // Act
        var shortestPath = Dijkstra.ShortestPath(_graph, sourceIp, destinationIp);

        // Assert
        Assert.True(shortestPath.Count == 0);
    }
}
