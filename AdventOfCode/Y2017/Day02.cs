using System.Linq;
using NUnit.Framework;

namespace Puzzles.AdventOfCode.Y2017
{

    [TestFixture]
    public class Day02
    {
        /*
 --- Day 2: Corruption Checksum ---

As you walk through the door, a glowing humanoid shape yells in your direction. 
"You there! Your state appears to be idle. 
Come help us repair the corruption in this spreadsheet - if we take another millisecond, we'll have to display an hourglass cursor!"

The spreadsheet consists of rows of apparently-random numbers. 
To make sure the recovery process is on the right track, they need you to calculate the spreadsheet's checksum. 
For each row, determine the difference between the largest value and the smallest value; 
the checksum is the sum of all of these differences.

For example, given the following spreadsheet:

5 1 9 5
7 5 3
2 4 6 8
The first row's largest and smallest values are 9 and 1, and their difference is 8.
The second row's largest and smallest values are 7 and 3, and their difference is 4.
The third row's difference is 6.
In this example, the spreadsheet's checksum would be 8 + 4 + 6 = 18.

What is the checksum for the spreadsheet in your puzzle input?
         */
        [TestCase(18, @"5 1 9 5
7 5 3
2 4 6 8")]
        [TestCase(21845, @"493	458	321	120	49	432	433	92	54	452	41	461	388	409	263	58
961	98	518	188	958	114	1044	881	948	590	972	398	115	116	451	492
76	783	709	489	617	72	824	452	748	737	691	90	94	77	84	756
204	217	90	335	220	127	302	205	242	202	259	110	118	111	200	112
249	679	4015	106	3358	1642	228	4559	307	193	4407	3984	3546	2635	3858	924
1151	1060	2002	168	3635	3515	3158	141	4009	3725	996	142	3672	153	134	1438
95	600	1171	1896	174	1852	1616	928	79	1308	2016	88	80	1559	1183	107
187	567	432	553	69	38	131	166	93	132	498	153	441	451	172	575
216	599	480	208	224	240	349	593	516	450	385	188	482	461	635	220
788	1263	1119	1391	1464	179	1200	621	1304	55	700	1275	226	57	43	51
1571	58	1331	1253	60	1496	1261	1298	1500	1303	201	73	1023	582	69	339
80	438	467	512	381	74	259	73	88	448	386	509	346	61	447	435
215	679	117	645	137	426	195	619	268	223	792	200	720	260	303	603
631	481	185	135	665	641	492	408	164	132	478	188	444	378	633	516
1165	1119	194	280	223	1181	267	898	1108	124	618	1135	817	997	129	227
404	1757	358	2293	2626	87	613	95	1658	147	75	930	2394	2349	86	385")]
        public void Part1(int expectedResult, string input)
        {
            var checkSum = 0;

            var rows = input.EnumerizeLines();

            foreach (var row in rows.SplitOnWhitespace())
            {
                var numbers =
                    row.Select(int.Parse)
                       .OrderBy(i => i);

                checkSum += numbers.Last() - numbers.First();
            }

            Assert.AreEqual(expectedResult, checkSum);
        }

        /*
--- Part Two ---

"Great work; looks like we're on the right track after all. Here's a star for your effort." 
However, the program seems a little worried. Can programs be worried?

"Based on what we're seeing, it looks like all the User wanted is some information about the evenly divisible values in the spreadsheet. 
Unfortunately, none of us are equipped for that kind of calculation - most of us specialize in bitwise operations."

It sounds like the goal is to find the only two numbers in each row where one evenly divides the other - that is, 
where the result of the division operation is a whole number. 
They would like you to find those numbers on each line, divide them, and add up each line's result.

For example, given the following spreadsheet:

5 9 2 8
9 4 7 3
3 8 6 5
In the first row, the only two numbers that evenly divide are 8 and 2; the result of this division is 4.
In the second row, the two numbers are 9 and 3; the result is 3.
In the third row, the result is 2.
In this example, the sum of the results would be 4 + 3 + 2 = 9.

What is the sum of each row's result in your puzzle input?
         */
        [TestCase(9, @"5 9 2 8
9 4 7 3
3 8 6 5")]
        [TestCase(191, @"493	458	321	120	49	432	433	92	54	452	41	461	388	409	263	58
961	98	518	188	958	114	1044	881	948	590	972	398	115	116	451	492
76	783	709	489	617	72	824	452	748	737	691	90	94	77	84	756
204	217	90	335	220	127	302	205	242	202	259	110	118	111	200	112
249	679	4015	106	3358	1642	228	4559	307	193	4407	3984	3546	2635	3858	924
1151	1060	2002	168	3635	3515	3158	141	4009	3725	996	142	3672	153	134	1438
95	600	1171	1896	174	1852	1616	928	79	1308	2016	88	80	1559	1183	107
187	567	432	553	69	38	131	166	93	132	498	153	441	451	172	575
216	599	480	208	224	240	349	593	516	450	385	188	482	461	635	220
788	1263	1119	1391	1464	179	1200	621	1304	55	700	1275	226	57	43	51
1571	58	1331	1253	60	1496	1261	1298	1500	1303	201	73	1023	582	69	339
80	438	467	512	381	74	259	73	88	448	386	509	346	61	447	435
215	679	117	645	137	426	195	619	268	223	792	200	720	260	303	603
631	481	185	135	665	641	492	408	164	132	478	188	444	378	633	516
1165	1119	194	280	223	1181	267	898	1108	124	618	1135	817	997	129	227
404	1757	358	2293	2626	87	613	95	1658	147	75	930	2394	2349	86	385")]
        public void Part2(int expectedResult, string input)
        {
            var checkSum = 0;

            var rows = input.EnumerizeLines();

            foreach (var row in rows.SplitOnWhitespace())
            {
                var numbers =
                    row.Select(int.Parse)
                        .OrderByDescending(i => i);

                int divisor = 0;
                var divideable = numbers.First(n =>
                {
                    divisor = numbers.SingleOrDefault(d => d != n && n % d == 0);
                    return divisor != 0;
                });

                checkSum += (divideable / divisor);
            }

            Assert.AreEqual(expectedResult, checkSum);
        }
    }
}