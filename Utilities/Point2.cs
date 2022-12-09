namespace AdventOfCode.Utilities;

public record Point2(long X, long Y)
{
    public IEnumerable<Point2> OrthogonalPoints
    {
        get
        {
            yield return Below;
            yield return Right;
            yield return Above;
            yield return Left;
        }
    }

    public IEnumerable<Point2> AdjacentPoints
    {
        get
        {
            yield return Below;
            yield return Below.Right;
            yield return Right;
            yield return Above.Right;
            yield return Above;
            yield return Above.Left;
            yield return Left;
            yield return Below.Left;
        }
    }
    public Point2 Left => this with { X = X - 1 };
    public Point2 Right => this with { X = X + 1 };
    public Point2 Above => this with { Y = Y + 1 };
    public Point2 Below => this with { Y = Y - 1 };

    public bool IsLeftOf(Point2 p) => X < p.X;
    public bool IsRightOf(Point2 p) => X > p.X;
    public bool IsAboveOf(Point2 p) => Y > p.Y;
    public bool IsBelowOf(Point2 p) => Y < p.Y;

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}