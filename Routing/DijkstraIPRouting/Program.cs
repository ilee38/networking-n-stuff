using System.Text.Json;

namespace DijkstraIPRouting;

public class Program
{
   public static void Main()
   {
      string routersInfoFilePath = "routerinfo.json";
      var routersGraph = InitializeGraph(routersInfoFilePath);

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
