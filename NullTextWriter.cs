using System.Text;

namespace AdventOfCode;

public class NullTextWriter : TextWriter
{
    public override Encoding Encoding { get; } = Encoding.UTF8;
}