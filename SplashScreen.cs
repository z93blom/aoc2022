using Spectre.Console;
using Spectre.Console.Rendering;
using System.Text;

namespace AdventOfCode;

public abstract class SplashScreen
{
    readonly FigletFont _font;
    public SplashScreen()
    {
        _font = FigletFont.Parse(Encoding.UTF8.GetString(Resource.serifcap));
    }

    public abstract void Show();

    protected static void Write(int rgb, string text)
    {
        AnsiConsole.Markup($"[#{rgb:x6}]{text}[/]");
    }

    protected void WriteFiglet(string text)
    {
        AnsiConsole.Write(new FigletText(_font, text));
    }

    protected void WriteFiglet(string text, Color color)
    {
        AnsiConsole.Write(new FigletText(_font, text).Color(color));
    }

}