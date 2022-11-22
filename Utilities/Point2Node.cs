namespace AdventOfCode.Utilities;

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