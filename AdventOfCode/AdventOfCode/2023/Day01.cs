using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode._2023
{
    public class Day01
    {
        private static Regex Digit = new("[0-9]");
        private static Regex FirstNumeral = new("(\\d|one|two|three|four|five|six|seven|eight|nine)");
        private static Regex LastNumeral = new("(\\d|one|two|three|four|five|six|seven|eight|nine)", RegexOptions.RightToLeft);


        private static int CalibrationValue(string input) =>
            Digit.Matches(input)
                .Map(x => (
                    First: int.Parse(x.First().ToString()) * 10,
                    Last: int.Parse(x.Last().ToString())
                ))
                .Map(x => x.First  + x.Last);

        private static string WordOrDigitToInt(string input) => input switch
        {
            "one" => "1",
            "two" => "2",
            "three" => "3",
            "four" => "4",
            "five" => "5",
            "six" => "6",
            "seven" => "7",
            "eight" => "8",
            "nine" => "9",
            _ => input
        };

        private static int CalibrationValue2(string input)
        {
            var first = FirstNumeral.Match(input);
            var last = LastNumeral.Match(input);

            var firstDigit = int.Parse(WordOrDigitToInt(first.Value));
            var lastDigit = int.Parse(WordOrDigitToInt(last.Value));

            var answer = firstDigit * 10 + lastDigit;
            return answer;
        }
            
        [Theory]
        [InlineData("1abc2", 12)]
        [InlineData("pqr3stu8vwx", 38)]
        [InlineData("a1b2c3d4e5f", 15)]
        [InlineData("treb7uchet", 77)]

        public void Day01_Test01(string input, int expectedAnswer)
        {
            CalibrationValue(input).Should().Be(expectedAnswer);
        }

        [Fact]
        public void Day01_Part01()
        {
            var input = File.ReadAllLines("./2023/Day01Input.txt");
            var answer = input.Sum(CalibrationValue);
            answer.Should().Be(55834);
        }

        [Theory]
        [InlineData("two1nine", 29)]
        [InlineData("eightwothree", 83)]
        [InlineData("abcone2threexyz", 13)]
        [InlineData("xtwone3four", 24)]
        [InlineData("4nineeightseven2", 42)]
        [InlineData("zoneight234", 14)]
        [InlineData("7pqrstsixteen", 76)]
        [InlineData("stbqnrhdqnjcvjgthtmht8xndfgprq3eightwol", 82)]

        public void Day01_Test02(string input, int expectedAnswer)
        {
            CalibrationValue2(input).Should().Be(expectedAnswer);
        }

        [Fact]
        public void Day01_Test03()
        {
            var input =  @"two1nine
eightwothree
abcone2threexyz
xtwone3four
4nineeightseven2
zoneight234
7pqrstsixteen".Split(Environment.NewLine);

            input.Sum(CalibrationValue2).Should().Be(281);
        }

        [Fact]
        public void Day01_Part02()
        {
            var input = File.ReadAllLines("./2023/Day01Input.txt");
            var answer = input.Sum(CalibrationValue2);
            answer.Should().Be(53221);
        }
    }
}
