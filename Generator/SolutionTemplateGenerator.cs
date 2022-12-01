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

                    public IEnumerable<object> Solve(string input)
                    {
                        yield return PartOne(input);
                        yield return PartTwo(input);
                    }

                    static object PartOne(string input)
                    {
                        return 0;
                    }

                    static object PartTwo(string input)
                    {
                        return 0;
                    }
                }
                """;
        }
    }
}