using AdventOfCode.Model;

namespace AdventOfCode.Generator
{

    public class SolutionTemplateGenerator {
        public static string Generate(Problem problem)
        {
            return $$"""
                using AdventOfCode.Utilities;

                namespace AdventOfCode.Y{{problem.Year}}.Day{{problem.Day:00}};

                class Solution : ISolver
                {
                    public int Year => {{problem.Year}};
                    public int Day => {{problem.Day}};
                    public string GetName() => "{{problem.Title}}";

                    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
                    {
                        yield return PartOne(input, getOutputFunction);
                        yield return PartTwo(input, getOutputFunction);
                    }

                    static object PartOne(string input, Func<TextWriter> getOutputFunction)
                    {
                        return 0;
                    }

                    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
                    {
                        return 0;
                    }
                }
                """;
        }
    }
}