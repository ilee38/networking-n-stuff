using System.Reflection;

namespace DijkstraIPRouting.PriorityQueue;

/// <summary>
/// Class representing a changeable priority queue. The type for the collection representing the queue is a `Tuple`,
/// where the first item in the tuple represents the index of the element within the collection, in this case a List, and
/// the second item is the Vertex element.
/// To represent the changeable priority queue, we use a Min-Heap implemented using an array-based structure (i.e. a List).
/// </summary>
public class ChangeablePriorityQueue : IPriorityQueue<QueueItem<Vertex>>
{
    /// <summary>
    /// A list to represent the min binary heap for the priority queue.
    /// </summary>
    private List<QueueItem<Vertex>> Queue { get; set; }

    public ChangeablePriorityQueue()
    {
        this.Queue = new();
    }

    /// <inheritdoc/>
    public int Add(int key, Vertex value)
    {
        var index = Queue.Count;
        var newElement = new QueueItem<Vertex>
        {
            Key = key,
            Value = value,
            Locator = index
        };

        // Add the element to the end of the list, then sift-up to fix the heap's order
        Queue.Add(newElement);
        SiftUp(index);

        return newElement.Locator;
    }

    /// <inheritdoc/>
    public Vertex Min()
    {
        if (Queue.Count == 0)
        {
            throw new Exception("Queue is empty.");
        }

        return Queue[0].Value;
    }

    /// <inheritdoc/>
    public Vertex ExtractMin()
    {
        if (Queue.Count == 0)
        {
            throw new Exception("Queue is empty.");
        }

        // Before removing, swap the root element (min) with the last element, then sift-down to fix the heap's order
        SwapElements(0, Queue.Count - 1);
        var minElement = Queue[^1].Value;
        Queue.RemoveAt(Queue.Count - 1);
        SiftDown(0);

        return minElement;
    }

    /// <inheritdoc/>
    public bool IsEmpty()
    {
        return Queue.Count == 0;
    }

    /// <inheritdoc/>
    public int Count()
    {
        return Queue.Count;
    }

    /// <summary>
    /// Updates the key and value for the element identified by its locator. The key and value are updated independently,
    /// i.e. the key can be updated while the value stays the same if the provided value is the same as the element currently.
    /// </summary>
    /// <param name="locator">The locator of the element to update.</param>
    /// <param name="key">The new key for the element.</param>
    /// <param name="value">The new value for the element.</param>
    public void Update(int locator, int key, Vertex? value=null)
    {
        var elementToUpdate = Queue[locator];
        elementToUpdate.Key = key;
        if (value != null)
        {
            elementToUpdate.Value = value;
        }
        else
        {
            elementToUpdate.Value.Weight = key;
        }

        Sift(locator);
    }

    /// <summary>
    /// Removes the element identified by its locator.
    /// </summary>
    /// <param name="locator"></param>
    public void Remove(int locator)
    {
        if (locator < 0 || locator >= Queue.Count)
        {
            throw new Exception("Invalid locator index.");
        }

        if (locator == Queue.Count - 1)
        {
            Queue.RemoveAt(locator);
        }
        else
        {
            SwapElements(locator, Queue.Count - 1);
            Queue.RemoveAt(Queue.Count - 1);
            Sift(locator);
        }
    }

    /// <summary>
    /// Performs the sift-up operation on the element at the given index to maintain the "heap order" property.
    /// </summary>
    /// <param name="index">Index of the element</param>
    /// <exception cref="NotImplementedException"></exception>
    private void SiftUp(int index)
    {
        var parentIndex = (index - 1) / 2;
        if (index > 0 && Queue[index].Key < Queue[parentIndex].Key)
        {
            SwapElements(index, parentIndex);
            SiftUp(parentIndex);
        }
    }

    /// <summary>
    /// Performs the sift-down operation on the element at the given index to maintain the "heap order" property.
    /// </summary>
    /// <param name="index"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void SiftDown(int index)
    {
        if (HasLeftChild(index))
        {
            var lowestChild = LeftChild(index);
            if (HasRightChild(index))
            {
                var rightChild = RightChild(index);
                if (rightChild.Key < lowestChild.Key)
                {
                    lowestChild = rightChild;
                }
            }
            if (lowestChild.Key < Queue[index].Key )
            {
                SwapElements(index, lowestChild.Locator);
                SiftDown(lowestChild.Locator);
            }
        }
    }

    /// <summary>
    /// Sifts an element up or down, depending on its priority to maintain the heap order property.
    /// </summary>
    /// <param name="index"></param>
    private void Sift(int index)
    {
        var parent = Queue[(index - 1) / 2];
        var currentElement = Queue[index];
        if (index > 0 && parent.Key > currentElement.Key)
        {
            SiftUp(index);
        }
        else
        {
            SiftDown(index);
        }
    }

    /// <summary>
    /// Swaps the elements at the two given indices
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    private void SwapElements(int i, int j)
    {
        (Queue[i], Queue[j]) = (Queue[j], Queue[i]);
        Queue[i].Locator = j;
        Queue[j].Locator = i;
    }

    private bool HasLeftChild(int index)
    {
        return (index * 2 + 1) <= Queue.Count - 1;
    }

    private bool HasRightChild(int index)
    {
        return (index * 2 + 2) <= Queue.Count - 1;
    }

    private QueueItem<Vertex> LeftChild(int index)
    {
        return Queue[index * 2 + 1];
    }

    private QueueItem<Vertex> RightChild(int index)
    {
        return Queue[index * 2 + 2];
    }
}
