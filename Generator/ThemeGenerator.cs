using AdventOfCode.Model;

namespace AdventOfCode.Generator;

static class ThemeGenerator
{
    public static string Generate(Calendar calendar)
    {
        return $$"""
                namespace AdventOfCode.Y{{calendar.Year}};
                
                class Theme : ITheme
                {
                    public Dictionary<string, int> Override(Dictionary<string, int> themeColors)
                    {
                        return themeColors;
                    }
                }
                """;
    }
}