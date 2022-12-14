namespace AdventOfCode.Utilities;

public record struct Point2(long X, long Y)
{
    public IEnumerable<Point2> OrthogonalPoints
    {
        get
        {
            yield return this with { Y = Y - 1 };
            yield return this with { X = X + 1 };
            yield return this with { Y = Y + 1 };
            yield return this with { X = X - 1 };
        }
    }

    public IEnumerable<Point2> AdjacentPoints
    {
        get
        {
            yield return this with { Y = Y - 1 };
            yield return new Point2(X + 1, Y - 1);
            yield return this with { X = X + 1 };
            yield return new Point2(X + 1, Y + 1);
            yield return this with { Y = Y + 1 };
            yield return new Point2(X - 1, Y + 1);
            yield return this with { X = X - 1 };
            yield return new Point2(X - 1, Y - 1);
        }
    }

    public static Point2 Origin = new(0, 0);

    public Point2Relative AsRelative(YAxisDirection yAxisDirection) => new(this, yAxisDirection);

    public Point2Relative AsRelative() => AsRelative(YAxisDirection.ZeroAtBottom);

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}