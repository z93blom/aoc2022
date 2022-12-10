using AdventOfCode.Utilities;
using LanguageExt;

namespace AdventOfCode.Y2022.Day06;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 6;
    public string GetName() => "Tuning Trouble";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var index = 0;
        while (index < input.Length)
        {
            var set = new Set<char>(input.Skip(index).Take(4));
            if (set.Count == 4)
                break;

            index++;
        }

        return index+4;
    }

    static object PartTwo(string input)
    {
        var index = 0;
        while (index < input.Length)
        {
            var set = new Set<char>(input.Skip(index).Take(14));
            if (set.Count == 14)
                break;

            index++;
        }

        return index + 14;
    }
}