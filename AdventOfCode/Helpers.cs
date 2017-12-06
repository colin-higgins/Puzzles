using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzles.AdventOfCode
{
    public static class Helpers
    {
        public static IEnumerable<string> EnumerizeLines(this string input)
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

        public static IEnumerable<int> SplitToIntegers(this string input)
        {
            return input.EnumerizeLines().SelectMany(s => s.SplitOnWhitespace()).Select(int.Parse);
        }
    }
}