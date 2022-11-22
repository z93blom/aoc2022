using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode;

partial class Program
{
    private class Resolver : IResolver
    {
        private readonly IServiceProvider _services;
        private readonly Type[] _solverTypes;

        public Resolver(IServiceProvider services, Type[] solverTypes)
        {
            _services = services;
            _solverTypes = solverTypes;
        }

        public ISolver? GetSolvers(int year, int day)
        {
            var solverType = _solverTypes
                .FirstOrDefault(t => SolverExtensions.Year(t) == year &&
                                     SolverExtensions.Day(t) == day);

            if (solverType == null)
            {
                return null;
            }

            var solver = _services.GetService(solverType) as ISolver;
            return solver;
        }

        public IEnumerable<ISolver> GetSolvers(int year)
        {
            var solverTypes = _solverTypes
                    .Where(t => SolverExtensions.Year(t) == year)
                    .OrderBy(SolverExtensions.Day)
                    .ToArray();

            foreach (var solverType in solverTypes)
            {
                if (_services.GetService(solverType) is ISolver solver)
                    yield return solver;
            }
        }
        public IEnumerable<ISolver> GetAllSolvers()
        {
            var solverTypes = _solverTypes
                .OrderBy(SolverExtensions.Year)
                .ThenBy(SolverExtensions.Day)
                .ToArray();

            foreach (var solverType in solverTypes)
            {
                if (_services.GetService(solverType) is ISolver solver)
                    yield return solver;
            }
        }

        public ISolver? GetLatestSolver()
        {
            var solverType = _solverTypes
                .OrderBy(SolverExtensions.Year)
                .ThenBy(SolverExtensions.Day)
                .LastOrDefault();

            if (solverType == null)
            {
                return null;
            }

            var solver = _services.GetService(solverType) as ISolver;
            return solver;
        }
    }
}
