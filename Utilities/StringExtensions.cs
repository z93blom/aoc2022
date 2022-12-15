using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Utilities
{
    public static class StringExtensions
    {
        private static readonly Regex _intRegex = new(@"([+-]?\d+)", RegexOptions.Compiled);

        private static readonly string[] LineEndings = { "\r\n", "\r", "\n" };

        public static IEnumerable<string> Lines(this string s, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            return s.Split(LineEndings, options);
        }

        public static IEnumerable<int> Integers(this string s)
        {
            var matches = _intRegex.Matches(s);
            return matches.SelectMany(m => m.Captures.Select(v => int.Parse(v.Value)));
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
                    while (position < t.Length && char.IsDigit(t[position]))
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

        public static bool TryReadNested(this string t, char startCharacter, char endCharacter, int startIndex, out int start, out int end )
        {
            start = startIndex;
            while (start < t.Length)
            {
                if (t[start] == startCharacter)
                {
                    break;
                }

                start++;
            }

            if (start >= t.Length)
            {
                end = t.Length;
                return false;
            }

            end = start;
            var nesting = 0;
            while (end < t.Length)
            {
                if (t[end] == startCharacter)
                {
                    nesting++;
                }
                else if (t[end] == endCharacter)
                {
                    nesting--;
                    if (nesting == 0)
                    {
                        break;
                    }
                }

                end++;
            }

            if (end < t.Length)
            {
                return true;
            }

            return false;
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

        public static bool Matches(this string s, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern, out Group[] groups)
        {
            var match = Regex.Match(s, pattern);
            groups = match.Groups.Cast<Group>().Skip(1).ToArray();
            return match.Success;
        }

        public static IEnumerable<Match> Matches(this string s, string pattern)
        {
            var collection = Regex.Matches(s, pattern);
            return collection;
        }

        /// <summary>
        /// Returns all the captured groups for the indicated pattern. Will only return the groups indicated by parenthesis.
        /// </summary>
        /// <param name="s">The string to apply the regular expression search pattern to.</param>
        /// <param name="pattern">The regular expression search pattern.</param>
        /// <returns></returns>
        public static IEnumerable<Group[]> Groups(this string s, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        {
            var collection = Regex.Matches(s, pattern);
            foreach (Match match in collection)
            {
                yield return match.Groups.Cast<Group>().Skip(1).ToArray();
            }
        }


        public static IEnumerable<Group[]> Groups(this IEnumerable<string> strings, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
        {
            foreach (var s in strings)
            {
                if (s.Matches(pattern, out var groups))
                {
                    yield return groups.ToArray();
                }
            }
        }


        public static IEnumerable<Group[]> Matches(this IEnumerable<string> strings, [StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
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
        /// Removes all occurrences of a character from a string.
        /// </summary>
        /// <param name="s">The string being changed.</param>
        /// <param name="c">The character to be removed.</param>
        /// <returns>The resulting string.</returns>
        public static string RemoveChar(this string s, char c)
        {
            return s.Replace(new string(c, 1), string.Empty);
        }

        /// <summary>
        /// Removes all occurrences of the characters from a string.
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