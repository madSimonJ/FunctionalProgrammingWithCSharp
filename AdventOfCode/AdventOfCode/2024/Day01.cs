using System.Text;

namespace AdventOfCode._2024
{
    public static class Day01Solution
    {
        public static (int, int) ParseLine(string input) =>
            input.Split("   ", StringSplitOptions.RemoveEmptyEntries)
            .Map(x => x.Select(y => int.Parse(y)))
            .Map(x => x.ToArray())
            .Map(x => (x[0], x[1]));

        public static (IEnumerable<int>, IEnumerable<int>) ParseLines(string input) =>
            input.Split(Environment.NewLine)
                .Select(x => ParseLine(x))
                .Aggregate(
                    (Enumerable.Empty<int>(), Enumerable.Empty<int>()),
                    (acc, x) => (
                        acc.Item1.Append(x.Item1),
                        acc.Item2.Append(x.Item2)
                    ));

        public static int LineAnswer(int a, int b) =>
            Math.Abs(a - b);

        public static int SetAnswer(string input) =>
            0;

        internal static int LineAnswerTotal(IEnumerable<int> input1, IEnumerable<int> input2) =>
            input1.Order()
                .Zip(input2.Order())
                .Sum(x => LineAnswer(x.First, x.Second));

        public static int CalculateAnswer(string input) =>
            ParseLines(input)
            .Map(x => LineAnswerTotal(x.Item1, x.Item2));

        internal static int CalculateSimularityScore(string input) =>
            ParseLines(input)
                .Map(x => (
                    x.Item1,
                    x.Item2.GroupBy(x => x)
                            .Select(y => (Key: y.Key, Count: y.Count()))
                            .ToDictionary(x => x.Key, x => x.Count)
                            .ToLookupWithDefault(x => 0)
                ))
            .Map(x =>
                x.Item1.Select(y => (Value: y, Count: x.Item2(y)))
            ).Sum(x => x.Count * x.Value);
    }

    public class Day01
    {

        [Theory]
        [InlineData("3   4", 3, 4)]
        [InlineData("4   3", 4, 3)]
        [InlineData("2   5", 2, 5)]
        [InlineData("1   3", 1, 3)]
        [InlineData("3   9", 3, 9)]
        [InlineData("3   3", 3, 3)]
        [InlineData("63721   98916", 63721, 98916)]
        public void Day01_Part01_Test01(string input, int expected1, int expected2)
        {
            Day01Solution.ParseLine(input).Should().Be((expected1, expected2));
        }

        [Fact]
        public void Day01_Part01_Test02()
        {
            var input = @"3   4
4   3
2   5
1   3
3   9
3   3";
            var actualAnswer = Day01Solution.ParseLines(input);
            actualAnswer.Item1.Should().BeEquivalentTo([3, 4, 2, 1, 3, 3]);
            actualAnswer.Item2.Should().BeEquivalentTo([4, 3, 5, 3, 9, 3]);
        }
        [Theory]
        [InlineData(1, 3, 2)]
        [InlineData(2, 3, 1)]
        [InlineData(3, 3, 0)]
        [InlineData(3, 4, 1)]
        [InlineData(3, 5, 2)]
        [InlineData(4, 9, 5)]
        [InlineData(9, 4, 5)]

        public void Day01_Part01_Test03(int input1, int input2, int expectedAnswer)
        {
            var answer = Day01Solution.LineAnswer(input1, input2);
            answer.Should().Be(expectedAnswer);
        }

        [Fact]
        public void Day01_Part01_Test04()
        {
            var input = @"3   4
4   3
2   5
1   3
3   9
3   3";
            var actualAnswer = Day01Solution.CalculateAnswer(input);
            actualAnswer.Should().Be(11);
        }


        [Fact]
        public void Day01_Part01()
        {
            var input = File.ReadAllText("./2024/Day01input.txt");
            var answer = Day01Solution.CalculateAnswer(input);
            answer.Should().Be(1341714);
        }

        [Fact]
        public void Day01_Part01_Test05()
        {
            var input = @"3   4
4   3
2   5
1   3
3   9
3   3";
            var actualAnswer = Day01Solution.CalculateSimularityScore(input);
            actualAnswer.Should().Be(31);
        }

        [Fact]
        public void Day01_Part02()
        {
            var input = File.ReadAllText("./2024/Day01input.txt");
            var answer = Day01Solution.CalculateSimularityScore(input);
            answer.Should().Be(27384707);
        }
    }
}
