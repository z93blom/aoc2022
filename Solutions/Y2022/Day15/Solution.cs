using System.Drawing;
using AdventOfCode.Utilities;

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

        // Brute force - let's see if it works
        for (var y = 0; y <= max; y++)
        {
            var remaining = new HashSet<int>(Enumerable.Range(0, max + 1));
            foreach (var (sensor, beacon) in sensorsAndBeacons)
            {
                // For each sensor - figure out how far it is from its beacon (Manhattan distance).
                var distance = sensor.ManhattanDistance(beacon);

                // Then, figure out how far away that sensor is from the target y-line,
                var distanceFromTarget = Math.Abs(sensor.Y - y);

                // and mark certain positions on that y-line as "covered".
                var remainingSquaresAtTarget = distance - distanceFromTarget;
                for (var x = sensor.X - remainingSquaresAtTarget; x <= sensor.X + remainingSquaresAtTarget; x++)
                {
                    remaining.Remove((int)x);
                }

                if (remaining.Count == 0)
                {
                    break;
                }
            }

            if (remaining.Count == 1)
            {
                beaconLocation = new Point2(remaining.First(), y, yAxis);
                break;
            }
        }

        return beaconLocation.X * 4000000 + beaconLocation.Y;
    }
}