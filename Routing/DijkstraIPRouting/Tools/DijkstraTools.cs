using System.Text.Json;

namespace DijkstraIPRouting.Tools;

public class DijkstraTools
{
    /// <summary>
    /// Creates a graph from the given json file
    /// </summary>
    /// <param name="routersInfoFilePath"></param>
    /// <returns>A graph represented as an adjacency list with a Dictionary, where the keys are the graph's vertices and
    /// the values are a list of edges for the vertex.
    /// </returns>
    public static Dictionary<Vertex, List<Edge>> InitializeGraph(string routersInfoFilePath)
    {
        var routersGraph = new Dictionary<Vertex, List<Edge>>();
        var jsonString = File.ReadAllText(routersInfoFilePath);

        using JsonDocument doc = JsonDocument.Parse(jsonString);
        JsonElement root = doc.RootElement;
        var routers = root.GetProperty("routers").EnumerateObject();
        while (routers.MoveNext())
        {
            var currentRouter = routers.Current.Value;
            var currentRouterIP = routers.Current.Name;
            var currentRouterNetMask = currentRouter.GetProperty("netmask").GetString();
            var currentRouterConnections = currentRouter.GetProperty("connections").EnumerateObject();

            // Create a new vertex for the current router and add it to the graph
            Vertex sourceVertex = new(currentRouterIP, netMask: currentRouterNetMask!);
            routersGraph.Add(sourceVertex, new List<Edge>());

            // Add edges to the graph for each connection from the current router
            while (currentRouterConnections.MoveNext())
            {
                var connection = currentRouterConnections.Current.Value;
                var connectionIP = currentRouterConnections.Current.Name;
                var connectionWeight = connection.GetProperty("ad").GetInt32();
                Vertex destinationVertex = new(connectionIP);
                Edge e = new (sourceVertex, destinationVertex, connectionWeight);

                // Add the edge to the graph
                routersGraph[sourceVertex].Add(e);
            }
        }
        return routersGraph;
    }
}
