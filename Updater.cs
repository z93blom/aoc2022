using System.Diagnostics;
using System.Net;
using AdventOfCode.Generator;
using AdventOfCode.Model;

namespace AdventOfCode;

public class Updater
{
    public const string SessionEnvironmentName = "AOC-SESSION";

    public static async Task Update(int year, int day, IEnumerable<Type> solvers)
    {
        if (!Environment.GetEnvironmentVariables().Contains(SessionEnvironmentName))
        {
            throw new Exception($"Specify '{SessionEnvironmentName}' environment variable.");
        }

        var cookieContainer = new CookieContainer();
        using var client = new HttpClient(
            new HttpClientHandler
            {
                CookieContainer = cookieContainer,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

        var baseAddress = new Uri("https://adventofcode.com/");
        client.BaseAddress = baseAddress;
        cookieContainer.Add(baseAddress, new Cookie("session", Environment.GetEnvironmentVariable(SessionEnvironmentName)));

        var calendar = await DownloadCalendar(client, year);
        var problem = await DownloadProblem(client, year, day);

        var dir = Dir(year, day);
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        var years = solvers.Select(s => SolverExtensions.Year(s));

        CreateThemeForYear(calendar);
        UpdateReadmeForYear(calendar);
        UpdateSplashScreen(calendar);
        UpdateReadmeForDay(problem);
        UpdateInput(problem);
        UpdateRefout(problem);
        UpdateSolutionTemplate(problem);
        UpdateExample(problem);
    }

    static async Task<string> Download(HttpClient client, string path)
    {
        Console.WriteLine($"Downloading {client.BaseAddress + path}");
        var response = await client.GetAsync(path);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }

    static void WriteFile(string file, string content)
    {
        var dir = Path.GetDirectoryName(file);
        if (!Directory.Exists(dir))
        {
            Debug.Assert(dir != null, nameof(dir) + " != null");
            Directory.CreateDirectory(dir);
        }

        Console.WriteLine($"Writing {file}");
        File.WriteAllText(file, content);
    }

    static string Dir(int year, int day) => Path.Combine(SolverExtensions.WorkingDir(year, day));

    static async Task<Calendar> DownloadCalendar(HttpClient client, int year)
    {
        var html = await Download(client, year.ToString());
        return Calendar.Parse(year, html);
    }

    static async Task<Problem> DownloadProblem(HttpClient client, int year, int day)
    {
        var problemStatement = await Download(client, $"{year}/day/{day}");
        var input = await Download(client, $"{year}/day/{day}/input");
        return Problem.Parse(year, day, client.BaseAddress + $"{year}/day/{day}", problemStatement, input);
    }

    static void UpdateReadmeForDay(Problem problem)
    {
        var file = Path.Combine(Environment.CurrentDirectory, Dir(problem.Year, problem.Day), "README.md");
        WriteFile(file, problem.ContentMd);
    }

    static void UpdateSolutionTemplate(Problem problem)
    {
        var file = Path.Combine(Environment.CurrentDirectory, Dir(problem.Year, problem.Day), "Solution.cs");
        if (!File.Exists(file))
        {
            WriteFile(file, SolutionTemplateGenerator.Generate(problem));
        }
    }

    static void UpdateExample(Problem problem)
    {
        var inputFile = Path.Combine(Environment.CurrentDirectory, Dir(problem.Year, problem.Day), "test", "example.in");
        if (!File.Exists(inputFile))
        {
            WriteFile(inputFile, string.Empty);
        }

        var answerFile = Path.Combine(Environment.CurrentDirectory, Dir(problem.Year, problem.Day), "test", "example.refout");
        if (!File.Exists(answerFile))
        {
            WriteFile(answerFile, string.Empty);
        }
    }

    static void CreateThemeForYear(Calendar calendar)
    {
        var file = Path.Combine(Environment.CurrentDirectory, SolverExtensions.WorkingDir(calendar.Year), "Theme.cs");
        if (!File.Exists(file))
        {
            WriteFile(file, ThemeGenerator.Generate(calendar));
        }
    }

    static void UpdateReadmeForYear(Calendar calendar)
    {
        var file = Path.Combine(Environment.CurrentDirectory, SolverExtensions.WorkingDir(calendar.Year), "README.md");
        WriteFile(file, ReadmeGeneratorForYear.Generate(calendar));
    }

    static void UpdateSplashScreen(Calendar calendar)
    {
        var themeColors = Theme.GetDefaultTheme();
        var theme = SolverExtensions.Theme(calendar.Year);
        if (theme != null)
        {
            theme.Override(themeColors);
        }

        var file = Path.Combine(Environment.CurrentDirectory, SolverExtensions.WorkingDir(calendar.Year), "SplashScreen.cs");
        WriteFile(file, SplashScreenGenerator.Generate(calendar, themeColors));
    }

    static void UpdateInput(Problem problem)
    {
        var file = Path.Combine(Environment.CurrentDirectory, Dir(problem.Year, problem.Day), "input.in");
        WriteFile(file, problem.Input);
    }

    static void UpdateRefout(Problem problem)
    {
        var file = Path.Combine(Environment.CurrentDirectory, Dir(problem.Year, problem.Day), "input.refout");
        if (problem.Answers.Any())
        {
            WriteFile(file, problem.Answers);
        }
    }
}
