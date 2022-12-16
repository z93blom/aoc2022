using System.Runtime.InteropServices;
using AdventOfCode.Utilities;
using LanguageExt;
using QuikGraph;
using QuikGraph.Algorithms;

namespace AdventOfCode.Y2022.Day16;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 16;
    public string GetName() => "Proboscidea Volcanium";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var rooms = input.Lines().Groups(@"^Valve ([A-Z][A-Z]) has flow rate=(\d+); [^A-Z]+ (.+)$")
            .Select(g => new Room(g[0].Value, int.Parse(g[1].Value),
                g[2].Value.Split(',').Select(t => t.Trim()).ToArray()))
            .ToDictionary(vr => vr.ValveName, vr => vr);

        var startingRoom = rooms["AA"];
        var states = new Queue<State>();
        states.Enqueue(new State(startingRoom, Arr<Room>.Empty, 0));
        for (var minute = 1; minute <= 30; minute++)
        {
            var newStates = new List<State>();
            while (states.Count > 0)
            {
                var state = states.Dequeue();

                var releasedPressure = state.OpenedValves.Aggregate(state.TotalReleasedPressure, (pressure, room) => pressure + room.FlowRate);

                var newState = state with { TotalReleasedPressure = releasedPressure };

                // Possible new states:
                // Open the valve if it has a flow rate > 0 and is not already opened.
                // Move to a different room.
                if (state.Location.FlowRate > 0 && !state.OpenedValves.Contains(state.Location))
                {
                    newStates.Add(newState with { OpenedValves = Arr.add(state.OpenedValves, state.Location) });
                }

                foreach (var tunnel in state.Location.Tunnels)
                {
                    newStates.Add(newState with { Location = rooms[tunnel] });
                }
            }

            states = new Queue<State>(newStates.OrderByDescending(s => s.TotalReleasedPressure).Take(1000));
        }

        var topState = states.MaxBy(s => s.TotalReleasedPressure);

        return topState.TotalReleasedPressure;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var rooms = input.Lines().Groups(@"^Valve ([A-Z][A-Z]) has flow rate=(\d+); [^A-Z]+ (.+)$")
            .Select(g => new Room(g[0].Value, int.Parse(g[1].Value),
                g[2].Value.Split(',').Select(t => t.Trim()).ToArray()))
            .ToDictionary(vr => vr.ValveName, vr => vr);

        var numberOfPossibleValves = rooms.Count(kvp => kvp.Value.FlowRate > 0);
        var startingRoom = rooms["AA"];
        var states = new Queue<State2>();
        states.Enqueue(new State2(startingRoom, startingRoom, Arr<Room>.Empty, 0));
        for (var minute = 1; minute <= 26; minute++)
        {
            var newStates = new List<State2>();
            while (states.Count > 0)
            {
                var state = states.Dequeue();

                var releasedPressureThisMinute = state.OpenedValves.Aggregate(0, (pressure, room) => pressure + room.FlowRate);
                var releasedPressure = state.TotalReleasedPressure + releasedPressureThisMinute;

                var newState = state with { TotalReleasedPressure = releasedPressure };

                // Shortcut - all possible valves are opened.
                if (state.OpenedValves.Length == numberOfPossibleValves)
                {
                    newStates.Add(newState);
                    continue;
                }

                // All possible new states.
                // Both valves can be opened
                // Either valve can be opened  (and the other actor moves)
                // The actors move (all permutations).
                var loc1 = state.Location;
                var loc2 = state.ElephantLocation;

                if (loc1 != loc2 && loc1.FlowRate > 0 && !state.OpenedValves.Contains(loc1) && loc2.FlowRate > 0 && !state.OpenedValves.Contains(loc2))
                {
                    // Both valves can be opened.
                    newStates.Add(newState with { OpenedValves = state.OpenedValves.Add(loc1).Add(loc2) });
                }
                else if (loc1.FlowRate > 0 && !state.OpenedValves.Contains(loc1))
                {
                    var openedValves = state.OpenedValves.Add(loc1);
                    foreach (var elephantRoom in loc2.Tunnels)
                    {
                        newStates.Add(newState with { OpenedValves = openedValves, ElephantLocation = rooms[elephantRoom] });
                    }
                }
                else if (loc2.FlowRate > 0 && !state.OpenedValves.Contains(loc2))
                {
                    var openedValves = state.OpenedValves.Add(loc2);
                    foreach (var room in loc1.Tunnels)
                    {
                        newStates.Add(newState with { OpenedValves = openedValves, Location = rooms[room] });
                    }
                }

                foreach (var tunnel in state.Location.Tunnels)
                {
                    foreach (var elephantTunnel in state.ElephantLocation.Tunnels)
                    {
                        newStates.Add(newState with { Location = rooms[tunnel], ElephantLocation = rooms[elephantTunnel] });
                    }
                }
            }

            states = new Queue<State2>(newStates.OrderByDescending(s => s.TotalReleasedPressure).Take(100_000));
        }

        var topState = states.MaxBy(s => s.TotalReleasedPressure);

        return topState.TotalReleasedPressure;
    }

    public record struct State(Room Location, Arr<Room> OpenedValves, int TotalReleasedPressure);
    public record struct State2(Room Location, Room ElephantLocation, Arr<Room> OpenedValves, int TotalReleasedPressure);

    public record struct Room(string ValveName, int FlowRate, string[] Tunnels);
}
