using System.Reflection;

namespace AdventOfCode;

public static class SolverExtensions
{
    public static string DayName(this ISolver solver)
    {
        return $"Day {solver.Day}";
    }

#pragma warning disable CS8602 // Dereference of a possibly null reference.

    public static int Year(Type t)
    {
        return int.Parse(t.FullName.Split('.')[1][1..]);
    }

    public static int Day(Type t)
    {
        return int.Parse(t.FullName.Split('.')[2][3..]);
    }
#pragma warning restore CS8602 // Dereference of a possibly null reference.


    public static string WorkingDir(int year)
    {
        return Path.Combine("Solutions", $"Y{year}");
    }

    public static string WorkingDir(int year, int day)
    {
        return Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));
    }

    public static string WorkingDir(this ISolver solver)
    {
        return WorkingDir(solver.Year, solver.Day);
    }

    public static SplashScreen SplashScreen(this ISolver solver)
    {
        var splashScreenType = solver.GetType().Assembly.GetTypes()
             .Where(t => t.GetTypeInfo().IsClass && !t.IsAbstract && typeof(SplashScreen).IsAssignableFrom(t))
             .Single(t => Year(t) == solver.Year);
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8603 // Possible null reference return.
        return (SplashScreen)Activator.CreateInstance(splashScreenType);
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }

    public static ITheme? Theme(int year)
    {
        var themeType = typeof(SolverExtensions).Assembly.GetTypes()
             .Where(t => t.GetTypeInfo().IsClass && !t.IsAbstract && typeof(ITheme).IsAssignableFrom(t))
             .SingleOrDefault(t => Year(t) == year);
        if (themeType == null)
        {
            return null;
        }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        return (ITheme)Activator.CreateInstance(themeType);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
    }
}
