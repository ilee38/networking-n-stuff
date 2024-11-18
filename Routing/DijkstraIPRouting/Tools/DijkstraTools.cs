using System.Text.Json;
using NetFunctions;

namespace DijkstraIPRouting.Tools;

/// <summary>
/// A collection of utility methods to support the implementation of Dijkstra's shortest path algorithm applied to
/// IP routing.
/// </summary>
public static class DijkstraTools
{
    /// <summary>
    /// Creates a graph from the given JSON file
    /// </summary>
    /// <param name="routersInfoFilePath"></param>
    /// <returns>A graph represented as an adjacency list with a Dictionary, where the keys are the graph's vertices and
    /// the values are a list of edges for the vertex.
    /// In this case, the graph's vertices represent router IP addresses in a network.
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

    /// <summary>
    /// Dijkstra's algorithm creates a tree of shortest path from the destination vertex to the source vertex
    /// by assigning a parent corresponding to the vertex that leads to the shortest path.
    /// By traversing the tree in reverse order, we can obtain the shortest path from the source to the destination.
    /// </summary>
    /// <param name="destinationIp">The destination IP address (i.e. Vertex) from where to start tracing the source IP</param>
    /// <returns>An ordered list of the vertices forming the shortest path from source to destination.</returns>
    public static List<Vertex> GetPathFromTree(Vertex destinationIp)
    {
        var shortestPathFromSourceToDestination = new List<Vertex>();

        // If the destinationIp vertex has no parent after running Dijkstra's, then
        // there's no path from the source to the destination
        if (destinationIp.Parent == null)
        {
            return shortestPathFromSourceToDestination;
        }

        Vertex currentVertex = destinationIp;
        while (currentVertex.Parent != null)
        {
            shortestPathFromSourceToDestination.Add(currentVertex);
            currentVertex = currentVertex.Parent;
        }
        // Add the source vertex to the path
        shortestPathFromSourceToDestination.Add(currentVertex);

        return shortestPathFromSourceToDestination.Reverse<Vertex>().ToList();
    }

    /// <summary>
    /// Verifies if two IP Addresses (represented as a graph Vertex) are part of the same subnet.
    /// The Vertex class contains the IP address and the subnet mask as properties. These are needed to verify if they
    /// belong to the same subnet.
    /// </summary>
    /// <param name="sourceIp"></param>
    /// <param name="destinationIp"></param>
    /// <returns>Returns true if the IPs belong to the same subnet, false otherwise.</returns>
    public static bool SameNetwork(Vertex sourceIp, Vertex destinationIp)
    {
        var sourceIpValue = NetFunctionsTools.Ipv4ToValue(sourceIp.Name);
        var sourceIpMask = NetFunctionsTools.GetSubnetMaskValue(sourceIp.NetMask);
        var destinationIpValue = NetFunctionsTools.Ipv4ToValue(destinationIp.Name);
        var destinationIpMask = NetFunctionsTools.GetSubnetMaskValue(destinationIp.NetMask);

        return (sourceIpValue & sourceIpMask) == (destinationIpValue & destinationIpMask);
    }

    /// <summary>
    /// Finds the router's IP from the given graph, corresponding to the IP address in a subnet.
    /// </summary>
    /// <param name="ip"></param>
    /// <param name="routersGraph"></param>
    /// <returns>The router's Vertex in the graph corresponding to the given subnet IP address, or null if the address
    /// doesn't belong to any router in the given graph</returns>
    public static Vertex? GetRouterFromIp(Vertex ip, Dictionary<Vertex, List<Edge>> routersGraph)
    {
        foreach (Vertex router in routersGraph.Keys)
        {
            if (NetFunctionsTools.IPsSameSubnet(ip.Name, router.Name, router.NetMask))
            {
                return router;
            }
        }
        return null;
    }
}
