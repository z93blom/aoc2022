using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Utilities;

public record struct Point2Relative(Point2 P, YAxisDirection YAxis)
{
    public Point2Relative Left => this with { P = P with { X = P.X - 1 } };
    public Point2Relative Right => this with { P = P with { X = P.X + 1 } };
    public Point2Relative Above => this with { P = P with { Y = YAxis == YAxisDirection.ZeroAtBottom ? P.Y + 1 : P.Y - 1 } };
    public Point2Relative Below => this with { P = P with { Y = YAxis == YAxisDirection.ZeroAtBottom ? P.Y - 1 : P.Y + 1 } };

    public bool IsLeftOf(Point2 p) => P.X < p.X;
    public bool IsRightOf(Point2 p) => P.X > p.X;
    public bool IsAbove(Point2 p) => YAxis == YAxisDirection.ZeroAtBottom ? P.Y > p.Y : P.Y < p.Y;
    public bool IsBelow(Point2 p) => YAxis == YAxisDirection.ZeroAtBottom ? P.Y < p.Y : P.Y > p.Y;

    public static implicit operator Point2(Point2Relative pr) => pr.P;
}