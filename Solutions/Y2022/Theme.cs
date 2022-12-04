namespace AdventOfCode.Y2022;

class Theme : ITheme
{
    public Dictionary<string, int> Override(Dictionary<string, int> themeColors)
    {
        themeColors["calendar-color-s"] = 0xd0b376;
        themeColors["calendar-color-u"] = 0x5eabb4;
        themeColors["calendar-color-g0"] = 0x488813;
        themeColors["calendar-color-g1"] = 0x4d8b03;
        themeColors["calendar-color-g2"] = 0x7fbd39;
        themeColors["calendar-color-g3"] = 0x427322;
        themeColors["calendar-color-g3"] = 0x01461f;
        themeColors["calendar-color-g4"] = 0x01461f;

        return themeColors;
    }
}