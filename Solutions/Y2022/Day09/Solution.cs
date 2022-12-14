using System.Drawing;
using System.Security.Principal;
using System.Text;
using AdventOfCode.Utilities;
using Newtonsoft.Json.Serialization;
using static QuikGraph.Algorithms.Assignment.HungarianAlgorithm;

namespace AdventOfCode.Y2022.Day09;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 9;
    public string GetName() => "Rope Bridge";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, emptyOutput);
        yield return PartTwo(input);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var steps = input.Lines().ToArray();
        var enableDebug = steps.Length < 20;
        var headPos = new Point2(0, 0, YAxisDirection.ZeroAtBottom);
        var tailPos = new Point2(0, 0, YAxisDirection.ZeroAtBottom);
        HashSet<Point2> tailVisits = new() { tailPos };
        var sb = new StringBuilder();

        var output = getOutputFunction();
        if (enableDebug) output.WriteLine();
        foreach (var step in steps)
        {
            if (enableDebug) output.WriteLine($"== {step} ==");
            Func<Point2, Point2> repositionFunc;
            switch (step[0])
            {
                case 'L':
                    repositionFunc = p => p.Left;
                    break;
                case 'R':
                    repositionFunc = p => p.Right;
                    break;
                case 'U':
                    repositionFunc = p => p.Above;
                    break;
                case 'D':
                    repositionFunc = p => p.Below;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected head movement.");
            }

            var numberOfSteps = int.Parse(step[2..]);
            for(var i = 0; i < numberOfSteps; i++)
            {
                headPos = repositionFunc(headPos);
                tailPos = RecalculateTail(headPos, tailPos);
                tailVisits.Add(tailPos);


                if (enableDebug) DrawDebugGrid(headPos, tailPos, tailVisits, sb);
                if (enableDebug) output.WriteLine(sb.ToString());
                if (enableDebug) output.WriteLine();
                if (enableDebug) output.WriteLine();
                sb.Clear();
            }
        }

        return tailVisits.Count;
    }

    public static void DrawDebugGrid(Point2 head, Point2 tail, HashSet<Point2> tailVisits, StringBuilder sb)
    {
        var lowX = Math.Min(head.X, Math.Min(tail.X, tailVisits.Min(p => p.X)));
        var highX = Math.Max(head.X, Math.Max(tail.X, tailVisits.Max(p => p.X)));
        var lowY = Math.Min(head.Y, Math.Min(tail.Y, tailVisits.Min(p => p.Y)));
        var highY = Math.Max(head.Y, Math.Max(tail.Y, tailVisits.Max(p => p.Y)));
        for (var y = highY; y >= lowY; y--)
        {
            for (var x = lowX; x <= highX; x++)
            {
                var p = new Point2(x, y, YAxisDirection.ZeroAtBottom);
                if (Equals(head, p))
                {
                    sb.Append('H');
                }
                else if (Equals(tail, p))
                {
                    sb.Append('T');
                }
                else if (p is { X: 0, Y: 0 })
                {
                    sb.Append('s');
                }
                else if (tailVisits.Contains(p))
                {
                    sb.Append('#');
                }
                else
                {
                    sb.Append('.');
                }
            }

            sb.AppendLine();
        }
    }

    public static Point2 RecalculateTail(Point2 p1, Point2 p2)
    {
        if (p1 == p2 || p1.AdjacentPoints.Contains(p2))
        {
            // The tail is already touching the head. Does not move.
            return p2;
        }

        if (p1.IsLeftOf(p2) && p1.IsAbove(p2))
        {
            return p2.Left.Above;
        }

        if (p1.IsLeftOf(p2) && p1.IsBelow(p2))
        {
            return p2.Left.Below;
        }

        if (p1.IsRightOf(p2) && p1.IsAbove(p2))
        {
            return p2.Right.Above;
        }

        if (p1.IsRightOf(p2) && p1.IsBelow(p2))
        {
            return p2.Right.Below;
        }

        if (p1.IsRightOf(p2))
        {
            return p2.Right;
        }

        if (p1.IsLeftOf(p2))
        {
            return p2.Left;
        }

        if (p1.IsAbove(p2))
        {
            return p2.Above;
        }

        if (p1.IsBelow(p2))
        {
            return p2.Below;
        }

        throw new Exception("Something wrong!");
    }

    static object PartTwo(string input)
    {
        var steps = input.Lines().ToArray();
        var knots = new Point2[10];
        HashSet<Point2> tailVisits = new() { knots[9] };

        foreach (var step in steps)
        {
            Func<Point2, Point2> repositionFunc;
            switch (step[0])
            {
                case 'L':
                    repositionFunc = p => p.Left;
                    break;
                case 'R':
                    repositionFunc = p => p.Right;
                    break;
                case 'U':
                    repositionFunc = p => p.Above;
                    break;
                case 'D':
                    repositionFunc = p => p.Below;
                    break;
                default:
                    throw new InvalidOperationException("Unexpected head movement.");
            }

            var numberOfSteps = int.Parse(step[2..]);
            for (var i = 0; i < numberOfSteps; i++)
            {
                knots[0] = repositionFunc(knots[0]);
                for (var j = 1; j < knots.Length; j++)
                    knots[j] = RecalculateTail(knots[j - 1], knots[j]);

                tailVisits.Add(knots[9]);
            }
        }

        return tailVisits.Count;
    }
}