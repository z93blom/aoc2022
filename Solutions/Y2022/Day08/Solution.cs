using AdventOfCode.Utilities;
using Spectre.Console;
using System.Drawing;

namespace AdventOfCode.Y2022.Day08;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 8;
    public string GetName() => "Treetop Tree House";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var lines = input.Lines().ToArray();
        var width = lines[0].Length;
        var height = lines.Length();
        var grid = new Grid<int>(width, height);
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = lines[y][x] - '0';
            }
        }
        var visibleTrees = new HashSet<Point2>();

        foreach (var p in grid.Points)
        {
            if (IsVisible(p, grid))
            {
                visibleTrees.Add(p);
            }
        }

        return visibleTrees.Count;
    }

    private static bool IsVisible(Point2 point, Grid<int> grid)
    {
        var h = grid[point];
        var xSlice = grid.XSlice(point.Y).ToArray();
        var ySlice = grid.YSlice(point.X).ToArray();
        var isVisible = false;
        isVisible = isVisible || ySlice.Where(p => p.Y < point.Y).All(p => grid[p] < h);
        isVisible = isVisible || ySlice.Where(p => p.Y > point.Y).All(p => grid[p] < h);
        isVisible = isVisible || xSlice.Where(p => p.X < point.X).All(p => grid[p] < h);
        isVisible = isVisible || xSlice.Where(p => p.X > point.X).All(p => grid[p] < h);

        return isVisible;
    }

    static object PartTwo(string input)
    {
        var lines = input.Lines().ToArray();
        var width = lines[0].Length;
        var height = lines.Length();
        var grid = new Grid<int>(width, height);
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                grid[x, y] = lines[y][x] - '0';
            }
        }

        var topScenicScore = grid.Points.Max(p => ScenicScore(p, grid));
        return topScenicScore;
    }

    private static long ScenicScore(Point2 point, Grid<int> grid)
    {

        if (grid.IsEdge(point))
        {
            return 0;
        }

        var leftVisible = VisibleTrees(grid, point, p => p.AsRelative(grid.YAxisDirection).Left);

        var rightVisible = VisibleTrees(grid, point, p => p.AsRelative(grid.YAxisDirection).Right);

        var topVisible = VisibleTrees(grid, point, p => p.AsRelative(grid.YAxisDirection).Above);

        var bottomVisible = VisibleTrees(grid, point, p => p.AsRelative(grid.YAxisDirection).Below);

        return leftVisible * rightVisible * topVisible * bottomVisible;
    }

    private static int VisibleTrees(Grid<int> grid, Point2 start, Func<Point2, Point2> stepper)
    {
        var visible = 0;
        var p = start;
        var h = grid[start];
        while (true)
        {
            var next = stepper(p);
            if (grid.Contains(next))
            {
                visible++;
            }

            // Should we continue?
            if (grid[next] >= h || grid.IsEdge(next))
            {
                break;
            }

            p = next;
        }

        return visible;
    }
}