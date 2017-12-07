using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles.AdventOfCode
{
    public static class Helpers
    {
        public static IEnumerable<string> SplitOnNewLine(this string input)
        {
            var lines = input.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                yield return line;
            }
        }

        public static IEnumerable<IEnumerable<string>> SplitOnWhitespace(this IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                yield return line.SplitOnWhitespace();
            }
        }

        public static IEnumerable<string> SplitOnWhitespace(this string input)
        {
            return input.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<string> SplitOn(this string input, params string[] separators)
        {
            return input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        }

        public static IList<string> TrimAll(this IList<string> strings)
        {
            for (var i = 0; i < strings.Count(); i++)
            {
                strings[i] = strings[i].Trim();
            }

            return strings;
        }

        public static IEnumerable<int> SplitToIntegers(this string input)
        {
            return input.SplitOnNewLine().SelectMany(s => s.SplitOnWhitespace()).Select(int.Parse);
        }

        public static string RemoveInstancesOf(this string input, params string[] matches)
        {
            foreach (var match in matches)
                input = input.Replace(match, "");

            return input;
        }

        public static IEnumerable<IGrouping<TKey, TElement>> WhereLengthIs<TKey, TElement>(
            this IEnumerable<IGrouping<TKey, TElement>> groupings, 
            int length)
        {
            return groupings.Where(g => g.Count() == length);
        }

        public static IEnumerable<IGrouping<TKey, TElement>> WhereLengthIsAtLeast<TKey, TElement>(
            this IEnumerable<IGrouping<TKey, TElement>> groupings,
            int length)
        {
            return groupings.Where(g => g.Count() >= length);
        }
    }
}