using System.Text;

namespace AdventOfCode.Utilities;

public class Grid<T>
{
    public YAxisDirection YAxisDirection { get; }
    private Point2 Offset { get; }
    public long Width { get; }
    public long Height { get; }

    private readonly T[,] _values;

    public Grid(long width, long height)
        : this(width, height, YAxisDirection.ZeroAtBottom)
    {
    }

    public Grid(long width, long height, YAxisDirection yAxisDirection)
        : this(width, height, Point2.Origin, yAxisDirection)
    {
    }

    public Grid(long width, long height, Point2 offset, YAxisDirection yAxisDirection)
    {
        YAxisDirection = yAxisDirection;
        Offset = offset;
        Width = width;
        Height = height;
        _values = new T[Width, Height];
    }

    public Grid(Point2 corner, Point2 oppositeCorner, YAxisDirection yAxisDirection)
    {
        YAxisDirection = yAxisDirection;
        var offset = new Point2(Math.Min(corner.X, oppositeCorner.X), Math.Min(corner.Y, oppositeCorner.Y));

        Offset = offset;
        Width = Math.Max(corner.X, oppositeCorner.X) - offset.X;
        Height = Math.Max(corner.X, oppositeCorner.X) - offset.Y;
        _values = new T[Width, Height];
    }

    public T this[long x, long y]
    {
        get => this[new Point2(x, y)];
        set => this[new Point2(x, y)] = value;
    }

    public Point2 Right(Point2 p) => p with { X = p.X + 1 };

    public Point2 Left(Point2 p) => p with { X = p.X - 1 };

    public Point2 Above(Point2 p) => p with { Y = YAxisDirection == YAxisDirection.ZeroAtBottom ? p.Y + 1 : p.Y - 1 };

    public Point2 Below(Point2 p) => p with { Y = YAxisDirection == YAxisDirection.ZeroAtBottom ? p.Y - 1 : p.Y + 1 };

    public T this[Point2 p]
    {
        get
        {
            if (!Contains(p))
            {
                throw new ArgumentOutOfRangeException(nameof(p));
            }

            return _values[p.X - Offset.X, p.Y - Offset.Y];
        }
        set
        {
            if (!Contains(p))
            {
                throw new ArgumentOutOfRangeException(nameof(p));
            }

            _values[p.X - Offset.X, p.Y - Offset.Y] = value;
        }
    }

    public IEnumerable<Point2> YSlice(long x)
    {
        for (var y = Offset.Y; y < Offset.Y + Height; y++)
        {
            yield return new Point2(x, y);
        }
    }

    public IEnumerable<Point2> XSlice(long y)
    {
        for (var x = Offset.X; x <  Offset.X + Width; x++)
        {
            yield return new Point2(x, y);
        }
    }

    public bool IsEdge(Point2 p)
    {
        return p.X == Offset.X || p.Y == Offset.Y || p.X == Offset.X + Width - 1 || p.Y == Offset.Y + Height - 1;
    }

    public IEnumerable<Point2> Points
    {
        get
        {
            for (var y = Offset.Y; y < Offset.Y + Height; y++)
            {
                for (var x = Offset.X; x < Offset.X + Width; x++)
                {
                    yield return new Point2(x, y);
                }
            }
        }
    }

    public bool Contains(Point2 p)
    {
        if (p.X < Offset.X || p.X >= Offset.X + Width)
        {
            return false;
        }

        if (p.Y < Offset.Y || p.Y >= Offset.Y + Height)
        {
            return false;
        }

        return true;
    }

    public override string ToString()
    {
        return ToString(" ");
    }

    public string ToString(string delimiter)
    {
        var sb = new StringBuilder();

        for (var y = Offset.Y; y < Offset.Y + Height; y++)
        {
            for (var x = Offset.X; x < Offset.X + Width; x++)
            {
                sb.Append(this[x, y]);
                if (x < Width - 1)
                {
                    sb.Append(delimiter);
                }
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}

public enum YAxisDirection
{
    ZeroAtBottom = 0,
    ZeroAtTop,
}