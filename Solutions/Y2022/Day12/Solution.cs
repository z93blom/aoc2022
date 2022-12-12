using AdventOfCode.Utilities;
using QuikGraph;
using QuikGraph.Algorithms;

namespace AdventOfCode.Y2022.Day12;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 12;
    public string GetName() => "Hill Climbing Algorithm";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var map = ReadMap(input);
        var (graph, nodes) = ConvertToGraph(map);
        var pathComputer = graph.ShortestPathsAStar(_ => 1, _ => 0, nodes[map.StartingPosition]);

        if (!pathComputer(nodes[map.Target], out var result))
        {
            throw new InvalidOperationException("Unable to reach target!");
        }

        var edges = result.ToArray();

        var r = edges.Length;
        return r;
    }

    private static (AdjacencyGraph<Point2Node<char>, Point2Edge<char>>, Dictionary<Point2, Point2Node<char>>) ConvertToGraph(Map map)
    {
        var nodes = new Dictionary<Point2, Point2Node<char>>();
        foreach (var point in map.Grid.Points)
        {
            var node = new Point2Node<char>(point, (char)(map.Grid[point] + 'a'));
            nodes.Add(point, node);
        }

        var edges = new List<Point2Edge<char>>();
        foreach (var point in map.Grid.Points)
        {
            var currentValue = map.Grid[point];
            foreach (var p2 in point.OrthogonalPoints)
            {
                if (map.Grid.Contains(p2))
                {
                    var targetValue = map.Grid[p2];
                    if (targetValue <= currentValue + 1)
                    {
                        var edge = new Point2Edge<char>(nodes[point], nodes[p2]);
                        edges.Add(edge);
                    }
                }
            }
        }

        var graph = new AdjacencyGraph<Point2Node<char>, Point2Edge<char>>();
        graph.AddVerticesAndEdgeRange(edges);
        return (graph, nodes);
    }

    private static Map ReadMap(string input)
    {
        var lines = input.Lines().ToArray();
        var width = lines[0].Length;
        var height = lines.Length;
        var map = new Map(width, height);
        for (var y = 0; y < height; y++)
        {
            var line = lines[y];
            var yGrid = height - y - 1;
            for (var x = 0; x < width; x++)
            {
                switch (line[x])
                {
                    case 'E':
                        map.Grid[x, yGrid] = 'z' - 'a';
                        map.Target = new Point2(x, yGrid);
                        break;
                    case 'S':
                        map.Grid[x, yGrid] = 'a' - 'a';
                        map.StartingPosition = new Point2(x, yGrid);
                        break;
                    default:
                        map.Grid[x, yGrid] = line[x] - 'a';
                        break;
                }
            }
        }

        return map;
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var map = ReadMap(input);
        var (graph, nodes) = ConvertToGraph(map);

        int shortestPath = int.MaxValue;

        foreach (var node in nodes.Keys.Where(node => map.Grid[node] == 0))
        {
            var pathComputer = graph.ShortestPathsAStar(_ => 1, _ => 0, nodes[node]);
            if (!pathComputer(nodes[map.Target], out var result))
            {
                continue;
            }

            var path = result.Count();
            if (path < shortestPath)
            {
                shortestPath = path;
            }
        }

        return shortestPath;
    }

    private class Map
    {
        public Map(int width, int height)
        {
            Grid = new Grid<int>(width, height);
        }

        public Point2 StartingPosition { get; set; }
        public Point2 Target { get; set; }

        public Grid<int> Grid { get; }

        public override string ToString()
        {
            return Grid.ToString();
        }
    }
}