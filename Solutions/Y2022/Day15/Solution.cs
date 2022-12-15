using System.Diagnostics;
using System.Drawing;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Text;
using AdventOfCode.Utilities;
using QuikGraph.Algorithms;

namespace AdventOfCode.Y2022.Day15;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 15;
    public string GetName() => "Beacon Exclusion Zone";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        const YAxisDirection yAxis = YAxisDirection.ZeroAtTop;
        var lines = input.Lines().ToArray();
        var targetLine = int.Parse(lines[0]); // Bonus input - added by me.
        var sensorsAndBeacons = lines
            .Skip(1)
            .Groups(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)")
            .Select(g => g.Select(v => v.ToString()).Select(int.Parse).ToArray())
            .Select(i => (new Point2(i[0], i[1], yAxis), new Point2(i[2], i[3], yAxis)))
            .ToArray();
        var coveredSquaresAtTarget = new HashSet<long>();

        var xOfbeaconsAtTarget = sensorsAndBeacons
            .Select(sensorAndBeacon => sensorAndBeacon.Item2)
            .Where(b => b.Y == targetLine)
            .Select(b => b.X)
            .ToHashSet();

        foreach (var (sensor, beacon) in sensorsAndBeacons)
        {
            // For each sensor - figure out how far it is from its beacon (Manhattan distance).
            var distance = sensor.ManhattanDistance(beacon);

            // Then, figure out how far away that sensor is from the target y-line,
            var distanceFromTarget = Math.Abs(sensor.Y - targetLine);

            // and mark certain positions on that y-line as "covered".
            var remainingSquaresAtTarget = distance - distanceFromTarget;
            for (var x = sensor.X - remainingSquaresAtTarget; x <= sensor.X + remainingSquaresAtTarget; x++)
            {
                coveredSquaresAtTarget.Add(x);
            }
        }

        coveredSquaresAtTarget.RemoveWhere(x => xOfbeaconsAtTarget.Contains(x));

        return coveredSquaresAtTarget.Count;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        const YAxisDirection yAxis = YAxisDirection.ZeroAtTop;
        var lines = input.Lines().ToArray();
        var max = int.Parse(lines[1]); // Bonus input - added by me.
        var sensorsAndBeacons = lines
            .Skip(2)
            .Groups(@"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)")
            .Select(g => g.Select(v => v.ToString()).Select(int.Parse).ToArray())
            .Select(i => (new Point2(i[0], i[1], yAxis), new Point2(i[2], i[3], yAxis)))
            .ToArray();
        var beaconLocation = new Point2();
        var sensorAndDistance =
            sensorsAndBeacons.Select(sAndB => (sensor: sAndB.Item1, beacon: sAndB.Item2, distance:sAndB.Item1.ManhattanDistance(sAndB.Item2)))
                .ToArray();

        // The beacon location has to be just outside of the sensors.

        // Get all the locations that are 1 step outside of the sensors.
        var justOutside = new HashSet<Point2>();
        foreach (var (sensor, _, distance) in sensorAndDistance)
        {
            var p = sensor with { Y = sensor.Y - distance - 1 };
            if (p.X >= 0 && p.X <= max && p.Y >= 0 && p.Y <= max) justOutside.Add(p);
            while (p.Y != sensor.Y)
            {
                p = p.Right.Below;
                if (p.X >= 0 && p.X <= max && p.Y >= 0 && p.Y <= max) justOutside.Add(p);
            }
            while (p.X != sensor.X)
            {
                p = p.Left.Below;
                if (p.X >= 0 && p.X <= max && p.Y >= 0 && p.Y <= max) justOutside.Add(p);
            }
            while (p.Y != sensor.Y)
            {
                p = p.Left.Above;
                if (p.X >= 0 && p.X <= max && p.Y >= 0 && p.Y <= max) justOutside.Add(p);
            }
            while (p.X != sensor.X)
            {
                p = p.Right.Above;
                if (p.X >= 0 && p.X <= max && p.Y >= 0 && p.Y <= max) justOutside.Add(p);
            }
        }

        foreach (var p in justOutside)
        {
            var isInsideSensorRange = false;
            foreach (var (sensor, beacon, distance) in sensorAndDistance)
            {
                if (p == beacon || p == sensor || sensor.ManhattanDistance(p) <= distance)
                {
                    isInsideSensorRange = true;
                    break;
                }
            }

            if (!isInsideSensorRange)
            {
                beaconLocation = p;
                break;
            }
            
        }

        return beaconLocation.X * 4000000 + beaconLocation.Y;
    }
}

