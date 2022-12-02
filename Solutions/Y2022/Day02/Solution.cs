using AdventOfCode.Utilities;

namespace AdventOfCode.Y2022.Day02;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 2;
    public string GetName() => "Rock Paper Scissors";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var (player1, player2) = ParseInput(input);
        var round = Enumerable.Range(0, player1.Length);
        var totalScore = round.Select(i => PartOneScore(player1[i], player2[i])).Sum();
        return totalScore;
    }
    private static int PartOneScore(char c, char p)
    {
        var win = (c == 'A' && p == 'Y' ||
                   c == 'B' && p == 'Z' ||
                   c == 'C' && p == 'X');

        var tie = (c == 'A' && p == 'X' ||
                   c == 'B' && p == 'Y' ||
                   c == 'C' && p == 'Z');

        var s = (p - 'X' + 1);
        var w = (win ? 6 : tie ? 3 : 0);
        return s + w;
    }

    private static (char[] player1, char[] player2) ParseInput(string input)
    {
        var l = input.Lines().ToArray();
        var player1 = l.Select(l => l[0]).ToArray();
        var player2 = l.Select(l => l[2]).ToArray();
        return (player1, player2);
    }

    static object PartTwo(string input)
    {
        var (player1, player2) = ParseInput(input);
        var round = Enumerable.Range(0, player1.Length);
        var totalScore = round.Select(i => PartTwoScore(player1[i], player2[i])).Sum();
        return totalScore;
    }

    private static int PartTwoScore(char c, char r)
    {
        var win = r == 'Z';

        var tie = r == 'Y';

#pragma warning disable CS8509 
        var p = (win, tie) switch
        {
            (true, false) => c == 'A' ? 'Y' : c == 'B' ? 'Z' : 'X',
            (false, true) => c == 'A' ? 'X' : c == 'B' ? 'Y' : 'Z',
            (false, false) => c == 'A' ? 'Z' : c == 'B' ? 'X' : 'Y',
        };
#pragma warning restore CS8509 
        var s = (p - 'X' + 1);
        var w = (win ? 6 : tie ? 3 : 0);
        return s + w;
    }
}