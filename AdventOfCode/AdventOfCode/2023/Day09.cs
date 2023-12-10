using AdventOfCode.Common;
using MoreLinq;
using System.Text.RegularExpressions;

namespace AdventOfCode._2023
{
    public class Day09
    {
        private static long GetLineAnswer(string input)
        {
            return input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .Map(x => new[] { x })
                .IterateUntil(x => x.Last().All(y => y == 0), x =>
                {
                    var newRow = x.Last().Pairwise((a, b) => b - a).ToArray();
                    return x.Append(newRow).ToArray();
                }).Sum(x => x.Any() ? x.Last() : 0);
        }

        private static long GetLineAnswer2(string input)
        {
            var history = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .Map(x => new[] { x })
                .IterateUntil(x => x.Last().All(y => y == 0), x =>
                {
                    var newRow = x.Last().Pairwise((a, b) => b - a).ToArray();
                    return x.Append(newRow).ToArray();
                });
            var ans = history.Select(x => x.First());
            var ans2 = ans.Reverse().Aggregate((Total: 0L, LastNum: 0L), (agg, x) =>
            {
                var newNum = x - agg.LastNum;
                return (agg.Total + newNum, newNum);
            });
            return ans2.LastNum;
        }

        [Theory]
        [InlineData("0 3 6 9 12 15", 18)]
        [InlineData("1 3 6 10 15 21", 28)]
        [InlineData("10 13 16 21 30 45", 68)]
        public void Day09_Test01(string input, long expectedAnswer)
        {
            var a = input.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(long.Parse)
                .Map(x => new[] { x })
                .IterateUntil(x => x.Last().All(y => y == 0), x =>
                {
                    var newRow = x.Last().Pairwise((a, b) => b - a).ToArray();
                    return x.Append(newRow).ToArray();
                }).Sum(x => x.Last());

            a.Should().Be(expectedAnswer);
        }

        [Fact]
        public void Day09_Test02()
        {
            var input = @"0 3 6 9 12 15
1 3 6 10 15 21
10 13 16 21 30 45".Split(Environment.NewLine);
            var answer = input.Sum(GetLineAnswer);


            answer.Should().Be(114);
        }

        [Fact]
        public void Day09_Part01()
        {
            var input = File.ReadAllLines("./2023/Day09Input.txt");
            var answer = input.Select(GetLineAnswer).ToArray();
            var s = string.Join(Environment.NewLine, answer);


            answer.Sum().Should().Be(1757008019);
        }

        [Fact]
        public void Day09_Part02()
        {
            var input = File.ReadAllLines("./2023/Day09Input.txt");
            var answer = input.Select(GetLineAnswer2).ToArray();
            var s = string.Join(Environment.NewLine, answer);


            answer.Sum().Should().Be(995);
        }

        [Fact]
        public void Day09_Test03()
        {
            const string input = "10 13 16 21 30 45";
            var answer = GetLineAnswer2(input);
            answer.Should().Be(5);

        }
    }


}
