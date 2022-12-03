using System.Diagnostics;
using AdventOfCode.Utilities;

namespace AdventOfCode.Y2022.Day03;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 3;
    public string GetName() => "Rucksack Reorganization";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var value = input.Lines().Select(l =>
            {
                var set = l[..(l.Length / 2)].ToHashSet();
                set.IntersectWith(l.Substring(l.Length / 2, l.Length / 2));
                return set.ToArray()[0];
            })
            .Select(Priority)
            .Sum();


        return value;
    }

    static int Priority(char c)
    {
        if (char.IsUpper(c))
        {
            return c - 'A' + 27;
        }

        return c - 'a' + 1;
    }

    static object PartTwo(string input)
    {
        var i = 0;
        var value = input.Lines()
            .GroupBy(l => i++/3)
            .Select(g => g.ToArray())
            .Select(g =>
            {
                var set = g[0].ToHashSet();
                set.IntersectWith(g[1]);
                set.IntersectWith(g[2]);
                return set.ToArray()[0];
            })
            .Select(Priority)
            .Sum();
            //.ToArray();

        return value;
    }
}