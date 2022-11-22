using QuikGraph;

namespace AdventOfCode.Utilities;

public class Point2Edge<T> : IEdge<Point2Node<T>>
{
    public Point2Node<T> Source { get; }
    public Point2Node<T> Target { get; }

    public Point2Edge(Point2Node<T> source, Point2Node<T> target)
    {
        Source = source;
        Target = target;
    }

    public override string ToString()
    {
        return $"{Source} -> {Target}";
    }
}