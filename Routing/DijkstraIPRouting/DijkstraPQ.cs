using DijkstraIPRouting.PriorityQueue;
using DijkstraIPRouting.Tools;

namespace DijkstraIPRouting;

/// <summary>
/// Implementation of Dijkstra's shortest path algorithm for IP routing, but using the ChangeablePriorityQueue class as
/// the underlying data structure for the queue.
/// </summary>
public class DijkstraPQ : IDijkstra
{
    private readonly Dictionary<Vertex, List<Edge>> _routersGraph;

    /// <summary>
    ///  A dictionary mapping the graph vertices to its location in the priority queue (needed to efficiently perform updates
    /// to the priority queue elements).
    /// </summary>
    private readonly IDictionary<Vertex, int> vertexLocators = new Dictionary<Vertex, int>();

    public DijkstraPQ(Dictionary<Vertex, List<Edge>> routersGraph)
    {
        _routersGraph = routersGraph;
    }

    public List<Vertex> ShortestPath(Vertex sourceIp, Vertex destinationIp)
    {
        if (DijkstraTools.SameNetwork(sourceIp, destinationIp))
        {
            return [];
        }

        Vertex? sourceRouter = DijkstraTools.GetRouterFromIp(sourceIp, _routersGraph);
        Vertex? destinationRouter = DijkstraTools.GetRouterFromIp(destinationIp, _routersGraph);

        var allShortestPaths = new List<Vertex>();
        var priorityQueue = InitializeQueue(sourceRouter!);

        while (priorityQueue.Count() > 0)
        {
            var currentVertex = priorityQueue.ExtractMin();
            allShortestPaths.Add(currentVertex);

            if (currentVertex.Name == destinationRouter!.Name)
            {
                break;
            }

            foreach (var edge in _routersGraph[currentVertex])
            {
                var adjacentVertex = _routersGraph.Keys.Where(v => v.Name == edge.Destination.Name).First();
                if (Relax(currentVertex, adjacentVertex, edge.EdgeWeight))
                {
                    var vertexLocator = vertexLocators[adjacentVertex];
                    var newLocator = priorityQueue.Update(vertexLocator, adjacentVertex.Weight);
                    vertexLocators[adjacentVertex] = newLocator;
                }
            }
        }
        var path = DijkstraTools.GetPathFromTree(destinationRouter!);

        return path;
    }

    /// <summary>
    /// Performs edge relaxation and updates the vertex's weight.
    /// </summary>
    /// <param name="currentVertex"></param>
    /// <param name="adjacentVertex"></param>
    /// <param name="edgeWeight"></param>
    /// <returns>True if the weight was relaxed, false otherwise.</returns>
    private static bool Relax(Vertex currentVertex, Vertex adjacentVertex, int edgeWeight)
    {
        if (adjacentVertex.Weight > currentVertex.Weight + edgeWeight)
        {
            adjacentVertex.Weight = currentVertex.Weight + edgeWeight;
            adjacentVertex.Parent = currentVertex;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Initializes the priority queue with initial wieghts for each vertex in the graph.
    /// </summary>
    /// <param name="sourceRouter">The source node (in this case a router) in the graph.</param>
    /// <returns></returns>
    private ChangeablePriorityQueue InitializeQueue(Vertex sourceRouter)
    {
        var q = new ChangeablePriorityQueue();

        foreach (var vertex in _routersGraph.Keys)
        {
            vertex.Weight = vertex.Name.Equals(sourceRouter.Name) ? 0 : int.MaxValue;
            var vartexLocator = q.Add(vertex.Weight, vertex);
            vertexLocators.Add(vertex, vartexLocator);
        }
        return q;
    }
}
