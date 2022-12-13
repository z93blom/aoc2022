using AdventOfCode.Utilities;

namespace AdventOfCode.Y2022.Day13;

class Solution : ISolver
{
    public int Year => 2022;
    public int Day => 13;
    public string GetName() => "Distress Signal";

    public IEnumerable<object> Solve(string input, Func<TextWriter> getOutputFunction)
    {
        // var emptyOutput = () => new NullTextWriter();
        yield return PartOne(input, getOutputFunction);
        yield return PartTwo(input, getOutputFunction);
    }

    static object PartOne(string input, Func<TextWriter> getOutputFunction)
    {
        var lines = input.Lines().ToArray();
        var inRightOrder = new List<int>();
        for (var index = 0; index < lines.Length; index += 2)
        {
            var left = Item.FromString(lines[index]);
            var right = Item.FromString(lines[index + 1]);
            var compare = Item.Compare(left, right);
            if (compare < 0)
                inRightOrder.Add(index / 2 + 1);
        }

        return inRightOrder.Sum();
    }

    static object PartTwo(string input, Func<TextWriter> getOutputFunction)
    {
        var two = Item.FromString("[[2]]");
        var six = Item.FromString("[[6]]");
        var ordered = input.Lines()
            .Select(Item.FromString)
            .Append(new [] {two, six})
            .Order()
            .ToList();

        return (ordered.IndexOf(two) + 1) * (ordered.IndexOf(six) + 1);
    }

    private class Item : IComparable<Item>
    {
        private Item[] SubItems { get; } = Array.Empty<Item>();
        private int Value { get; } = 0;
        private readonly bool _isList = false;

        private bool IsInteger => !_isList;
        private bool IsList => _isList;

        private Item(int value)
        {
            Value = value;
            _isList = false;
        }

        private Item(Item[] items)
        {
            SubItems = items;
            _isList = true;
        }

        public int CompareTo(Item? other)
        {
            if (other == null)
            {
                return -1;
            }

            return Compare(this, other);
        }

        public override string ToString()
        {
            return _isList ? $"[{string.Join(",", SubItems.Select(i => i.ToString()))}]" : Value.ToString();
        }

        public static Item FromString(string s)
        {
            var index = 0;
            var item = ReadItem(s, ref index);
            if (index < s.Length) throw new Exception("Item Parse error - not fully parsed!");
            return item;
        }

        private static Item ReadItem(string s, ref int index)
        {
            if (s[index] == '[')
            {
                if (!s.TryReadNested('[', ']', index, out var listStart, out var listEnd)) throw new ArgumentException("Unmatched list!");
                index = listStart + 1;
                List<Item> subItems = new();
                while (index < listEnd)
                {
                    subItems.Add(ReadItem(s, ref index));
                    if (s[index] != ',' && s[index] != ']') throw new ArgumentException(nameof(s), "Missing comma!");
                    index++;
                }

                index = listEnd + 1;
                return new Item(subItems.ToArray());
            }

            // Must be an integer.
            var valueStart = index;
            while (char.IsDigit(s[index]))
            {
                index++;
            }

            var v = int.Parse(s[valueStart..index]);
            return new Item(v);
        }

        public static int Compare(Item left, Item right)
        {
            // If both values are integers, the lower integer should come left
            if (left.IsInteger && right.IsInteger)
            {
                // If the left integer is lower than the right integer, the inputs are in the right order
                if (left.Value < right.Value)
                {
                    return -1;
                }

                // If the left integer is higher than the right integer, the inputs are not in the right order
                if (left.Value > right.Value)
                {
                    return 1;
                }

                // Otherwise, the inputs are the same integer; continue checking the next part of the input.
                return 0;
            }

            // If both values are lists, compare the first value of each list, then the second value, and so on.
            if (left.IsList && right.IsList)
            {
                var subIndex = 0;
                while (subIndex < left.SubItems.Length && subIndex < right.SubItems.Length)
                {
                    var compare = Compare(left.SubItems[subIndex], right.SubItems[subIndex]);
                    if (compare != 0)
                    {
                        return compare;
                    }

                    subIndex++;
                }

                // If the left list runs out of items first, the inputs are in the right order
                if (left.SubItems.Length < right.SubItems.Length)
                {
                    return -1;
                }

                // If the right list runs out of items first, the inputs are not in the right order.
                if (right.SubItems.Length < left.SubItems.Length)
                {
                    return 1;
                }

                // If the lists are the same length and no comparison makes a decision about the order, continue checking the next part of the input.
                return 0;
            }

            // If exactly one value is an integer, convert the integer to a list which contains that integer as its only value, then retry the comparison.
            if (left.IsInteger)
            {
                var replacedLeft = new Item(new [] { left });
                return Compare(replacedLeft, right);
            }

            var replacedRight = new Item(new[] { right });
            return Compare(left, replacedRight);

        }
    }
}