using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using AdventOfCode.Utilities;

namespace AdventOfCode.Y2022.Day05;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 5;
    public string GetName() => "Supply Stacks";
#pragma warning disable 8602

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var lines = input.Lines(StringSplitOptions.None).ToArray();
        var stackLines = lines.TakeWhile(l => !string.IsNullOrEmpty(l))
            .Reverse()
            .ToArray();
        var names = stackLines.First();
        var stacksAndIndices = names.Select((c,i) => c != ' ' ? new { Index = i, Stack = new Stack<char>()} : null)
            .Where(a => a is not null)
            .ToArray();
        foreach (var sl in stackLines.Skip(1))
        {
            foreach (var si in stacksAndIndices)
            {
                if (si.Index < sl.Length && sl[si.Index] != ' ')
                {
                    si.Stack.Push(sl[si.Index]);
                }
            }
        }

        var stacks = stacksAndIndices.Select(si => si.Stack).ToArray();

        foreach (var op in lines.Skip(stackLines.Length + 1).Select(l => l.ParseNumbers().ToArray()))
        {
            var count = op[0];
            var from = op[1] - 1;
            var to = op[2] - 1;
            for (var i = 0; i < count; i++)
            {
                var c = stacks[from].Pop();
                stacks[to].Push(c);
            }
        }

        var result = new string(stacks.Select(s => s.Peek()).ToArray());

        return result;
    }

    static object PartTwo(string input)
    {
        var lines = input.Lines(StringSplitOptions.None).ToArray();
        var stackLines = lines.TakeWhile(l => !string.IsNullOrEmpty(l))
            .Reverse()
            .ToArray();
        var names = stackLines.First();
        var stacksAndIndices = names.Select((c, i) => c != ' ' ? new { Index = i, Stack = new Stack<char>() } : null)
            .Where(a => a != null)
            .ToArray();

        foreach (var sl in stackLines.Skip(1))
        {
            foreach (var si in stacksAndIndices)
            {
                if (si.Index < sl.Length && sl[si.Index] != ' ')
                {
                    si.Stack.Push(sl[si.Index]);
                }
            }
        }

        var stacks = stacksAndIndices.Select(si => si.Stack).ToArray();

        var queue = new Stack<char>();
        foreach (var op in lines.Skip(stackLines.Length + 1).Select(l => l.ParseNumbers().ToArray()))
        {
            var count = op[0];
            var from = op[1] - 1;
            var to = op[2] - 1;
            for (var i = 0; i < count; i++)
            {
                var c = stacks[from].Pop();
                queue.Push(c);
            }

            while (queue.Count > 0)
            {
                var c = queue.Pop();
                stacks[to].Push(c);
            }
        }

        var result = new string(stacks.Select(s => s.Peek()).ToArray());

        return result;
    }
#pragma warning restore 8602
}