using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Puzzles.AdventOfCode.Y2017
{
    [TestFixture]
    public class Day06
    {
        /// <summary>
        /// --- Day 6: Memory Reallocation ---
        /// 
        /// A debugger program here is having an issue: it is trying to repair a memory reallocation routine, but it keeps getting stuck in an infinite loop.
        /// 
        ///     In this area, there are sixteen memory banks; each memory bank can hold any number of blocks.The goal of the reallocation routine is to balance the blocks between the memory banks.
        /// 
        /// 
        ///     The reallocation routine operates in cycles.In each cycle, it finds the memory bank with the most blocks (ties won by the lowest-numbered memory bank) and redistributes those blocks among the banks. To do this, it removes all of the blocks from the selected bank, then moves to the next-highest-indexed memory bank and inserts one of the blocks. It continues doing this until it runs out of blocks; if it reaches the last memory bank, it wraps around to the first one.
        /// 
        ///     The debugger would like to know how many redistributions can be done before a blocks-in-banks configuration is produced that has been seen before.
        /// 
        ///     For example, imagine a scenario with only four memory banks:
        /// 
        /// The banks start with 0, 2, 7, and 0 blocks. The third bank has the most blocks, so it is chosen for redistribution.
        ///     Starting with the next bank (the fourth bank) and then continuing to the first bank, the second bank, and so on, the 7 blocks are spread out over the memory banks. The fourth, first, and second banks get two blocks each, and the third bank gets one back. The final result looks like this: 2 4 1 2.
        /// Next, the second bank is chosen because it contains the most blocks (four). Because there are four memory banks, each gets one block. The result is: 3 1 2 3.
        /// Now, there is a tie between the first and fourth memory banks, both of which have three blocks. The first bank wins the tie, and its three blocks are distributed evenly over the other three banks, leaving it with none: 0 2 3 4.
        /// The fourth bank is chosen, and its four blocks are distributed such that each of the four banks receives one: 1 3 4 1.
        /// The third bank is chosen, and the same thing happens: 2 4 1 2.
        /// At this point, we've reached a state we've seen before: 2 4 1 2 was already seen. The infinite loop is detected after the fifth block redistribution cycle, and so the answer in this example is 5.
        /// 
        /// Given the initial block counts in your puzzle input, how many redistribution cycles must be completed before a configuration is produced that has been seen before?
        /// </summary>
        [TestCase(5, "0 2 7 0")]
        [TestCase(6681, PuzzleInput)]
        public void Part1(int iterationsToDetect, string input)
        {
            var banks = BuildMemoryBanks(input);
            var iterations = 0;

            var states = new HashSet<string>();

            while (true)
            {
                var state = MemoryState(banks);

                if (states.Contains(state))
                    break;

                states.Add(state);

                var toDistribute = banks.OrderByDescending(b => b.Blocks).ThenBy(b => b.Index).First();

                var numberToRedistribute = toDistribute.Blocks;
                toDistribute.Blocks = 0;

                var currentBlock = toDistribute.Next;

                while (numberToRedistribute > 0)
                {
                    currentBlock.Blocks++;
                    numberToRedistribute--;
                    currentBlock = currentBlock.Next;
                }

                iterations++;
            }

            Assert.AreEqual(expected: iterationsToDetect, actual: iterations);
        }

        private static List<MemoryBank> BuildMemoryBanks(string input)
        {
            var blocks = input.SplitToIntegers().ToArray();
            var banks = new List<MemoryBank>();

            for (var i = 0; i < blocks.Length; i++)
            {
                banks.Add(new MemoryBank()
                {
                    Index = i,
                    Blocks = blocks[i],
                });
            }

            foreach (var bank in banks)
            {
                bank.Next = banks[(bank.Index + 1) % banks.Count];
            }
            return banks;
        }

        public static string MemoryState(IEnumerable<MemoryBank> banks)
        {
            var strings = banks.OrderBy(b => b.Index).Select(b => b.Blocks.ToString());
            return string.Join(" ", strings);
        }

        public class MemoryBank
        {
            public int Index { get; set; }
            public int Blocks { get; set; }
            public MemoryBank Next { get; set; }
        }

        private const string PuzzleInput = "4  1   15  12  0   9   9   5   5   8   7   3   14  5   12  3";

        /// <summary>
        /// Out of curiosity, the debugger would also like to know the size of the loop: starting from a state that has already been seen, how many block redistribution cycles must be performed before that same state is seen again?
        /// 
        /// In the example above, 2 4 1 2 is seen again after four cycles, and so the answer in that example would be 4.
        /// 
        /// How many cycles are in the infinite loop that arises from the configuration in your puzzle input?
        /// </summary>
        [TestCase(4, "2 4 1 2")]
        [TestCase(2392, PuzzleInput)]
        public void Part2(int iterationsToDetect, string input)
        {
            var banks = BuildMemoryBanks(input);
            var iterationsSinceEncounter = 0;

            var states = new HashSet<string>();
            string stateEncountered = null;

            while (true)
            {
                var state = MemoryState(banks);

                if (stateEncountered == state)
                {
                    break;
                }

                if (stateEncountered == null && states.Contains(state))
                {
                    stateEncountered = state;
                }

                states.Add(state);

                var toDistribute = banks.OrderByDescending(b => b.Blocks).ThenBy(b => b.Index).First();

                var numberToRedistribute = toDistribute.Blocks;
                toDistribute.Blocks = 0;

                var currentBlock = toDistribute.Next;

                while (numberToRedistribute > 0)
                {
                    currentBlock.Blocks++;
                    numberToRedistribute--;
                    currentBlock = currentBlock.Next;
                }

                if (stateEncountered != null)
                    iterationsSinceEncounter++;
            }

            Assert.AreEqual(expected: iterationsToDetect, actual: iterationsSinceEncounter);
        }
    }
}