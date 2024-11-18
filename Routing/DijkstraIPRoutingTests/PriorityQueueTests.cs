using DijkstraIPRouting;
using DijkstraIPRouting.PriorityQueue;

namespace DijkstraIPRoutingTests;

public class PriorityQueueTests
{
    private readonly ChangeablePriorityQueue _priorityQueue = new();
    private readonly List<int> _priorities = new() { 4, 5, 6, 15, 9, 7, 20, 16 };
    private readonly List<string> _names = new() { "C", "A", "Z", "K", "F", "Q", "B", "X" };

    [Fact]
    public void AddElementsTests()
    {
        // Arrange
        InitializePriorityQueue();

        // Assert
        Assert.True(_priorityQueue.Count() == _priorities.Count);
    }

    [Fact]
    public void MinElementTests()
    {
        // Arrange
        InitializePriorityQueue();

        // Act
        var minElement = _priorityQueue.Min();

        // Assert
        Assert.Equal(4, minElement.Weight);
        Assert.Equal("C", minElement.Name);
    }

    [Fact]
    public void ExtractMinElementTests()
    {
        // Arrange
        InitializePriorityQueue();

        // Act
        var minElement = _priorityQueue.ExtractMin();
        var newMinElement = _priorityQueue.Min();

        // Assert
        Assert.Equal(4, minElement.Weight);
        Assert.Equal("C", minElement.Name);
        Assert.Equal(5, newMinElement.Weight);
        Assert.Equal("A", newMinElement.Name);
        Assert.Equal(_priorities.Count - 1, _priorityQueue.Count());
    }

    [Fact]
    public void UpdateElementTests()
    {
        // Arrange
        InitializePriorityQueue();

        // Act
        var locator = 6;
        var newLocator = _priorityQueue.Update(locator, 2);
        var newMinElement = _priorityQueue.Min();

        // Assert
        Assert.Equal(2, newMinElement.Weight);
        Assert.Equal("B", newMinElement.Name);
        Assert.NotEqual(locator, newLocator);
        Assert.Equal(0, newLocator);
    }

    private void InitializePriorityQueue()
    {
        for (int i = 0; i < _priorities.Count; i++)
        {
            _priorityQueue.Add(_priorities[i], new Vertex(_names[i], _priorities[i]));
        }
    }
}
