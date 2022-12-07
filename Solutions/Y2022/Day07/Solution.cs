using AdventOfCode.Utilities;

namespace AdventOfCode.Y2022.Day07;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 7;
    public string GetName() => "No Space Left On Device";

    public IEnumerable<object> Solve(string input)
    {
        yield return PartOne(input);
        yield return PartTwo(input);
    }

    static object PartOne(string input)
    {
        var (rootDir, directories) = CalculateTree(input);

        var result = directories.Where(d => d.DirectorySize <= 100_000).Select(d => d.DirectorySize).Sum();
        return result;
    }

    private static (Dir rootDir, HashSet<Dir> directories) CalculateTree(string input)
    {
        var lines = input.Lines().ToArray();
        var i = 1; // Skip the first cd /
        var rootDir = new Dir("", null);
        var directories = new HashSet<Dir>(new[] { rootDir });
        var currentDir = rootDir;
        while (i < lines.Length)
        {
            var commandArgs = lines[i].Split(" ");
            switch (commandArgs[0])
            {
                case "$":
                    switch (commandArgs[1])
                    {
                        case "cd":
                        {
                            if (commandArgs[2] == "..")
                            {
                                currentDir = currentDir.Parent;
                            }
                            else if (commandArgs[2] == "/")
                            {
                                currentDir = rootDir;
                            }
                            else
                            {
                                currentDir = currentDir.SubDirs.First(d => d.Name == commandArgs[2]);
                            }

                            i++;
                            break;
                        }

                        case "ls":
                        {
                            i++;
                            break;
                        }
                    }

                    break;

                case "dir":
                    var dir = new Dir(commandArgs[1], currentDir);
                    currentDir.SubDirs.Add(dir);
                    directories.Add(dir);
                    i++;
                    break;
                default:
                    var size = int.Parse(commandArgs[0]);
                    var name = commandArgs[1];
                    var file = new File(name, size);
                    currentDir.Files.Add(file);
                    i++;
                    break;
            }
        }

        return (rootDir, directories);
    }

    static object PartTwo(string input)
    {
        var (rootDir, directories) = CalculateTree(input);

        var diskSpace = 70_000_000;
        var neededSpace = 30_000_000;

        var usedSpace = rootDir.DirectorySize;

        var diskSpaceRemaining = diskSpace - usedSpace;

        var needed = neededSpace - diskSpaceRemaining;

        var dir = directories.OrderBy(d => d.DirectorySize).First(d => d.DirectorySize > needed);
        return dir.DirectorySize;
    }

    class Dir
    {
        public string Name { get; }
        public Dir Parent { get; }

        public Dir(string name, Dir? parent)
        {
            Name = name;
            Parent = parent;
        }

        public List<Dir> SubDirs { get; } = new();

        public List<File> Files { get; } = new();

        private long FileSizes => Files.Select(f => f.Size).Sum();

        public long DirectorySize => SubDirs.Select(d => d.DirectorySize).Sum() + FileSizes;
    }

    record File(string Name, long Size);
}