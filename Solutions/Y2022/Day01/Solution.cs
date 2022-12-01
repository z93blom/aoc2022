using AdventOfCode.Utilities;

namespace AdventOfCode.Y2022.Day01;

class Solution : ISolver
{
    public int Year => 2022;

    public int Day => 1;

    public string GetName() => "Calorie Counting";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var foodPerElf = FoodPerElf(input);

        var max = foodPerElf.Max(kvp => kvp.Value.Sum());
        return max;
    }

    private static Dictionary<int, List<int>> FoodPerElf(string input)
    {
        var lines = input.Lines(StringSplitOptions.None);
        var foodPerElf = new Dictionary<int, List<int>>();
        int elfIndex = 0;
        foodPerElf[elfIndex] = new List<int>();
        foreach (var line in lines)
        {
            if (int.TryParse(line, out var food))
            {
                foodPerElf[elfIndex].Add(food);
            }
            else
            {
                elfIndex++;
                foodPerElf[elfIndex] = new List<int>();
            }
        }

        return foodPerElf;
    }

    static object PartTwo(string input)
    {
        var foodPerElf = FoodPerElf(input);
        var topThree = foodPerElf.OrderByDescending(kvp => kvp.Value.Sum()).Take(3).Sum(kvp => kvp.Value.Sum());
        return topThree;
    }
}