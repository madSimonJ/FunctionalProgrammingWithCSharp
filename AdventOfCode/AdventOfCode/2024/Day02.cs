namespace AdventOfCode._2024
{
    public static class Day02Solution
    {
        public static bool IsSafe(IEnumerable<int> input) =>
            input
            .Map(x => x.Zip(x.Skip(1)))
            .Select(x => x.First - x.Second)
            .Map(x =>
                x.All(y => y.IsBetween(-3, -1)) ||
                   x.All(y => y.IsBetween(1, 3))
            );


        private static bool IsUnsafeLevel(int from, int to, bool isAscending) =>
            from == to ||
                (isAscending && ((from > to) || (to - from) > 3)) ||
                (!isAscending && ((to > from) || (from - to) > 3));

        public static IEnumerable<int> ParseInput(string input) =>
            input.Split(' ').Select(x => int.Parse(x)).ToArray();

        public static bool IsSafeAfterProblemDampener(IEnumerable<int> input)
        {
            var levels = input.ToArray();
            var isAscending = (levels[1] - levels[0]) > 0;
            var unsafeLevels = Enumerable.Range(0, levels.Length - 1)
                                        .Where(x => IsUnsafeLevel(levels[x], levels[x + 1], isAscending))
                                        .SelectMany(x => new[] { x, x + 1 })
                                        .Concat([0, 1])
                                        .Distinct()
                                        .ToArray();
            var reducedLevelsToCheckAgain = unsafeLevels.Select(x => levels[..x].Concat(levels[(x+1)..])).ToArray();
            var safeAfterProblemDampener = reducedLevelsToCheckAgain.Any(x => IsSafe(x));
            return safeAfterProblemDampener;
        }
                

        public static int CalculateSafeReports(string input) =>
            input.Split(Environment.NewLine).Count(x => IsSafe(ParseInput(x)));

        public static int CalculateSafeReportsWithDampener(string input) =>
            input.Split(Environment.NewLine)
            .Select(x => ParseInput(x))
            .Count(x => IsSafe(x) || IsSafeAfterProblemDampener(x));
           
    }

    public class Day02
    {
        [Theory]
        [InlineData("7 6 4 2 1", true)]
        [InlineData("1 2 7 8 9", false)]
        [InlineData("9 7 6 2 1", false)]
        [InlineData("1 3 2 4 5", false)]
        [InlineData("8 6 4 4 1", false)]
        [InlineData("1 3 6 7 9", true)]
        public void Day02_part01_test01(string input, bool expectedResult)
        {
            var parsedInput = Day02Solution.ParseInput(input);
            var actualResult = Day02Solution.IsSafe(parsedInput);
            actualResult.Should().Be(expectedResult); 
        }

        [Fact]
        public void Day02_part01_test02()
        {
            const string input = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";

            var answer = Day02Solution.CalculateSafeReports(input);
            answer.Should().Be(2);
        }

        [Fact]
        public void Day02_Part01()
        {
            var input = File.ReadAllText("./2024/Day02input.txt");
            var answer = Day02Solution.CalculateSafeReports(input);
            answer.Should().Be(257);
        }

        [Theory]
        [InlineData("1 2 7 8 9", false)]
        [InlineData("9 7 6 2 1", false)]
        [InlineData("1 3 2 4 5", true)]
        [InlineData("8 6 4 4 1", true)]
        public void Day02_part01_test03(string input, bool expectedResult)
        {
            var parsedInput = Day02Solution.ParseInput(input);
            var actualResult = Day02Solution.IsSafeAfterProblemDampener(parsedInput);
            actualResult.Should().Be(expectedResult);
        }

        [Fact]
        public void Day02_part01_test04()
        {
            const string input = @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9";

            var answer = Day02Solution.CalculateSafeReportsWithDampener(input);
            answer.Should().Be(4);
        }

        [Fact]
        public void Day02_Part02()
        {
            var input = File.ReadAllText("./2024/Day02input.txt");
            var answer = Day02Solution.CalculateSafeReportsWithDampener(input);
            answer.Should().Be(328);
        }
    }
}
