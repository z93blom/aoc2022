using Spectre.Console;
using System.Diagnostics;
using System.Text;
using Spectre.Console.Rendering;

namespace AdventOfCode;

public class Runner
{
    public static void RunAll(params ISolver[] solvers)
    {
        var lastYear = -1;
        foreach (var solver in solvers)
        {
            if (lastYear != solver.Year)
            {
                solver.SplashScreen().Show();
                lastYear = solver.Year;
            }

            var workingDir = solver.WorkingDir();
            var currentDirectory = Environment.CurrentDirectory;
            var directories = new[]
            {
                Path.Combine(currentDirectory, workingDir, "test"),
                Path.Combine(currentDirectory, @"..\..\..", workingDir, "test"),
                Path.Combine(currentDirectory, workingDir),
                Path.Combine(currentDirectory, @"..\..\..", workingDir),
            };
            var allFiles = directories.SelectMany(dir =>
            {
                if (!Directory.Exists(dir))
                {
                    return Enumerable.Empty<string>();
                }

                return Directory.EnumerateFiles(dir).Where(file => file.EndsWith(".in"));
            }).ToArray();

            var commonPrefix = GetLongestCommonPrefix(allFiles);

            var root = new Tree($"[white]{solver.DayName()}: {solver.GetName()}[/]")
            {
                Style = new Style()
                    .Foreground(Color.Grey35)
            };

            AnsiConsole.Live(root)
                .AutoClear(false)
                .Overflow(VerticalOverflow.Visible)
                .Start(ctx =>
                {
                    var stopWatch = new Stopwatch();
                    foreach (var file in allFiles)
                    {
                        var filePresentation = file.Substring(commonPrefix.Length,
                            file.Length - commonPrefix.Length - Path.GetExtension(file).Length);
                        var fileNode = root.AddNode(filePresentation);
                        var table = new Table()
                            .Border(TableBorder.Horizontal)
                            .BorderColor(Color.Grey35);
                        table.AddColumn("Part");
                        table.AddColumn("[bold]Value[/]");
                        table.AddColumn("Time (ms)", tc => tc.Alignment(Justify.Right));
                        table.AddColumn("Error");
                        table.AddColumn("Output");
                        fileNode.AddNode(table);
                        ctx.Refresh();

                        var partIndex = 0;
                        var valueIndex = 1;
                        var timeIndex = 2;
                        var errorIndex = 3;
                        var outputIndex = 4;

                        var refoutFile = file.Replace(".in", ".refout");
                        var refout = File.Exists(refoutFile) ? File.ReadAllLines(refoutFile) : null;
                        var input = File.ReadAllText(file).TrimEnd();
                        if (string.IsNullOrEmpty(input))
                        {
                            continue;
                        }

                        var iline = 0;
                        stopWatch.Start();
                        var partNumber = 1;
                        var outputStreams = new Dictionary<int, StringWriter>();

                        var func = new Func<TextWriter>(() =>
                        {
                            if (!outputStreams.ContainsKey(partNumber))
                            {
                                outputStreams[partNumber] = new StringWriter();
                            }

                            return outputStreams[partNumber];
                        });

                        foreach (var line in solver.Solve(input, func))
                        {
                            var elapsed = stopWatch.Elapsed;
                            var parts = new IRenderable[5];

                            var output = outputStreams.ContainsKey(partNumber) ? outputStreams[partNumber].ToString() : "";

                            if (refout == null || refout.Length <= iline)
                            {
                                parts[partIndex] = new Markup($"{partNumber++} [cyan]?[/]");
                                parts[errorIndex] = new Text("");
                            }
                            else if (refout[iline] == line.ToString())
                            {
                                parts[partIndex] = new Markup($"{partNumber++} [darkgreen]✓[/]");
                                parts[errorIndex] = new Text("");
                            }
                            else
                            {
                                parts[partIndex] = new Markup($"{partNumber++} [red]X[/]");
                                parts[errorIndex] =
                                    new Text($"{solver.DayName()}: In line {iline + 1} expected '{refout[iline]}' but found '{line}'");
                            }

                            parts[valueIndex] = new Text($"{line}", new Style().Decoration(Decoration.Bold));

                            var milliseconds = elapsed.Ticks / (double)TimeSpan.TicksPerMillisecond;
                            if (elapsed > TimeSpan.FromMilliseconds(1000))
                            {
                                parts[timeIndex] = new Markup($"[red]{milliseconds:N3}[/]");
                            }
                            else if (elapsed > TimeSpan.FromMilliseconds(500))
                            {
                                parts[timeIndex] = new Markup($"[darkorange3_1]{milliseconds:N3}[/]");
                            }
                            else
                            {
                                parts[timeIndex] = new Markup($"[darkgreen]{milliseconds:N3}[/]");
                            }

                            parts[outputIndex] = new Text(output);

                            table.AddRow(parts);
                            ctx.Refresh();
                            stopWatch.Restart();
                            iline++;
                        }
                    }
                });
        }
    }


    private static string GetLongestCommonPrefix(IReadOnlyList<string> s)
    {
        if (s.Count == 0)
        {
            return string.Empty;
        }

        if (s.Count == 1)
        {
            return Path.GetDirectoryName(s[0]) + "\\";
        }

        var k = s[0].Length;
        for (var i = 1; i < s.Count; i++)
        {
            k = Math.Min(k, s[i].Length);
            for (var j = 0; j < k; j++)
                if (s[i][j] != s[0][j])
                {
                    k = j;
                    break;
                }
        }
        return s[0][..k];
    }
}
