namespace AdventOfCode.Utilities;

/// <summary>
/// Represents a 2D point node in a graph.
/// </summary>
/// <typeparam name="T">The type of value that the node holds.</typeparam>
public class Point2Node<T>
{
    public Point2 Point { get; }
    public T Value { get; }

    public Point2Node(Point2 point, T value)
    {
        Point = point;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Point} = {Value}";
    }
}