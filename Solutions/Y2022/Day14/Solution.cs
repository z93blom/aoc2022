using AdventOfCode.Utilities;
using QuikGraph;

//using Spectre.Console;

namespace AdventOfCode.Y2022.Day14;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 14;
    public string GetName() => "Regolith Reservoir";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var grid = GetGrid(input);

        var sandCornsAtRest = 0;
        var sandHasFallenIntoVoid = false;
        while (!sandHasFallenIntoVoid)
        {
            // Start a new corn of sand at 500, 0
            var sand = new Point2(500, 0, grid.YAxisDirection);
            while (true)
            {
                while (grid.Contains(sand.Below) && grid[sand.Below] == '.')
                {
                    sand = sand.Below;
                }

                if (grid.IsEdge(sand))
                {
                    sandHasFallenIntoVoid = true;
                    break;
                }

                if (grid[sand.Below.Left] == '.')
                {
                    sand = sand.Below.Left;
                }
                else if (grid[sand.Below.Right] == '.')
                {
                    sand = sand.Below.Right;
                }
                else
                {
                    grid[sand] = 'o';
                    sandCornsAtRest++;
                    break;
                }
            }
        }

        //var output = getOutputFunction();
        //output.Write(grid.ToString(""));

        return sandCornsAtRest;
    }

    private static Grid<char> GetGrid(string input, bool addFloor = false)
    {
        var points = input.Lines()
            .Select(l =>
                l.Groups(@"(\d+),(\d+)")
                    .Select(g => new Point2(int.Parse(g[0].ToString()), int.Parse(g[1].ToString()), YAxisDirection.ZeroAtTop))
                    .ToArray())
            .ToArray();

        var minX = points.SelectMany(p => p).Select(p => p.X).Min() - 2;
        var maxX = points.SelectMany(p => p).Select(p => p.X).Max() + 2;
        const int minY = 0; 
        var maxY = points.SelectMany(p => p).Select(p => p.Y).Max();

        if (addFloor)
        {
            maxY += 2;
            minX = Math.Min(minX, 500 - maxY) - 2;
            maxX = Math.Max(maxX, 500 + maxY) + 2;
        }

        var width = maxX - minX + 1;
        var height = maxY - minY + 1;
        var grid = new Grid<char>(width, height, new Point2(minX, minY, YAxisDirection.ZeroAtTop));
        foreach (var point in grid.Points)
        {
            grid[point] = '.';
        }

        grid[500, 0] = '+';
        if (addFloor)
        {
            foreach (var point in grid.XSlice(maxY))
            {
                grid[point] = '#';
            }
        }

        foreach (var pointList in points)
        {
            // Add a rock at point[0]
            var current = pointList[0];
            var index = 1;
            grid[pointList[0]] = '#';
            while (index < pointList.Length)
            {
                var next = pointList[index];
                Func<Point2, Point2> movement;
                if (next.IsRightOf(current))
                {
                    movement = p => p.Right;
                }
                else if (next.IsLeftOf(current))
                {
                    movement = p => p.Left;
                }
                else if (next.IsAbove(current))
                {
                    movement = p => p.Above;
                }
                else //if (next.IsBelow(current))
                {
                    movement = p => p.Below;
                }

                while (current != next)
                {
                    current = movement(current);
                    grid[current] = '#';
                }


                index++;
            }
        }

        return grid;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var grid = GetGrid(input, true);

        var sandCornsAtRest = 0;
        var sandIsStopped = false;
        var sandOrigin = new Point2(500, 0, grid.YAxisDirection);
        while (!sandIsStopped)
        {
            // Start a new corn of sand at 500, 0
            var sand = sandOrigin;
            while (true)
            {
                while (grid.Contains(sand.Below) && grid[sand.Below] == '.')
                {
                    sand = sand.Below;
                }

                if (grid[sand.Below.Left] == '.')
                {
                    sand = sand.Below.Left;
                }
                else if (grid[sand.Below.Right] == '.')
                {
                    sand = sand.Below.Right;
                }
                else
                {

                    grid[sand] = 'o';
                    sandCornsAtRest++;
                    if (sand == sandOrigin)
                    {
                        sandIsStopped = true;
                    }

                    break;
                }
            }
        }

        //var output = getOutputFunction();
        //output.Write(grid.ToString(""));

        return sandCornsAtRest;
    }
}