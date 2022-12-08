using System.Text;

namespace AdventOfCode.Utilities;

public class Grid<T>
{
    public long Width { get; }
    public long Height { get; }
    private readonly T[,] _values;

    public Grid(long width, long height)
    {
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

            return _values[p.X, p.Y];
        }
        set
        {
            if (!Contains(p))
            {
                throw new ArgumentOutOfRangeException(nameof(p));
            }

            _values[p.X, p.Y] = value;
        }
    }

    public IEnumerable<Point2> YSlice(long x)
    {
        for (var y = 0; y < Height; y++)
        {
            yield return new Point2(x, y);
        }
    }

    public IEnumerable<Point2> XSlice(long y)
    {
        for (var x = 0; x < Width; x++)
        {
            yield return new Point2(x, y);
        }
    }

    public bool IsEdge(Point2 p)
    {
        return p.X == 0 || p.Y == 0 || p.X == Width - 1 || p.Y == Height - 1;
    }

    public IEnumerable<Point2> Points
    {
        get
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    yield return new Point2(x, y);
                }
            }
        }
    }

    public bool Contains(Point2 p)
    {
        if (p.X < 0 || p.X >= Width)
        {
            return false;
        }

        if (p.Y < 0 || p.Y >= Height)
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

        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
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