using System.Text.RegularExpressions;

namespace AdventOfCode;

public static class StringExtensions
{
    public static string Indent(this string st, int l)
    {
        return string.Join("\n" + new string(' ', l),
            from line in st.Split('\n')
            select Regex.Replace(line, @"^\s*\|", "")
        );
    }
}