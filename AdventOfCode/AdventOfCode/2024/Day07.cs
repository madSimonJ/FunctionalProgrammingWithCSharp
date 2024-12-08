using AdventOfCode._2024;
using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{

    public readonly record struct Day07State
    {
        public required long Total { get; init; }
        public required IEnumerable<long> RemainingNumbers { get; init; }
    }

    public static class Day07Solution
    {
        public static IEnumerable<Day07State> ParseInput(string input)
        {
            return input.Split("\r\n").Select(x =>
                x.Split(":")
                    .Map(y =>
                    {
                        return (
                            Total: long.Parse(y[0]),
                            Numbers: y[1].Trim().Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(z => long.Parse(z))
                    );
                    })
                    .Map(x => new Day07State { RemainingNumbers = x.Numbers, Total = x.Total })
            );


        }

        public static bool EquationIsPossible(Day07State state) =>
            state.RemainingNumbers.ToArray()
                .Map(x =>
                    x.Length == 2
                        ? x[0] * x[1] == state.Total || x[0] + x[1] == state.Total
                        : EquationIsPossible(state with { RemainingNumbers = state.RemainingNumbers.Skip(2).Prepend(x[0] * x[1]) })
                               || EquationIsPossible(state with { RemainingNumbers = state.RemainingNumbers.Skip(2).Prepend(x[0] + x[1]) })
                );

        public static bool EquationIsPossible2(Day07State state) => state.RemainingNumbers.ToArray()
                .Map(x =>
                     x.Length == 2
                        ? x[0] * x[1] == state.Total || x[0] + x[1] == state.Total || long.Parse(x[0].ToString() + x[1].ToString()) == state.Total
                        : EquationIsPossible2(state with { RemainingNumbers = state.RemainingNumbers.Skip(2).Prepend(x[0] * x[1]) })
                               || EquationIsPossible2(state with { RemainingNumbers = state.RemainingNumbers.Skip(2).Prepend(x[0] + x[1]) })
                               || EquationIsPossible2(state with { RemainingNumbers = state.RemainingNumbers.Skip(2).Prepend(long.Parse(x[0].ToString() + x[1].ToString())) })
                );

        public static long CalculateAnswer(string input) =>
                 Day07Solution.ParseInput(input)
                    .Where(EquationIsPossible)
                        .Sum(x => x.Total);

        internal static long CalculateAnswePart2(string input) =>
            Day07Solution.ParseInput(input)
                    .Where(EquationIsPossible2)
                        .Sum(x => x.Total);
    }
    public class Day07
    {
        [Fact]
        public void Day07_part01_test01()
        {
            var input = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20";

            var state = Day07Solution.ParseInput(input);
            state.Should().BeEquivalentTo([
                new Day07State
            {
                Total = 190,
                RemainingNumbers = [10, 19]
            },
            new Day07State
            {
                Total = 3267,
                RemainingNumbers = [81, 40, 27]
            },
            new Day07State
            {
                Total = 83,
                RemainingNumbers = [17, 5]
            },
            new Day07State
            {
                Total = 156,
                RemainingNumbers = [15, 6]
            },
            new Day07State
            {
                Total = 7290,
                RemainingNumbers = [6, 8, 6, 15]
            },
            new Day07State
            {
                Total = 161011,
                RemainingNumbers = [16, 10, 13]
            },
            new Day07State
            {
                Total = 192,
                RemainingNumbers = [17, 8, 14]
            },
            new Day07State
            {
                Total = 21037,
                RemainingNumbers = [9, 7, 18, 13]
            },
            new Day07State
            {
                Total = 292,
                RemainingNumbers = [11, 6, 16, 20]
            }
            ]);


        }

        [Theory]
        [InlineData("190: 10 19", true)]
        [InlineData("3267: 81 40 27", true)]
        [InlineData("83: 17 5", false)]
        [InlineData("156: 15 6", false)]
        [InlineData("7290: 6 8 6 15", false)]
        [InlineData("161011: 16 10 13", false)]
        [InlineData("192: 17 8 14", false)]
        [InlineData("21037: 9 7 18 13", false)]
        [InlineData("292: 11 6 16 20", true)]
        public void Day07_Part01_Test02(string input, bool expectedResult)
        {
            var parsedInput = Day07Solution.ParseInput(input).First();
            var result = Day07Solution.EquationIsPossible(parsedInput);
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void Day07_Part01_Test03()
        {
            var input = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20";
            var answer = Day07Solution.CalculateAnswer(input);
            answer.Should().Be(3749);
        }

        [Fact]
        public void Day07_Part01()
        {
            var input = File.ReadAllText("./2024/Day07input.txt");
            var answer = Day07Solution.CalculateAnswer(input);
            answer.Should().Be(850435817339L);
 
        }


        [Fact]
        public void Day07_Part02_Test01()
        {
            var input = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20";
            var answer = Day07Solution.CalculateAnswePart2(input);
            answer.Should().Be(11387);
        }


        [Fact]
        public void Day07_Part02()
        {
            var input = File.ReadAllText("./2024/Day07input.txt");
            var answer = Day07Solution.CalculateAnswePart2(input);
            answer.Should().Be(104824810233437L);
        }
    }
}