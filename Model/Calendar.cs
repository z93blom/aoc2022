using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace AdventOfCode.Model
{

    public class CalendarToken {
        public string[] Styles = Array.Empty<string>();
        public string Text { get; set; } = string.Empty;
    }

    public class Calendar {
        public int Year;

        public IReadOnlyList<IReadOnlyList<CalendarToken>> Lines { get; private set; } = new List<IReadOnlyList<CalendarToken>>();

        public static Calendar Parse(int year, string html)
        {
            var document = new HtmlDocument();
            document.LoadHtml(html);

            var calendar = document.DocumentNode.SelectSingleNode("//*[contains(@class,'calendar')]");

            if (calendar.SelectNodes(".//script") != null)
            {
                foreach (var script in calendar.SelectNodes(".//script").ToList())
                {
                    script.Remove();
                }
            }

            var lines = new List<List<CalendarToken>>();
            var line = new List<CalendarToken>();
            lines.Add(line);

            foreach (var textNode in calendar.SelectNodes(".//text()"))
            {
                var styles = GetStyles(textNode);

                var text = textNode.InnerText;
                var widthSpec = styles.FirstOrDefault(style => style.StartsWith("width:"));
                if (widthSpec != null)
                {
                    var m = Regex.Match(widthSpec, "[.0-9]+");
                    if (m.Success)
                    {
                        var width = double.Parse(m.Value) * 1.7;
                        var c = (int)Math.Round(width - text.Length, MidpointRounding.AwayFromZero);
                        if (c > 0)
                        {
                            text += new string(' ', c);
                        }
                    }
                }

                var i = 0;
                while (i < text.Length)
                {
                    var iNext = text.IndexOf("\n", i);
                    if (iNext == -1)
                    {
                        iNext = text.Length;
                    }

                    var token = new CalendarToken
                    {
                        Styles = styles.ToArray(),
                        Text = HtmlEntity.DeEntitize(text[i..iNext])
                    };

                    line.Add(token);

                    if (iNext < text.Length)
                    {
                        line = new List<CalendarToken>();
                        lines.Add(line);
                    }

                    i = iNext + 1;
                }
            }


            return new Calendar { Year = year, Lines = lines };
        }

        private static List<string> GetStyles(HtmlNode textNode)
        {
            var styles = new List<string>();
            foreach (var node in textNode.Ancestors())
            {
                if (node.Attributes["class"] != null)
                {
                    styles.AddRange(node.Attributes["class"].Value.Split(' '));
                }
                if (node.Attributes["style"] != null)
                {
                    styles.Add(node.Attributes["style"].Value);
                }
            }

            return styles;
        }
    }
}