using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Utilities;

public record struct Point2(long X, long Y, YAxisDirection YAxis)
{
    public Point2 Left => this with { X = X - 1  };
    public Point2 Right => this with { X = X + 1 };
    public Point2 Above => this with { Y = YAxis == YAxisDirection.ZeroAtBottom ? Y + 1 :Y - 1 };
    public Point2 Below => this with { Y = YAxis == YAxisDirection.ZeroAtBottom ? Y - 1 : Y + 1 };

    public bool IsLeftOf(Point2 p) => X < p.X;
    public bool IsRightOf(Point2 p) => X > p.X;
    public bool IsAbove(Point2 p) => YAxis == YAxisDirection.ZeroAtBottom ? Y > p.Y : Y < p.Y;
    public bool IsBelow(Point2 p) => YAxis == YAxisDirection.ZeroAtBottom ? Y < p.Y : Y > p.Y;

    public IEnumerable<Point2> OrthogonalPoints
    {
        get
        {
            yield return Above;
            yield return Right;
            yield return Below;
            yield return Left;
        }
    }

    public IEnumerable<Point2> AdjacentPoints
    {
        get
        {
            yield return Above;
            yield return Above.Right;
            yield return Right;
            yield return Below.Right;
            yield return Below;
            yield return Below.Left;
            yield return Left;
            yield return Above.Left;
        }
    }
}