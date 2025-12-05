namespace AdventOfCode._2025
{
    public static class Day03Solution
    {
        public static int FindBiggestPossibleValue(string input)
        {
            var indexedValues = input[..^1].Select((x, i) => (x, i));
            var maxValue = indexedValues.Max(x => x.x);
            var firstMax = indexedValues.First(x => x.x == maxValue);
                                        
            var secondLargestValue = input[(firstMax.i+ 1)..].Max();
            var finalValue = (int.Parse(firstMax.x.ToString()) * 10) + int.Parse(secondLargestValue.ToString());
            return finalValue;
        }

        public static ulong FindBiggestPossibleValue2(string input, int numValues) =>
            Enumerable.Range(0, numValues)
            .Reverse()
            .Aggregate((Total: 0ul, LastChar: 0), (agg, x) =>
            {
                var stringWindow = input[agg.LastChar..^x];
                var maxValue = stringWindow.Max();
                var firstMax = stringWindow.IndexOf(maxValue);
                return (agg.Total + (ulong)(Math.Pow(10, x) * int.Parse(maxValue.ToString())), agg.LastChar + firstMax + 1);
            }).Total;

        public static ulong CalculateOutputJoltage(string input, bool isPart1 = true) =>
            input.Split(Environment.NewLine)
            .Select(x => FindBiggestPossibleValue2(x, isPart1 ? 2 : 12))
            .Aggregate(0UL, (agg, x) => agg + x);
    }

    public class Day03
    {
        [Theory]
        [InlineData("987654321111111", 98UL)]
        [InlineData("811111111111119", 89UL)]
        [InlineData("234234234234278", 78UL)]
        [InlineData("818181911112111", 92UL)]
        public void Day03_Test01(string input, ulong expectedOutput)
        {
            var actualOutput = Day03Solution.FindBiggestPossibleValue2(input, 2);
            actualOutput.Should().Be(expectedOutput);
        }

        [Fact]
        public void Day03_Test02()
        {
            var input = @"987654321111111
811111111111119
234234234234278
818181911112111";

            var output = Day03Solution.CalculateOutputJoltage(input);
            output.Should().Be(357UL);
        }

        [Fact]
        public void Day03_Part01()
        {
            var input = File.ReadAllText("./2025/Day03input.txt");
            var answer = Day03Solution.CalculateOutputJoltage(input);

            answer.Should().Be(17613UL);
        }

        [Theory]
        [InlineData("987654321111111", 987654321111)]
        [InlineData("811111111111119", 811111111119)]
        [InlineData("234234234234278", 434234234278)]
        [InlineData("818181911112111", 888911112111)]
        public void Day03_Test04(string input, ulong expectedOutput)
        {
            var actualOutput = Day03Solution.FindBiggestPossibleValue2(input, 12);
            actualOutput.Should().Be(expectedOutput);
        }

        [Fact]
        public void Day03_Test03()
        {
            var input = @"987654321111111
811111111111119
234234234234278
818181911112111";

            var output = Day03Solution.CalculateOutputJoltage(input, true);
            output.Should().Be(357UL);
        }

        [Fact]
        public void Day03_Part02()
        {
            var input = File.ReadAllText("./2025/Day03input.txt");
            var answer = Day03Solution.CalculateOutputJoltage(input, false);

            answer.Should().Be(17613UL);
        }
    }
}
