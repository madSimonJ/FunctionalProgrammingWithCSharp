using AdventOfCode.Common;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{

    public readonly record struct Day05State
    {
        public IEnumerable<IEnumerable<int>> Updates { get; init; }
        public IDictionary<int, IEnumerable<int>> PageLessThan { get; init; }
    }

    public static class Day05Solution
    {
        public static Day05State ParseInput(string input)
        {
            var splitInput = input.Split("\r\n\r\n").ToArray();
            var pageRules = splitInput[0]
                                .Split("\r\n")
                                .Select(x => x.Split("|"))
                                .Select(x => (x[0], x[1]))
                                .Select(x => (int.Parse(x.Item1), int.Parse(x.Item2)));
            var groupedPageRules = pageRules.GroupBy(x => x.Item1)
                                            .Select(x => (x.Key, x.Select(y => y.Item2)));
            var pageRulesDictionary = groupedPageRules.ToDictionary(x => x.Key, x => x.Item2.Order().AsEnumerable());

            var updates = splitInput[1].Split("\r\n")
                                    .Select(x => x.Split(","))
                                    .Select(x => x.Select(y => int.Parse(y)));

            return new Day05State
            {
                Updates = updates,
                PageLessThan = pageRulesDictionary
            };
        }

        public static bool ValidateUpdate(IEnumerable<int> update, IDictionary<int, IEnumerable<int>> pageLessThanRules)
        {
            var updateArr = update.ToArray();
            var updatePositions = updateArr.Select((x, i) => (x, i)).ToDictionary(x => x.x, x => x.i);
             var pageLessThanlookup = pageLessThanRules.ToLookupWithDefault(x => [0]);

            var rules =
                from u in updateArr
                from r in pageLessThanlookup(u).Where(x => updateArr.Contains(x))
                select new { PagePos = updatePositions[u], RulePos = updatePositions[r] };

            var isValid = rules.All(x => x.RulePos > x.PagePos);

            return isValid;

        }

        public static IEnumerable<int> CorrectUpdate(IEnumerable<int> update, IDictionary<int, IEnumerable<int>> pageLessThanRules)
        {
            var updateArr = update.ToArray();
            var updatePositions = updateArr.Select((x, i) => (x, i)).ToDictionary(x => x.x, x => x.i);
            var pageLessThanlookup = pageLessThanRules.ToLookupWithDefault(x => [0]);
            var pageComparer = Comparer<int>.Create((p1, p2) => pageLessThanlookup(p1).Contains(p2) ? -1 : 1);

            var reorderedPages = updateArr.OrderBy(x => x, pageComparer).ToArray();
            return reorderedPages;

        }



        public static int GetMiddleValue(IEnumerable<int> update) =>
            update.ToArray()
                .Map(x => x[(x.Length / 2)]);

        public static int CalculateAnswer(string input, bool correctInvalidInput = false) =>
            input.Map(ParseInput)
                .Map(x => (
                    GroupedPages: x.Updates.GroupBy(y => ValidateUpdate(y, x.PageLessThan)).ToDictionary(x => x.Key, x => x.ToArray()),
                    Rules: x.PageLessThan
                ))
                .Map(x => (correctInvalidInput
                    ? x.GroupedPages[false].Select(y => CorrectUpdate(y, x.Rules))
                    : x.GroupedPages[true]))
                .Sum(GetMiddleValue);
    }

    public class Day05
    {
        private readonly string Input = @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47";

        [Fact]
        public void Day05_Part01_test01()
        {
            var state = Day05Solution.ParseInput(Input);
            state.Should().BeEquivalentTo(
                new Day05State
                {
                    PageLessThan = new Dictionary<int, IEnumerable<int>>
                    {
                        { 47, [ 13, 29, 53, 61 ] },
                        { 97, [13, 29, 47, 53, 61, 75]  },
                        { 75, [13, 29, 47, 53, 61] },
                        { 61, [13, 29, 53] },
                        { 29, [13] },
                        { 53, [13, 29] }
                    },
                    Updates = [

                        [75, 47, 61, 53, 29],
                        [97,61,53,29,13],
                        [75,29,13],
                        [75,97,47,61,53],
                        [61,13,29],
                        [97,13,75,29,47]
                    ]
                }
            );
        }

        [Theory]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        [InlineData(4, false)]
        [InlineData(5, false)]
        public void Day05_Part01_test02(int updateIndex, bool expectedResult)
        {
            var state = Day05Solution.ParseInput(Input);
            var valid = Day05Solution.ValidateUpdate(state.Updates.ToArray()[updateIndex], state.PageLessThan);
            valid.Should().Be(expectedResult);
        }

        [Fact]
        public void Day05_Part01_Test03()
        {
            var state = Day05Solution.ParseInput(Input);
            var validInputs = state.Updates.Where(x => Day05Solution.ValidateUpdate(x, state.PageLessThan)).Select(x => x.ToArray()).ToArray();
            validInputs.Should().BeEquivalentTo(
            [
                [75, 47, 61, 53, 29],
                [97,61,53,29,13],
                new[] { 75, 29, 13 }
            ]);
        }

        [Theory]
        [InlineData(new[] { 75, 47, 61, 53, 29 }, 61)]
        [InlineData(new[] { 97, 61, 53, 29, 13 }, 53)]
        [InlineData(new[] { 75, 29, 13 }, 29)]
        public void Day05_Part01_test04(IEnumerable<int> update, int expectedResult)
        {
            var state = Day05Solution.ParseInput(Input);
            var middleValue = Day05Solution.GetMiddleValue(update);
            middleValue.Should().Be(expectedResult);
        }


        [Fact]
        public void Day05_Part01_Test05()
        {
            var answer = Day05Solution.CalculateAnswer(Input);
            answer.Should().Be(143);
        }

        [Fact]
        public void Day05_Part01()
        {
            var input = File.ReadAllText("./2024/Day05input.txt");
            var answer = Day05Solution.CalculateAnswer(input);
            answer.Should().Be(5762);
        }

        [Theory]
        [InlineData(new[] { 75, 97, 47, 61, 53 }, new[] { 97, 75, 47, 61, 53 })]
        [InlineData(new[] { 61, 13, 29 }, new[] { 61, 29, 13 })]
        [InlineData(new[] { 97, 13, 75, 29, 47 }, new[] { 97, 75, 47, 29, 13 })]
        public void Day05_Part02_test01(IEnumerable<int> update, IEnumerable<int> expectedResult)
        {
            var state = Day05Solution.ParseInput(Input);
            var correctedUpdate = Day05Solution.CorrectUpdate(update, state.PageLessThan);
            correctedUpdate.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Day05_Part02_Test02()
        {
            var answer = Day05Solution.CalculateAnswer(Input, true);
            answer.Should().Be(123);
        }

        [Fact]
        public void Day05_Part02()
        {
            var input = File.ReadAllText("./2024/Day05input.txt");
            var answer = Day05Solution.CalculateAnswer(input, true);
            answer.Should().Be(4130);
        }
    }
}
