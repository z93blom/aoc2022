using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode;

partial class Program
{
    private interface IResolver
    {
        ISolver? GetSolvers(int year, int day);

        IEnumerable<ISolver> GetSolvers(int year);

        IEnumerable<ISolver> GetAllSolvers();

        ISolver? GetLatestSolver();
    }
}
