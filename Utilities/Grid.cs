using System.Text;

namespace AdventOfCode.Utilities;

public class Grid<T>
{
    private Point2 LowerLeft { get; } = new Point2(0, 0);
    public long Width { get; }
    public long Height { get; }
    private readonly T[,] _values;

    public Grid(long width, long height)
    {
        Width = width;
        Height = height;
        _values = new T[width, height];
    }

    public Grid(Point2 lowerLeft, long width, long height)
    {
        LowerLeft = lowerLeft;
        Width = width;
        Height = height;
        _values = new T[width, height];
    }

    public T this[long x, long y]
    {
        get => this[new Point2(x, y)];
        set => this[new Point2(x, y)] = value;
    }

    public T this[Point2 p]
    {
        get
        {
            if (!Contains(p))
            {
                throw new ArgumentOutOfRangeException(nameof(p));
            }

            return _values[p.X - LowerLeft.X, p.Y - LowerLeft.Y];
        }
        set
        {
            if (!Contains(p))
            {
                throw new ArgumentOutOfRangeException(nameof(p));
            }

            _values[p.X - LowerLeft.X, p.Y - LowerLeft.Y] = value;
        }
    }

    public IEnumerable<Point2> YSlice(long x)
    {
        for (var y = LowerLeft.Y; y < LowerLeft.Y + Height; y++)
        {
            yield return new Point2(x, y);
        }
    }

    public IEnumerable<Point2> XSlice(long y)
    {
        for (var x = LowerLeft.X; x <  LowerLeft.X + Width; x++)
        {
            yield return new Point2(x, y);
        }
    }

    public bool IsEdge(Point2 p)
    {
        return p.X == LowerLeft.X || p.Y == LowerLeft.Y || p.X == LowerLeft.X + Width - 1 || p.Y == LowerLeft.Y + Height - 1;
    }

    public IEnumerable<Point2> Points
    {
        get
        {
            for (var y = LowerLeft.Y; y < LowerLeft.Y + Height; y++)
            {
                for (var x = LowerLeft.X; x < LowerLeft.X + Width; x++)
                {
                    yield return new Point2(x, y);
                }
            }
        }
    }

    public bool Contains(Point2 p)
    {
        if (p.X < LowerLeft.X || p.X >= LowerLeft.X + Width)
        {
            return false;
        }

        if (p.Y < LowerLeft.Y || p.Y >= LowerLeft.Y + Height)
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

        for (var y = LowerLeft.Y; y < LowerLeft.Y + Height; y++)
        {
            for (var x = LowerLeft.X; x < LowerLeft.X + Width; x++)
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