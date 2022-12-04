using AdventOfCode.Utilities;
using LanguageExt.UnitsOfMeasure;

namespace AdventOfCode.Y2022.Day04;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 4;
    public string GetName() => "Camp Cleanup";

    public IEnumerable<object> Solve(string input)
    {
        var pairs = input.Lines()
            .Select(s => s.Split(","))
            .Select(a => new Pairs(ToArea(a[0]), ToArea(a[1])))
            .ToArray();

        yield return PartOne(pairs);
        yield return PartTwo(pairs);
    }

    private record Pairs(Area First, Area Second);

    private record Area(int Start, int End)
    {
        public bool Contains(Area a)
        {
            return Start <= a.Start && End >= a.End;
        }

        public bool Overlaps(Area a)
        {
            return (Start >= a.Start && Start <= a.End) || (End >= a.Start && End <= a.End);
        }
    }

    static object PartOne(Pairs[] pairs)
    {
        var count = pairs.Count(p => p.First.Contains(p.Second) || p.Second.Contains(p.First));
        return count;
    }

    private static Area ToArea(string input)
    {
        var a = input.Split("-");
        var start = int.Parse(a[0]);
        var end = int.Parse(a[1]);
        return new Area(start, end);
    }

    static object PartTwo(Pairs[] pairs)
    {
        var count = pairs.Count(p => p.First.Overlaps(p.Second) || p.Second.Overlaps(p.First));
        return count;
    }
}