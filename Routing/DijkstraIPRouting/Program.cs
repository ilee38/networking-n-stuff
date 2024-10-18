using System.Text.Json;

namespace DijkstraIPRouting;

public class Program
{
   public static void Main()
   {
      string routersInfoFilePath = "routerinfo.json";
      var routersGraph = InitializeGraph(routersInfoFilePath);

      // Test some paths from the json file
      Vertex sourceIp = routersGraph.Keys.Where(v => v.Name == "10.34.209.1").First();
      Vertex destinationIp = routersGraph.Keys.Where(v => v.Name == "10.34.166.1").First();

      var allShortestPathsSet = Dijkstra.DijkstraShortestPath(routersGraph, sourceIp, destinationIp);

      // If the destinationIp vertex has no parent after running Dijkstra's, then
      // there's no path from the source to the destination
      if (destinationIp.Parent == null)
      {
         Console.WriteLine($"No path from {sourceIp.Name} to {destinationIp.Name} exists:");
         Console.WriteLine("[]");
         return;
      }

      var shortestPathFromSourceToDestination = new List<Vertex>();
      Vertex currentVertex = destinationIp;
      while (currentVertex.Parent != null)
      {
         shortestPathFromSourceToDestination.Add(currentVertex);
         currentVertex = currentVertex.Parent;
      }
      // Add the source vertex to the path
      shortestPathFromSourceToDestination.Add(sourceIp);

      Console.WriteLine($"Shortest path from {sourceIp.Name} to {destinationIp.Name}:");
      for (int i = shortestPathFromSourceToDestination.Count - 1; i >= 0; i--)
      {
         Console.WriteLine($"{shortestPathFromSourceToDestination[i].Name}");
      }

   }

   private static Dictionary<Vertex, List<Edge>> InitializeGraph(string routersInfoFilePath)
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
         var currentRouterConnections = currentRouter.GetProperty("connections").EnumerateObject();

         // Create a new vertex for the current router and add it to the graph
         Vertex sourceVertex = new(currentRouterIP);
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
