using AdventOfCode.Common;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode._2024
{
    public static class Day09Solution
    {
        public static IEnumerable<int> MakeSubstitution(IEnumerable<int> input)
        {
            var inputArr = input.ToArray();
            var firstFreePosition = inputArr.findIndex(x => x == -1);
            var lastChar = inputArr.Length -  inputArr.Reverse().findIndex(x => x != -1) - 1;

            var returnValue = inputArr.Select((x, i) => i switch
            {
                _ when i == firstFreePosition => inputArr[lastChar],
                _ when i == lastChar => inputArr[firstFreePosition],
                _ => inputArr[i]
            });
            return returnValue.ToArray();
        }

        public static IEnumerable<int> MakeFileSubstitution(IEnumerable<int> input, int fileId)
        {
            var inputArr = input.Select((x, i) => (Value: x, Index: i)).ToArray();
            var fileToReplace = inputArr.Where(x => x.Value == fileId).ToArray();
            var replacementLocation =
                inputArr.Where(x => x.Index < fileToReplace.Last().Index)
                        .Window(fileToReplace.Length)
                        .FirstOrDefault(x => x.All(y => y.Value == -1))
                        ?.ToArray();

            var replacementLocationStart = replacementLocation == default ? -1 : replacementLocation.First().Index;
            var replacementLocationEnd = replacementLocation == default ? -1 : replacementLocation.Last().Index;

            var updatedArray = inputArr.Select((x, i) =>

                i.IsBetween(replacementLocationStart, replacementLocationEnd)
                    ? fileId
                    : x.Value == fileId && replacementLocation != default
                        ? -1
                        : x.Value

            ).ToArray();

            return updatedArray;
        }

        public static IEnumerable<int> ParseInput(string input) =>
            input.Aggregate(
                (IsFile: true, FileId: 0, DiskMap: Enumerable.Empty<int>()),
                (acc, x) => 
                {
                    return acc.IsFile switch
                    {
                        true => (false, acc.FileId + 1, acc.DiskMap.Concat(Enumerable.Repeat(acc.FileId, int.Parse(x.ToString())))),
                        false => (true, acc.FileId, acc.DiskMap.Concat(Enumerable.Repeat(-1, int.Parse(x.ToString()))))
                    };
                }).DiskMap.ToArray();

        public static IEnumerable<int> MakeFreeSpace(string input) =>
            ParseInput(input)
                .IterateWhile(x => x.Zip(x.Skip(1)).Any(y => y.First == -1 && y.Second != -1), MakeSubstitution);

        public static IEnumerable<int> MakeFileFreeSpace(string input)
        {
            var parsedInput = ParseInput(input).ToArray();
            var maxFileId = parsedInput.Max();
            var updatedArray = Enumerable.Range(0, maxFileId + 1)
                        .Reverse()
                        .Aggregate(parsedInput, (acc, x) => MakeFileSubstitution(acc, x).ToArray());
            return updatedArray;
        }

        public static long CalculateChecksum(string input, bool moveFiles = false)
        {
            var files = (moveFiles ? MakeFileFreeSpace(input) : MakeFreeSpace(input))
                .Select((x, i) => (Index: i, Value: x))
                .ToArray();

                var checksum = files.Aggregate(0l, (agg, x) =>
                {
                    return agg + ((long)x.Index * (long)(x.Value == -1 ? 0 : x.Value));
                });
            return checksum;
        }
    }

    public class Day09
    {
        [Fact]
        public void Day09_Part01_Test01()
        {
            var input = "12345";
            var output = Day09Solution.ParseInput(input);
            output.Should().BeEquivalentTo([
                0, -1, -1, 1, 1, 1, -1, -1, -1, -1, 2, 2, 2, 2, 2
            ]);
        }

        [Fact]
        public void Day09_Part01_Test02()
        {
            var input = "2333133121414131402";
            var output = Day09Solution.ParseInput(input);
            output.Should().BeEquivalentTo([
                0, 0, -1, -1, -1, 1, 1, 1, -1, -1, -1, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7,7, -1, 8, 8, 8, 8, 9, 9
            ]);
        }

        [Theory]
        [InlineData(1, new int[] { 0, 2, -1, 1, 1, 1, -1, -1, -1, -1, 2, 2, 2, 2, -1 })]
        [InlineData(2, new int[] { 0, 2, 2, 1, 1, 1, -1, -1, -1, -1, 2, 2, 2, -1, -1 })]
        [InlineData(3, new int[] { 0, 2, 2, 1, 1, 1, 2, -1, -1, -1, 2, 2, -1, -1, -1 })]
        [InlineData(4, new int[] { 0, 2, 2, 1, 1, 1, 2, 2, -1, -1, 2, -1, -1, -1, -1 })]
        [InlineData(5, new int[] { 0, 2, 2, 1, 1, 1, 2, 2, 2, -1, -1, -1, -1, -1, -1 })]
        public void Day09_Part01_Test03(int numberOfIterations, IEnumerable<int> expectedResult)
        {
            var input = "12345";
            var output = Day09Solution.ParseInput(input);
            var answer = Enumerable.Repeat(0, numberOfIterations)
                .Aggregate(output, (acc, x) => Day09Solution.MakeSubstitution(acc));
            output.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [InlineData(1, new int[] { 0, 0, 9, -1, -1, 1, 1, 1, -1, -1, -1, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, 7, -1, 8, 8, 8, 8, 9, -1 })]
        [InlineData(2, new int[] { 0, 0, 9, 9, -1, 1, 1, 1, -1, -1, -1, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, 7, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(3, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, -1, -1, -1, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, 7, -1, 8, 8, 8, -1, -1, -1 })]
        [InlineData(4, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, -1, -1, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, 7, -1, 8, 8, -1, -1, -1, -1 })]
        [InlineData(5, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, 8, -1, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, 7, -1, 8, -1, -1, -1, -1, -1 })]
        [InlineData(6, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, 8, 8, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, 7, -1, -1, -1, -1, -1, -1, -1 })]
        [InlineData(7, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, 8, 8, 2, 7, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, -1, -1, -1, -1, -1, -1, -1, -1 })]
        [InlineData(8, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, 8, 8, 2, 7, 7, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1 })]
        [InlineData(9, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, 8, 8, 2, 7, 7, 7, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 })]
        [InlineData(10, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, 8, 8, 2, 7, 7, 7, 3, 3, 3, 6, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 })]
        [InlineData(11, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, 8, 8, 2, 7, 7, 7, 3, 3, 3, 6, 4, 4, 6, 5, 5, 5, 5, -1, 6, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 })]
        [InlineData(12, new int[] { 0, 0, 9, 9, 8, 1, 1, 1, 8, 8, 8, 2, 7, 7, 7, 3, 3, 3, 6, 4, 4, 6, 5, 5, 5, 5, 6, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 })]
        public void Day09_Part01_Test04(int numberOfIterations, IEnumerable<int> expectedResult)
        {
            var input = "2333133121414131402";
            var output = Day09Solution.ParseInput(input);

        }

        [Fact]
        public void Day09_Part01_Test05()
        {
            var input = "12345";
            var output = Day09Solution.MakeFreeSpace(input);
            output.Should().BeEquivalentTo([
                0, 2, 2, 1, 1, 1, 2, 2, 2, -1, -1, -1, -1, -1, -1
            ]);
        }

        [Fact]
        public void Day09_Part01_Test06()
        {
            var input = "2333133121414131402";
            var output = Day09Solution.MakeFreeSpace(input);
            output.Should().BeEquivalentTo([
                0, 0, 9, 9, 8, 1, 1, 1, 8, 8, 8, 2, 7, 7, 7, 3, 3, 3, 6, 4, 4, 6, 5, 5, 5, 5, 6, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
            ]);
        }

        [Fact]
        public void Day09_Part01_Test07()
        {
            var input = "2333133121414131402";
            var output = Day09Solution.CalculateChecksum(input);
            output.Should().Be(1928);
        }

        [Fact]
        public void Day09_Part01()
        {
            var input = File.ReadAllText("./2024/Day09input.txt");
            var output = Day09Solution.CalculateChecksum(input);
            output.Should().Be(6367087064415L);
        }

        [Theory]
        [InlineData(1, new[] { 0, 0, 9, 9, -1, 1, 1, 1, -1, -1, -1, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, 7, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(2, new[] { 0, 0, 9, 9, -1, 1, 1, 1, -1, -1, -1, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, 7, 7, 7, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(3, new[] { 0, 0, 9, 9, -1, 1, 1, 1, 7, 7, 7, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(4, new[] { 0, 0, 9, 9, -1, 1, 1, 1, 7, 7, 7, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(5, new[] { 0, 0, 9, 9, -1, 1, 1, 1, 7, 7, 7, 2, -1, -1, -1, 3, 3, 3, -1, 4, 4, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(6, new[] { 0, 0, 9, 9, -1, 1, 1, 1, 7, 7, 7, 2, 4, 4, -1, 3, 3, 3, -1, -1, -1, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(7, new[] { 0, 0, 9, 9, -1, 1, 1, 1, 7, 7, 7, 2, 4, 4, -1, 3, 3, 3, -1, -1, -1, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(8, new[] { 0, 0, 9, 9, 2, 1, 1, 1, 7, 7, 7, -1, 4, 4, -1, 3, 3, 3, -1, -1, -1, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(9, new[] { 0, 0, 9, 9, -1, 1, 1, 1, 7, 7, 7, 2, 4, 4, -1, 3, 3, 3, -1, -1, -1, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1 })]
        [InlineData(10, new[] { 0, 0, 9, 9, -1, 1, 1, 1, 7, 7, 7, 2, 4, 4, -1, 3, 3, 3, -1, -1, -1, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1 })]
        public void Day09_Part02_Test01(int numberOfIterations, IEnumerable<int> expectedResult)
        {
            var input = "2333133121414131402";
            var parsedInput = Day09Solution.ParseInput(input);

            var answer = Enumerable.Range(0, numberOfIterations).Select(x => 9-x)
                .Aggregate(parsedInput, (acc, x) => Day09Solution.MakeFileSubstitution(acc, x));
            answer.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void Day09_Part02_Test02()
        {
            var input = "2333133121414131402";
            var answer = Day09Solution.MakeFileFreeSpace(input);
            answer.Should().BeEquivalentTo([0, 0, 9, 9, -1, 1, 1, 1, 7, 7, 7, 2, 4, 4, -1, 3, 3, 3, -1, -1, -1, -1, 5, 5, 5, 5, -1, 6, 6, 6, 6, -1, -1, -1, -1, -1, 8, 8, 8, 8, -1, -1]);
        }

        [Fact]
        public void Day09_Part03_Test03()
        {
            var input = "2333133121414131402";
            var output = Day09Solution.CalculateChecksum(input, true);
            output.Should().Be(2858);
        }

        [Fact]
        public void Day09_Part02()
        {
            var input = File.ReadAllText("./2024/Day09input.txt");
            var output = Day09Solution.CalculateChecksum(input, true);
            output.Should().Be(6390781891880L);
        }

    }
}
