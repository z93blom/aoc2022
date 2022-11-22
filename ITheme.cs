namespace AdventOfCode;

public interface ITheme
{
    Dictionary<string, int> Override(Dictionary<string, int> themeColors);
}
