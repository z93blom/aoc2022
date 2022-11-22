namespace AdventOfCode;

public interface ISolver
{
    string GetName();
    int Year { get; }
    
    int Day { get; }

    IEnumerable<object> Solve(string input);
}
