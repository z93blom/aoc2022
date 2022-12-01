using System.Text;
using AdventOfCode.Model;

namespace AdventOfCode.Generator;

public class SplashScreenGenerator {
    public static string Generate(Calendar calendar, Dictionary<string, int> themeColors) {
        string calendarPrinter = CalendarPrinter(calendar, themeColors);
        return $$"""
            namespace AdventOfCode.Y{{calendar.Year}};
            
            class SplashScreenImpl : SplashScreen
            {
                public override void Show()
                {
                    WriteFiglet("Advent of code {{calendar.Year}}", Spectre.Console.Color.Yellow);
                    {{calendarPrinter.Indent(8)}}
                    Console.WriteLine();
                }
            }
            """;
    }

    private static string CalendarPrinter(Calendar calendar, Dictionary<string, int> themeColors) {
        var lines = calendar.Lines.Select(line =>
            new[] { new CalendarToken { Text = "           " } }.Concat(line)).ToList();
        var bw = new BufferWriter();
        foreach (var line in lines)
        {
            foreach (var token in line)
            {
                var consoleColor = 0x888888;
                if (!string.IsNullOrWhiteSpace(token.Text))
                {
                    var style = token.Styles.FirstOrDefault(s => themeColors.ContainsKey(s));
                    consoleColor = style == default ? 0x888888 : themeColors[style];
                }

                bw.Write(consoleColor, token.Text);
            }

            bw.Write(-1, "\n");
        }

        return bw.GetContent();
    }

    class BufferWriter {
        readonly StringBuilder sb = new();
        int bufferColor = -1;
        string buffer = "";

        public void Write(int color, string text) {
            if (!string.IsNullOrWhiteSpace(text)) {
                if (color != bufferColor && !string.IsNullOrWhiteSpace(buffer)) {
                    Flush();
                }
                bufferColor = color;
            }
            buffer += text;
        }

        private void Flush() {
            while (buffer.Length > 0) {
                var block = buffer[..Math.Min(100, buffer.Length)];
                buffer = buffer[block.Length..];
                block = block.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\r", "\\r").Replace("\n", "\\n");
                sb.AppendLine($@"Write(0x{bufferColor:x6}, ""{block}"");");
            }
            buffer = "";
        }

        public string GetContent() {
            Flush();
            return sb.ToString();
        }
    }
}
