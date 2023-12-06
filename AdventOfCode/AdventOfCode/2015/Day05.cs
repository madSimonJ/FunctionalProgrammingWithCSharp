using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode._2015
{
    public class Day05
    {
        private static bool IsNice(string input) => input.Validate(
                x => x.Sum(x => "aeiou".Contains(x) ? 1 : 0 ) >= 3,
                x => x.ConsecutiveAny((a, b) =>
                {
                    return a == b;
                }),
                x => !x.Contains("ab"),
                x => !x.Contains("cd"),
                x => !x.Contains("pq"),
                x => !x.Contains("xy")
                
        );

        private static bool IsNiceV2(string input) => input.Validate(
            x => Regex.IsMatch(x, "(([a-z])([a-z])).*\\1"),
            x => Regex.IsMatch(x, "([a-z]).\\1")
        );


        [Theory]
        [InlineData("ugknbfddgicrmopn", true)]
        [InlineData("aaa", true)]
        [InlineData("jchzalrnumimnmhp", false)]
        [InlineData("haegwjzuvuyypxyu", false)]
        [InlineData("dvszwmarrgswjxmb", false)]
        public void Day05_Test01(string input, bool expectedResult)
        {
            IsNice(input).Should().Be(expectedResult);
        }


        [Fact]
        public void Day05_Part01()
        {
            var input = File.ReadAllLines("./2015/Day05Input.txt");
            var result = input.Count(IsNice);
            result.Should().Be(238);
        }

        [Theory]
        [InlineData("qjhvhtzxzqqjkmpb", true)]
        [InlineData("xxyxx", true)]
        [InlineData("uurcxstgmygtbstg", false)]
        [InlineData("ieodomkazucvgmuy", false)]
        public void Day05_Test02(string input, bool expectedResult)
        {
            IsNiceV2(input).Should().Be(expectedResult);
        }

        [Fact]
        public void Day05_Part02()
        {
            var input = File.ReadAllLines("./2015/Day05Input.txt");
            var result = input.Count(IsNiceV2);
            result.Should().Be(69);
        }
    }
}
