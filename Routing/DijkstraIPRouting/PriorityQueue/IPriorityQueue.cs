namespace DijkstraIPRouting;

public interface IPriorityQueue<T>
{
    /// <summary>
    /// Adds an element to the priority queue with the given priority.
    /// </summary>
    /// <param name="key">Represents the priority of the element.</param>
    /// <param name="value">The element to be inserted.</param>
    /// <returns>A Locator record containing its position within the underlying collection.</returns>
    int Add(int key, Vertex value);

    /// <summary>
    /// Returns the element with the minimum priority in the queue, without removing it.
    /// </summary>
    /// <returns></returns>
    Vertex Min();

    /// <summary>
    /// Removes the element with the minimum priority in the queue.
    /// </summary>
    /// <returns>The element with the minimum priority in the queue</returns>
    Vertex ExtractMin();

    /// <summary>
    /// Returns true if the queue is empty, false otherwise.
    /// </summary>
    /// <returns></returns>
    bool IsEmpty();

    /// <summary>
    /// Returns the number of elements in the queue.
    /// </summary>
    /// <returns></returns>
    int Count();
}
