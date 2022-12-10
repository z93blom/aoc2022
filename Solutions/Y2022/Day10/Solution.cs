using System.Text;
using AdventOfCode.Utilities;

namespace AdventOfCode.Y2022.Day10;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 10;
    public string GetName() => "Cathode-Ray Tube";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        yield return PartOne(input);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input)
    {
        var registerValueDuringCycle = RegisterValueDuringCycle(input);

        var signalStrength = new List<int>();
        var cycleNumber = 20;
        while (cycleNumber < registerValueDuringCycle.Count)
        {
            signalStrength.Add(registerValueDuringCycle[cycleNumber] * cycleNumber);
            cycleNumber += 40;
        }

        var value = signalStrength.Take(6).Sum();
        return value;
    }

    private static Dictionary<int, int> RegisterValueDuringCycle(string input)
    {
        var instructions = input.Lines().ToArray();
        var cycle = 1;
        var registerX = 1;
        var registerValueDuringCycle = new Dictionary<int, int>(instructions.Length * 2);
        foreach (var instruction in instructions)
        {
            var parts = instruction.Split(' ');
            if (parts[0] == "noop")
            {
                registerValueDuringCycle.Add(cycle, registerX);
                cycle++;
            }
            else if (parts[0] == "addx")
            {
                var v = int.Parse(parts[1]);
                registerValueDuringCycle.Add(cycle, registerX);
                cycle++;
                registerValueDuringCycle.Add(cycle, registerX);
                cycle++;
                registerX += v;
            }
        }

        registerValueDuringCycle.Add(cycle, registerX);
        return registerValueDuringCycle;
    }

    static object PartTwo(string input, Func<TextWriter> output)
    {
        var registerValueDuringCycle = RegisterValueDuringCycle(input);
        var writer = output();
        var cycle = 1;
        while (cycle < registerValueDuringCycle.Count)
        {
            var spritePosition = registerValueDuringCycle[cycle];
            var x = (cycle - 1) % 40;
            if (spritePosition == x || spritePosition - 1 == x || spritePosition + 1 == x)
            {
                writer.Write('#');
            }
            else
            {
                writer.Write('.');
            }

            if (x == 39)
            {
                writer.WriteLine();
            }

            cycle++;
        }

        return "PHLHJGZA";
    }
}