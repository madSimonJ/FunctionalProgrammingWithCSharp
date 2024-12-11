using AdventOfCode._2023;
using AdventOfCode.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    public static class Day11Solution
    {
        public static string Blink(string input)
        {
            var TransformStone = (string x) => x switch
            {
                "0" => "1",
                    _ when x.Length % 2 == 0 => long.Parse(x[0..(x.Length / 2)]) + " " + long.Parse(x[(x.Length / 2)..]) ,
                    _ => (long.Parse(x) * 2024).ToString()
            };
            var memoisedTransform = TransformStone.Memoise();

            var splitInput = input.Split(" ");
            var updatedInput = splitInput.Select(memoisedTransform);

            var returnValue = string.Join(" ", updatedInput);
            return returnValue;
        }

        public static long BlinkBig(IEnumerable<long> input, int numberOfIterations)
        {
            var dic = new DefaultDictionary<(long, long), long>();

            long calculateCount (long x, long nOfIt, long result) => (Number: x, Iteration: nOfIt) switch
            {
                { Iteration: 0 } => 1,
                _ => dic.TryGetKey((x, nOfIt), (k) =>
                {
                    return x == 0
                                ? calculateCount(1, nOfIt - 1, result)
                                : x.ToString()
                                    .Map(s =>
                                        s.Length % 2 == 0
                                            ? result + calculateCount(long.Parse(s[..(s.Length / 2)]), k.Item2 - 1, result)
                                                 + calculateCount(long.Parse(s[(s.Length / 2)..]), k.Item2 - 1, result)
                                            : result + calculateCount(x * 2024, k.Item2 - 1, result));
                })
            };

            var answer = input.Sum(x => calculateCount(x, numberOfIterations, 0));
            return answer;
        }
    }

    public class Day11
    {
        [Fact]
        public void Day11_Part1_Test01()
        {
            var input = "0 1 10 99 999";
            var answer = Day11Solution.Blink(input);
            answer.Should().Be("1 2024 1 0 9 9 2021976");
        }

        [Theory]
        [InlineData(1, "253000 1 7")]
        [InlineData(2, "253 0 2024 14168")]
        [InlineData(3, "512072 1 20 24 28676032")]
        [InlineData(4, "512 72 2024 2 0 2 4 2867 6032")]
        [InlineData(5, "1036288 7 2 20 24 4048 1 4048 8096 28 67 60 32")]
        [InlineData(6, "2097446912 14168 4048 2 0 2 4 40 48 2024 40 48 80 96 2 8 6 7 6 0 3 2")]
        public void Day11_Part1_Test02(int numberOfIterations, string expectedOutput)
        {
            var input = "125 17";
            var answer = Enumerable.Range(1, numberOfIterations)
                .Aggregate(input, (acc, x) => Day11Solution.Blink(acc));
                
            answer.Should().Be(expectedOutput);
        }

        [Fact]
        public void Day11_Part1_Test03()
        {
            var input = "125 17";
            var output = Enumerable.Range(1, 25)
                .Aggregate(input, (acc, x) => Day11Solution.Blink(acc));
            var answer = output.Split(" ").Length;

            answer.Should().Be(55312);
        }

        [Fact]
        public void Day11_Part1()
        {
            var input = "6563348 67 395 0 6 4425 89567 739318";
            var output = Enumerable.Range(1, 25)
                .Aggregate(input, (acc, x) => Day11Solution.Blink(acc));
            var answer = output.Split(" ").Length;

            answer.Should().Be(184927);
        }

        [Fact]
        public void Day11_Part1a()
        {
            var input = "6563348 67 395 0 6 4425 89567 739318";
            Run(input, false, 25);
          //  output.Should().Be(184927);
        }

        [Fact]
        public void Day11_Part2()
        {
            var input = "6563348 67 395 0 6 4425 89567 739318";
            var answer = Day11Solution.BlinkBig(input.Split(" ").Select(long.Parse), 75);

            answer.Should().Be(220357186726677L);
        }

        [Fact]
        public void Day11_Part2_Test01()
        {
            var input = "0 1 10 99 999";
            var answer = Day11Solution.BlinkBig(input.Split(" ").Select(long.Parse), 1);
            answer.Should().Be(7);
        }

        [Fact]
        public void Day11_Part2_Test02()
        {
            var input = "125 17";
            var answer = Day11Solution.BlinkBig(input.Split(" ").Select(long.Parse), 25);

            answer.Should().Be(55312);
        }

        [Fact]
        public void Day11_Part2_Test03()
        {
            var input = "6563348 67 395 0 6 4425 89567 739318";
            var answer = Day11Solution.BlinkBig(input.Split(" ").Select(long.Parse), 25);

            answer.Should().Be(184927);
        }

        private readonly string input = "6563348 67 395 0 6 4425 89567 739318";
        private readonly Dictionary<long, long> cache = [];


        long Run(string inputfile, bool isTest, int numberoftimes, long supposedanswer1 = 0)
        {
            var S = inputfile.Split(' ').Select(a => long.Parse(a)).ToList();
            long answer1 = 0;

            var cache = new Dictionary<(long number, int numberoftimes), long>();
            long Getstones(long number, int times)
            {
                if (times == 0) return 1;
                else
                {
                    if (cache.TryGetValue((number, times), out long result)) return result;
                    if (number == 0)
                    {
                        result = Getstones(1, times - 1);
                    }
                    else
                    {
                        var scur = number.ToString();
                        if (scur.Length % 2 == 0)
                        {
                            result += Getstones(long.Parse(scur.Substring(0, scur.Length / 2)), times - 1);
                            result += Getstones(long.Parse(scur.Substring(scur.Length / 2)), times - 1);
                        }
                        else
                        {
                            result = Getstones(number * 2024, times - 1);
                        }
                    }
                    cache[(number, times)] = result;
                    return result;
                }
            }

            foreach (var number in S)
            {
                answer1 += Getstones(number, numberoftimes);
            }

            return answer1;
        }


    }
}
