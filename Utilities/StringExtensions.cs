using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Utilities
{
    public static class StringExtensions
    {
        private static readonly Regex _intRegex = new(@"([+-]?\d+)", RegexOptions.Compiled);

        public static IEnumerable<string> Lines(this string s, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return s.Split(new[] {'\r', '\n'}, options);
        }

        public static IEnumerable<int> Integers(this string s)
        {
            var matches = _intRegex.Matches(s);
            return matches.Cast<Match>().SelectMany(m => m.Captures.Cast<Capture>().Select(v => int.Parse(v.Value)));
        }

        public static IEnumerable<int> ParseNumbers(this string t)
        {
            var position = 0;
            while (position < t.Length)
            {
                if (char.IsDigit(t[position]) || 
                    t[position] == '-' && char.IsDigit(t[position + 1]))
                {
                    var start = position;
                    position += 1;
                    while (char.IsDigit(t[position]))
                    {
                        position++;
                    }

                    yield return int.Parse(t[start..position]);
                }
                else
                {
                    position++;
                }
            }
        }

        public static string Replace(this string text, int index, int length, string replacement)
        {
            return new StringBuilder()
                .Append(text[..index])
                .Append(replacement)
                .Append(text.AsSpan(index + length))
                .ToString();
        }

        public static string ReplaceAll(this MatchCollection matches, string source, string replacement)
        {
            foreach (var match in matches.Cast<Match>())
            {
                source = match.Replace(source, replacement);
            }

            return source;
        }

        public static string Replace(this Match match, string source, string replacement)
        {
            return string.Concat(source.AsSpan(0, match.Index), replacement, source.AsSpan(match.Index + match.Length));
        }

        public static string Slice(this string s, int startIndex, int endIndex)
        {
            return s[startIndex..endIndex];
        }

        public static bool Matches(this string s, string pattern, out Group[] groups)
        {
            var match = Regex.Match(s, pattern);
            groups = match.Groups.Cast<Group>().Skip(1).ToArray();
            return match.Success;
        }

        public static IEnumerable<Group[]> Matches(this IEnumerable<string> strings, string pattern)
        {
            foreach(var s in strings)
            {
                if (s.Matches(pattern, out var groups))
                {
                    yield return groups;
                }
            }
        }

        /// <summary>
        /// Removes all occurances of a character from a string.
        /// </summary>
        /// <param name="s">The string being changed.</param>
        /// <param name="c">The character to be removed.</param>
        /// <returns>The resulting string.</returns>
        public static string RemoveChar(this string s, char c)
        {
            return s.Replace(new string(c, 1), string.Empty);
        }

        /// <summary>
        /// Removes all occurances of the characters from a string.
        /// </summary>
        /// <param name="s">The string being changed.</param>
        /// <param name="characters">The characters to be removed from the string.</param>
        /// <returns>The resulting string.</returns>
        public static string RemoveAll(this string s, char[] characters)
        {
            foreach(var c in characters)
            {
                s = s.RemoveChar(c);
            }

            return s;
        }

        /// <summary>
        /// Removes all characters from a string.
        /// </summary>
        /// <param name="s">The string being changed.</param>
        /// <param name="c">A string containing all the characters to be removed..</param>
        /// <returns>The resulting string.</returns>
        public static string RemoveAllChars(this string s, string remove)
        {
            foreach (var c in remove)
            {
                s = s.RemoveChar(c);
            }

            return s;
        }
    }
}