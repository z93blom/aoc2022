namespace AdventOfCode.Y2022;

class Theme : ITheme
{
    public Dictionary<string, int> Override(Dictionary<string, int> themeColors)
    {
        themeColors["calendar-color-s"] = 0xd0b376;

        return themeColors;
    }
}