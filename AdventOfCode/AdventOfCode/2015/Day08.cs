using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode._2015
{
    public class Day08
    {
        private static int CalculateCharLength(string input) =>
            input.Length;

        private static Regex EscapedNumericValues = new("\\\\x[0-9a-f]{2}");

        private static Regex Whitespace = new Regex("\\s+");
        //private static Regex EscapedChars = new("\\\\[^x]");

        private static int CalculateStringLength(string input) =>
            input[1..^1]
                .Replace("\\\\", "\\")
                .Replace("\\\"", "\"")
                .Map(x => EscapedNumericValues.Replace(x, y => "a"))
                .Length;

        private static int CalculateStringLength2(string input)
        {
            var s = "\"\\\"" + input[1..^1]
                                 .Replace("\\", "\\\\")
                                 .Replace("\"", "\\\"")

                             + "\\\"\"";
            return s.Length;
        }

        [Theory]
        [InlineData("\"\"", 2)]
        [InlineData("\"abc\"", 5)]
        [InlineData("\"aaa\\\"aaa\"", 10)]
        [InlineData("\"\\x27\"", 6)]
        public void Day08_Test01(string input, int expectedLength)
        {
            CalculateCharLength(input).Should().Be(expectedLength);
        }

        [Theory]
        [InlineData("\"\"", 0)]
        [InlineData("\"abc\"", 3)]
        [InlineData("\"aaa\\\"aaa\"", 7)]
        [InlineData("\"\\x27\"", 1)]
        public void Day08_Test02(string input, int expectedLength)
        {
            CalculateStringLength(input).Should().Be(expectedLength);
        }

        [Fact]
        public void Day08_Test03()
        {
            var input = new[]
            {
                "\"\"",
                "\"abc\"",
                "\"aaa\\\"aaa\"",
                "\"\\x27\""
            };

            input.Sum(CalculateCharLength).Should().Be(23);
            input.Sum(CalculateStringLength).Should().Be(11);
        }

        [Fact]
        public void Day08_Part01()
        {
            var input = File.ReadAllLines("./2015/Day08Input.txt");
            var charLength = input.Sum(CalculateCharLength);
            var strLength = input.Sum(CalculateStringLength);

            var result = charLength - strLength;
            result.Should().Be(1350);
        }

        [Theory]
        [InlineData("\"\"", 6)]
        [InlineData("\"abc\"", 9)]
        [InlineData("\"aaa\\\"aaa\"", 16)]
        [InlineData("\"\\x27\"", 11)]
        public void Day08_Test04(string input, int expectedLength)
        {
            CalculateStringLength2(input).Should().Be(expectedLength);
        }



        [Fact]
        public void Day08_Part02()
        {
            var input = File.ReadAllLines("./2015/Day08Input.txt");
            var charLength = input.Sum(CalculateCharLength);
            var strLength = input.Sum(CalculateStringLength2);

            var result = strLength - charLength ;
            result.Should().Be(2085);
        }

    }
}
