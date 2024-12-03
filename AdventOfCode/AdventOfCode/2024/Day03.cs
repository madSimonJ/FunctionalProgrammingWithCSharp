using AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace AdventOfCode._2024
{
    public readonly record struct Day03State
    {
        public uint CurrentTotal { get; init; }
        public bool IsOn { get; init; }
    }

    public static class Day03Solution
    {
        private static Regex MatchMulOnly = new Regex("mul[(][0-9]*,[0-9]*[)]");
        private static Regex MatchMulAndDo = new Regex("mul[(][0-9]*,[0-9]*[)]|do[(][)]|don[']t[(][)]");


        public static IEnumerable<string> GetCommands(string input, bool includeDoDont = false) =>
           (includeDoDont
                ? MatchMulAndDo.Matches(input)
                : MatchMulOnly.Matches(input)).Select(x => x.Value);

        public static (int, int) ParseCommand(string input) =>
            input.Replace("mul(", string.Empty)
                .Replace(")", string.Empty)
                .Split(",")
                .Select(x => x.Trim())
                .ToArray()
                .Map(x => (x[0], x[1]))
                .Map(x => (int.Parse(x.Item1), int.Parse(x.Item2)));

        public static uint CalculateAnswer(string input) =>
            GetCommands(input)
            .Select(x => ParseCommand(x))
            .Aggregate(0u, (acc, x) => acc + ((uint)x.Item1 * (uint)x.Item2));

        public static uint CalculateAnswerWithState(string input) =>
            GetCommands(input, true)
            .Aggregate(
                new Day03State
                {
                    CurrentTotal = 0,
                    IsOn = true
                },
                (acc, x) => x switch
                {
                    "do()" => acc with {  IsOn = true },
                    "don't()" => acc with {  IsOn = false },
                    _ => acc with { CurrentTotal = acc.CurrentTotal + (uint)(acc.IsOn ? ParseCommand(x).Map(x => (uint)x.Item1 * (uint)x.Item2) : 0) }
                }
            ).CurrentTotal;

    }

    public class Day03
    {
        [Fact]
        public void Day03_Part01_Test01()
        {
            var input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

            var answer = Day03Solution.GetCommands(input);
            answer.Should().BeEquivalentTo([
                "mul(2,4)",
                "mul(5,5)",
                "mul(11,8)",
                "mul(8,5)"
                ]);
        }

        [Fact]
        public void Day03_Part01_Test02()
        {
            var input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

            var answer = Day03Solution.GetCommands(input)
                .Select(x => Day03Solution.ParseCommand(x));
            answer.Should().BeEquivalentTo([
                (2,4),
                (5,5),
                (11,8),
                (8,5)
                ]);
        }

        [Fact]
        public void Day03_Part01_Test03()
        {
            var input = "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))";

            var answer = Day03Solution.CalculateAnswer(input);
            answer.Should().Be(161);
        }

        [Fact]
        public void Day03_Part01()
        {
            var input = File.ReadAllText("./2024/Day03input.txt");

            var answer = Day03Solution.CalculateAnswer(input);
            answer.Should().Be(183788984u);
        }

        [Fact]
        public void Day03_Part02_Test01()
        {
            var input = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

            var answer = Day03Solution.GetCommands(input, true);
            answer.Should().BeEquivalentTo([
                "mul(2,4)",
                "don't()",
                "mul(5,5)",
                "mul(11,8)",
                "do()",
                "mul(8,5)"
                ]);
        }

        [Fact]
        public void Day03_Part02_Test02()
        {
            var input = "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

            var answer = Day03Solution.CalculateAnswerWithState(input);
            answer.Should().Be(48);
        }

        [Fact]
        public void Day03_Part02()
        {
            var input = File.ReadAllText("./2024/Day03input.txt");

            var answer = Day03Solution.CalculateAnswerWithState(input);
            answer.Should().Be(62098619u);
        }
    }
}
