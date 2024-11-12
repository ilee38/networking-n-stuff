namespace DijkstraIPRouting.PriorityQueue;

/// <summary>
/// Represents a generic Item in the priority queue.
/// </summary>
public class QueueItem<T>
{
    /// <summary>
    /// The key represents the priority of the item.
    /// </summary>
    public int Key { get; set; }

    /// <summary>
    /// The actual object's value
    /// </summary>
    public T Value { get; set; }

    /// <summary>
    /// Represents the location of the item in the collection. I.e. the index in a List
    /// </summary>
    public int Locator { get; set; }
}
