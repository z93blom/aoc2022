
namespace AdventOfCode.Y2022;

class Theme : ITheme
{
    public Dictionary<string, int> Override(Dictionary<string, int> themeColors)
    {
        return themeColors;
    }
}