using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;

namespace Puzzles.AdventOfCode.Y2017
{
    /*
You come across an experimental new kind of memory stored on an infinite two-dimensional grid.

Each square on the grid is allocated in a spiral pattern starting at a location marked 1 and then counting up while spiraling outward. For example, the first few squares are allocated like this:

17  16  15  14  13
18   5   4   3  12
19   6   1   2  11
20   7   8   9  10
21  22  23---> ...
While this is very space-efficient (no squares are skipped), requested data must be carried back to square 1 (the location of the only access port for this memory system) by programs that can only move up, down, left, or right. They always take the shortest path: the Manhattan Distance between the location of the data and square 1.

For example:

Data from square 1 is carried 0 steps, since it's at the access port.
Data from square 12 is carried 3 steps, such as: down, left, left.
Data from square 23 is carried only 2 steps: up twice.
Data from square 1024 must be carried 31 steps.
How many steps are required to carry the data from the square identified in your puzzle input all the way to the access port?

Your puzzle input is 325489.
     */
    [TestFixture]
    public class Day03
    {
        public class Layer
        {
            public int Index { get; set; }
            public int Minimum { get; set; }
        }

        public static string SpiralString =
            @"
37 36  35  34  33  32  31   
38 17  16  15  14  13  30   55
39 18   5   4   3  12  29   54
40 19   6   1   2  11  28   53
41 20   7   8   9  10  27   52
42 21  22  23  24  25  26   51
43 44  45  46  47  48  49   50   
";

        [TestCase(0, 1)]
        [TestCase(3, 12)]
        [TestCase(2, 23)]
        [TestCase(7, 50)]
        [TestCase(6, 51)]
        [TestCase(4, 53)]
        [TestCase(31, 1024)]
        [TestCase(552, 325489)]
        public void Part1(int expectedSteps, int numberToSeek)
        {
            var layer = new Layer()
            {
                Index = 0,
                Minimum = 2,
            };

            var lastMinimum = 1;
            while (numberToSeek >= layer.Minimum)
            {
                lastMinimum = layer.Minimum;
                layer.Minimum += 8 * ++layer.Index;
            }
            layer.Minimum = lastMinimum; // Reset, we went one too far

            int actualSteps = 0;
            if (layer.Index > 0)
            {
                var lengthOfSide = 1 + (layer.Index * 2);
                var maxStepsToMiddle = (lengthOfSide) / 2;
                var stepsFromBottomRight = numberToSeek - layer.Minimum + 1;
                var stepsFromEdge = stepsFromBottomRight % (lengthOfSide - 1);
                var stepsFromCenter = Math.Abs(stepsFromEdge - maxStepsToMiddle);
                actualSteps = stepsFromCenter + layer.Index;
            }

            Assert.AreEqual(expected: expectedSteps, actual: actualSteps);
        }

        /*
  --- Part Two ---

As a stress test on the system, the programs here clear the grid and then store the value 1 in square 1. Then, in the same allocation order as shown above, they store the sum of the values in all adjacent squares, including diagonals.

So, the first few squares' values are chosen as follows:

Square 1 starts with the value 1.
Square 2 has only one adjacent filled square (with value 1), so it also stores 1.
Square 3 has both of the above squares as neighbors and stores the sum of their values, 2.
Square 4 has all three of the aforementioned squares as neighbors and stores the sum of their values, 4.
Square 5 only has the first and fourth squares as neighbors, so it gets the value 5.
Once a square is written, its value does not change. Therefore, the first few squares would receive the following values:

147  142  133  122   59
304    5    4    2   57
330   10    1    1   54
351   11   23   25   26
362  747  806--->   ...
What is the first value written that is larger than your puzzle input?

Your puzzle input is still 325489.       
*/


        public class SpiralPoint
        {
            public int Y { get; set; }
            public int X { get; set; }
            public int Value { get; set; }
            public SpiralPoint Next { get; set; }
            public SpiralPoint Previous { get; set; }
        }

        public SpiralPoint FindNext(int numberToSeek)
        {
            var spiral = new SpiralPoint()
            {
                Value = 1
            };

            var spiralPoints = new List<SpiralPoint>();

            Func<int, int, int> getPointValue = (x, y) =>
            {
                return spiralPoints.SingleOrDefault(p => p.X == x && p.Y == y)?.Value ?? 0;
            };

            Func<SpiralPoint, SpiralPoint> next = thisPoint =>
            {
                spiralPoints.Add(thisPoint);

                var xValues = spiralPoints.Select(p => p.X).ToList();
                var yValues = spiralPoints.Select(p => p.Y).ToList();
                var maxX = xValues.Max();
                var maxY = yValues.Max();
                var minX = xValues.Min();
                var minY = yValues.Min();

                var goingUp = maxX.AbsGreaterThan(maxY, minY, minX);

                var goingLeft = maxY.AbsGreaterThan(minY, minX) && maxY == maxX;

                var goingDown = Day3Helpers.AbsEqual(minX, maxX) && !Day3Helpers.AbsEqual(minX, minY);

                var goingRight = Day3Helpers.AbsEqual(minY, minX) && Math.Abs(minY) == Math.Abs(maxX);

                var nextPoint = new SpiralPoint()
                {
                    X = thisPoint.X,
                    Y = thisPoint.Y
                };

                if (goingDown)
                    nextPoint.Y--;
                if (goingUp)
                    nextPoint.Y++;
                if (goingRight)
                    nextPoint.X++;
                if (goingLeft)
                    nextPoint.X--;

                thisPoint.Next = nextPoint;
                nextPoint.Previous = thisPoint;

                var right = getPointValue(nextPoint.X + 1, nextPoint.Y);
                var topRight = getPointValue(nextPoint.X + 1, nextPoint.Y + 1);
                var top = getPointValue(nextPoint.X, nextPoint.Y + 1);
                var topLeft = getPointValue(nextPoint.X - 1, nextPoint.Y + 1);
                var left = getPointValue(nextPoint.X -1, nextPoint.Y);
                var bottomLeft = getPointValue(nextPoint.X - 1, nextPoint.Y - 1);
                var bottom = getPointValue(nextPoint.X, nextPoint.Y - 1);
                var bottomRight = getPointValue(nextPoint.X + 1, nextPoint.Y - 1);

                var nextValue = right + topRight + top + topLeft + left + bottomLeft + bottom + bottomRight;

                nextPoint.Value = nextValue;

                return nextPoint;
            };

            var spiralPoint = spiral;

            while (spiralPoint.Value <= numberToSeek)
            {
                spiralPoint = next(spiralPoint);
            }

            return spiralPoint;
        }

        private static readonly Tuple<int, int> Right = new Tuple<int, int>(1, 0);
        private static readonly Tuple<int, int> Left = new Tuple<int, int>(-1, 0);
        private static readonly Tuple<int, int> Up = new Tuple<int, int>(0, 1);
        private static readonly Tuple<int, int> Down = new Tuple<int, int>(0, -1);




        private static string Part2Visual = @"
      6151   5729   5332     5018   2448
147    142    133    122     59     2389
304      5      4      2     57     2273
330     10     c1      1     54     2103
351     11     23     25     26     1966
362    747    806     879    930    956
";

        [TestCase(2, 4)]
        [TestCase(23, 25)]
        [TestCase(304, 330)]
        [TestCase(362, 747)]
        [TestCase(325489, 0)]
        public void Part2(int numberToSeek, int expectedNextValue)
        {
            var desiredPoint = FindNext(numberToSeek);
            Assert.AreEqual(actual: desiredPoint.Value, expected: expectedNextValue);
        }
    }

    public static class Day3Helpers
    {
        public static bool AbsEqual(params int[] values)
        {
            var first = Math.Abs(values[0]);
            return values.All(v => first == Math.Abs(v));
        }

        public static bool AbsGreaterThan(this int compare, params int[] values)
        {
            var greater = true;

            foreach (var value in values)
            {
                greater = greater && (Math.Abs(value) < Math.Abs(compare));
            }

            return greater;
        }
    }
}