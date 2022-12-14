using AdventOfCode.Utilities;
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
            var sand = new Point2(500, 0);
            while (true)
            {
                while (grid.Contains(sand.Above) && grid[sand.Above] == '.')
                {
                    sand = sand.Above;
                }

                if (grid.IsEdge(sand))
                {
                    sandHasFallenIntoVoid = true;
                    break;
                }

                if (grid[sand.Above.Left] == '.')
                {
                    sand = sand.Above.Left;
                }
                else if (grid[sand.Above.Right] == '.')
                {
                    sand = sand.Above.Right;
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
        // Figure out the min and max x, y
        //if (!input.Matches(@"(\d+),(\d+)", out var groups)) throw new Exception("Coding error");
        //var groups = input.Lines().Matches(@"(\d+),(\d+)").ToArray();
        var points = input.Lines()
            .Select(l =>
                l.Matches(@"(\d+),(\d+)")
                    .Select(m => new Point2(int.Parse(m.Groups[1].ToString()), int.Parse(m.Groups[2].ToString())))
                    .ToArray())
            .ToArray();

        var integers = input.Integers().ToArray();
        var minX = integers.Where((_, index) => index % 2 == 0).Min() - 2;
        var maxX = integers.Where((_, index) => index % 2 == 0).Max() + 2;
        const int minY = 0; // integers.Where((_, index) => index % 2 == 1).Min();
        var maxY = integers.Where((_, index) => index % 2 == 1).Max();

        if (addFloor)
        {
            maxY += 2;
            minX = Math.Min(minX, 500 - maxY) - 2;
            maxX = Math.Max(maxX, 500 + maxY) + 2;
        }

        var grid = new Grid<char>(new Point2(minX, minY), maxX - minX + 1, maxY - minY + 1);
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
                else if (next.IsAboveOf(current))
                {
                    movement = p => p.Above;
                }
                else //if (next.IsBelowOf(current))
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
        var sandOrigin = new Point2(500, 0);
        while (!sandIsStopped)
        {
            // Start a new corn of sand at 500, 0
            var sand = sandOrigin;
            while (true)
            {
                while (grid.Contains(sand.Above) && grid[sand.Above] == '.')
                {
                    sand = sand.Above;
                }

                if (grid[sand.Above.Left] == '.')
                {
                    sand = sand.Above.Left;
                }
                else if (grid[sand.Above.Right] == '.')
                {
                    sand = sand.Above.Right;
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