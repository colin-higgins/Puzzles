using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Puzzles.AdventOfCode.Y2017
{
    [TestFixture]
    public class Day10
    {
        /// <summary>
        /// 
        /// </summary>
        [TestCase(4114, "165,1,255,31,87,52,24,113,0,91,148,254,158,2,73,153")]
        public void Part1(int expectedResult, string input)
        {
            var lengths = input.SplitToIntegers(",");

            var knot = new Knot();

            foreach (var length in lengths)
            {
                knot.Flip(length);
            }

            Assert.AreEqual(expectedResult, knot.MultiplyFirstTwo());
        }

        /// <summary>
        /// 
        /// </summary>
        [TestCase("a2582a3a0e66e6e86e3812dcb672a272", "")]
        [TestCase("33efeb34ea91902bb2f59c9920caa6cd", "AoC 2017")]
        [TestCase("3efbe78a8d82f29979031a4aa0b16a9d", "1,2,3")]
        [TestCase("63960835bcdc130f0b66d7ff4f6a5a8e", "1,2,4")]
        [TestCase("2f8c3d2100fdd57cec130d928b0fd2dd", "165,1,255,31,87,52,24,113,0,91,148,254,158,2,73,153")]
        public void Part2(string expectedResult, string input)
        {
            var codes = input.ToCharArray().Select(c => (int) c);

            var rejoined = string.Join(",", codes);

            var saltedInput = rejoined + "," + "17, 31, 73, 47, 23";

            var splitCodes = saltedInput.SplitToIntegers(",").ToList();

            var knot = new Knot();

            for (var i=0;i < 64; i++)
            {
                foreach (var length in splitCodes)
                {
                    knot.Flip(length);
                }
            }

            var hashSet = new List<int>();
            var skip = 0;

            Func<IList<int>> toHash = () =>
            {
                var next = knot.List.Skip(skip).Take(16);
                skip += 16;
                return next.ToList();
            };

            while (skip < knot.MaxSize)
            {
                var next = toHash();
                var hashPiece = 0;
                foreach (var piece in next)
                {
                    hashPiece = hashPiece ^ piece;
                }
                hashSet.Add(hashPiece);
            }

            var knotHashPieces = hashSet.Select(p =>
            {
                var piece = p.ToString("X");
                if (piece.Length == 1)
                    piece = "0" + piece;
                return piece;
            });

            var knotHash = string.Join("", knotHashPieces);

            Assert.AreEqual(expectedResult, knotHash.ToLower());
        }

        [TestCase(3, 4, 5, "3, 4, 1, 5")]
        [TestCase(255, 254, 256, "256")]
        public void VerifyKnots(int expectFirst, int expectSecond, int startingLength, string inputLengths)
        {
            var lengths = inputLengths.SplitToIntegers(",");

            var knot = new Knot(startingLength);

            foreach (var length in lengths)
            {
                knot.Flip(length);
            }

            Assert.AreEqual(expectFirst, knot.List.First());
            Assert.AreEqual(expectSecond, knot.List.Skip(1).First());
        }

        public class Knot
        {
            public Knot(int maxSize = 256)
            {
                Init(maxSize);
                List = new List<int>();

                for (var i = 0; i < maxSize; i++)
                    List.Add(i);
            }

            private void Init(int maxSize)
            {
                Direction = 1;
                Index = 0;
                SkipSize = 0;
                MaxSize = maxSize;
            }

            public int MaxSize { get; private set; }
            public List<int> List { get; private set; }
            public int Index { get; private set; }
            public int Direction { get; private set; }
            public int SkipSize { get; private set; }

            public int MultiplyFirstTwo()
            {
                return List.First() * List.Skip(1).First();
            }

            private List<List<int>> Sections(int start, int length)
            {
                var startIndex = start % MaxSize;
                var lastIndex = length + startIndex;
                var marksFromEnd = MaxSize - start;

                var sections = new List<List<int>>();

                var beginning = List.Skip(startIndex);

                var endPortion = beginning.Take(lastIndex > MaxSize ? marksFromEnd : length).ToList();
                var wrappedPortion = List.Take(lastIndex > MaxSize ? length - marksFromEnd : 0).ToList();

                sections.Add(endPortion);
                sections.Add(wrappedPortion);

                return sections.Where(s => s.Count > 0).ToList();
            }

            public void Flip(int length)
            {
                var newSections = Sections(Index, length);

                var reversedSections = newSections.SelectMany(i => i).Reverse().ToList();

                var temp = 0;

                while (temp < length)
                {
                    List[(Index + temp) % MaxSize] = reversedSections[temp];
                    temp++;
                }

                Index = (length + SkipSize + Index) % MaxSize;
                SkipSize++;
            }
        }
    }
}