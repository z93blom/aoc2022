using System.Text;

namespace AdventOfCode.Utilities;

public class Grid<T>
{
    public YAxisDirection YAxisDirection => Offset.YAxis;
    private Point2 Offset { get; }
    public long Width { get; }
    public long Height { get; }

    private readonly T[,] _values;

    public Grid(long width, long height)
        : this(width, height, YAxisDirection.ZeroAtBottom)
    {
    }

    public Grid(long width, long height, YAxisDirection yAxisDirection)
        : this(width, height, new Point2(0, 0, yAxisDirection))
    {
    }

    public Grid(long width, long height, Point2 offset)
    {
        Offset = offset;
        Width = width;
        Height = height;
        _values = new T[Width, Height];
    }

    public T this[long x, long y]
    {
        get => this[new Point2(x, y, YAxisDirection)];
        set => this[new Point2(x, y, YAxisDirection)] = value;
    }

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
            yield return new Point2(x, y, YAxisDirection);
        }
    }

    public IEnumerable<Point2> XSlice(long y)
    {
        for (var x = Offset.X; x <  Offset.X + Width; x++)
        {
            yield return new Point2(x, y, YAxisDirection);
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
                    yield return new Point2(x, y, YAxisDirection);
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