using HtmlAgilityPack;

namespace AdventOfCode.Utilities;

public readonly struct Point2
{
    public long X { get; }
    public long Y { get; }

    public Point2(long x, long y)
    {
        X = x;
        Y = y;
    }

    public IEnumerable<Point2> OrthogonalPoints
    {
        get
        {
            yield return new Point2(X, Y - 1);
            yield return new Point2(X + 1, Y);
            yield return new Point2(X, Y + 1);
            yield return new Point2(X - 1, Y);
        }
    }

    public IEnumerable<Point2> AdjacentPoints
    {
        get
        {
            yield return new Point2(X,     Y - 1);
            yield return new Point2(X + 1, Y - 1);
            yield return new Point2(X + 1, Y);
            yield return new Point2(X + 1, Y + 1);
            yield return new Point2(X,     Y + 1);
            yield return new Point2(X - 1, Y + 1);
            yield return new Point2(X - 1, Y);
            yield return new Point2(X - 1, Y - 1);
        }
    }
    public Point2 Left => new Point2(X - 1, Y);
    public Point2 Right => new Point2(X + 1, Y);
    public Point2 Above => new Point2(X, Y - 1);
    public Point2 Below => new Point2(X, Y + 1);

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}