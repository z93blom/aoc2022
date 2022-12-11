using System.Diagnostics.Eventing.Reader;
using AdventOfCode.Utilities;
using QuikGraph;

namespace AdventOfCode.Y2022.Day11;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 11;
    public string GetName() => "Monkey in the Middle";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, emptyOutput);
        yield return PartTwo(input, emptyOutput);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var monkeys = ParseMonkeys(input);
        var output = getOutputFunction();
        for (var round = 0; round < 20; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Inspect();
            }
        }

        var ii = 0;
        foreach (var monkey in monkeys)
        {
            output.WriteLine($"Monkey {ii++}: inspected items {monkey.TotalNumberOfInspectedItems} times.");
        }

        var result = monkeys.OrderByDescending(m => m.TotalNumberOfInspectedItems).Take(2).Aggregate(1L, (v, m) => m.TotalNumberOfInspectedItems * v);
        return result;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var monkeys = ParseMonkeys(input);

        // All the divisors are prime numbers, and we can use their aggregate factor to keep the worry levels to manageable levels.
        var divisor = monkeys.Aggregate(1L, (v, m) => m.Divisor * v);
        var output = getOutputFunction();
        for (var round = 0; round < 10_000; round++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Inspect2(divisor);
            }
            
            if ((round + 1) % 1_000 == 0 || round == 19 || round == 0)
            {
                var i = 0;
                output.WriteLine($"== After round {round + 1} ==");
                foreach (var monkey in monkeys)
                {
                    output.WriteLine($"Monkey {i++}: inspected items {monkey.TotalNumberOfInspectedItems} times.");
                }

                output.WriteLine();
            }
        }

        var result = monkeys.OrderByDescending(m => m.TotalNumberOfInspectedItems).Take(2).Aggregate(1L, (v, m) => m.TotalNumberOfInspectedItems * v);
        return result;
    }

    private static Monkey[] ParseMonkeys(string input)
    {
        var lines = input.Lines().ToArray();
        var monkeys = new Monkey[lines.Length / 6];
        for (var i = 0; i < monkeys.Length; ++i)
        {
            monkeys[i] = new Monkey();
        }

        var lineIndex = 0;
        foreach (var monkey in monkeys)
        {
            lineIndex++;

            // Starting items: 79, 98
            monkey.ItemWorryLevels = lines[lineIndex++].Integers().Select(v => (Int128)v).ToList();

            // Operation: new = old * 19
            var parts = lines[lineIndex++].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            monkey.ModifierOperation = GetModifierOperation(parts);

            // Test: divisible by 23
            monkey.Divisor = lines[lineIndex++].Integers().First();

            // If true: throw to monkey 2
            monkey.TrueMonkey = monkeys[lines[lineIndex++].Integers().First()];

            // If false: throw to monkey 3
            monkey.FalseMonkey = monkeys[lines[lineIndex++].Integers().First()];
        }

        return monkeys;
    }

    private static Func<Int128, Int128> GetModifierOperation(string[] parts)
    {
        if (parts.Length != 6) { throw new ArgumentOutOfRangeException("Unknown operation."); }
        // 3 = operand1
        // 4 = op
        // 5 = operand2
        var op1IsV = !int.TryParse(parts[3], out var op1);
        var op2IsV = !int.TryParse(parts[5], out var op2);
        switch (parts[4])
        {
            case "+":
                if (op1IsV && op2IsV)
                    return v => v + v;
                if (op1IsV)
                    return v => v + op2;
                if (op2IsV)
                    return v => op1 + v;
                return _ => op1 + op2;

            case "-":
                if (op1IsV && op2IsV)
                    return _ => 0; // v - v
                if (op1IsV)
                    return v => v - op2;
                if (op2IsV)
                    return v => op1 - v;
                return _ => op1 - op2;

            case "*":
                if (op1IsV && op2IsV)
                    return v => v * v;
                if (op1IsV)
                    return v => v * op2;
                if (op2IsV)
                    return v => op1 * v;
                return _ => op1 * op2;

            case "/":
                if (op1IsV && op2IsV)
                    return v => 1; // v / v
                if (op1IsV)
                    return v => v / op2;
                if (op2IsV)
                    return v => op1 / v;
                return _ => op1 / op2;
            default:
                throw new ArgumentOutOfRangeException($"Unknown operator {parts[4]}.");
        }
    }

    private class Monkey
    {
        public List<Int128> ItemWorryLevels { get; set; } = new();
        public long Divisor { get; set; } = 1;

        public Func<Int128, Int128> ModifierOperation { get; set; }

        public Monkey TrueMonkey { get; set; }
        public Monkey FalseMonkey { get; set; }

        public long TotalNumberOfInspectedItems = 0;

        public void Inspect()
        {
            TotalNumberOfInspectedItems += ItemWorryLevels.Count;
            foreach (var itemWorryLevel in ItemWorryLevels)
            {
                var newWorryLevel = ModifierOperation(itemWorryLevel);
                newWorryLevel /= 3;
                if (newWorryLevel % Divisor == 0)
                {
                    TrueMonkey.ItemWorryLevels.Add(newWorryLevel);
                }
                else
                {
                    FalseMonkey.ItemWorryLevels.Add(newWorryLevel);
                }
            }

            ItemWorryLevels.Clear();
        }

        public void Inspect2(long divisor)
        {
            TotalNumberOfInspectedItems += ItemWorryLevels.Count;
            foreach (var itemWorryLevel in ItemWorryLevels)
            {
                var newWorryLevel = ModifierOperation(itemWorryLevel);
                newWorryLevel %= divisor;
                if (newWorryLevel % Divisor == 0)
                {
                    TrueMonkey.ItemWorryLevels.Add(newWorryLevel);
                }
                else
                {
                    FalseMonkey.ItemWorryLevels.Add(newWorryLevel);
                }
            }

            ItemWorryLevels.Clear();
        }

    }
}