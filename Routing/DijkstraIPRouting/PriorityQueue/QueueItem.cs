namespace DijkstraIPRouting.PriorityQueue;

/// <summary>
/// Represents a generic Item in the priority queue.
/// </summary>
public class QueueItem<T>
{
    /// <summary>
    /// The key represents the priority of the item in the queue.
    /// </summary>
    public int Key { get; set; }

    /// <summary>
    /// The actual object's value
    /// </summary>
    public T Value { get; set; }
}
