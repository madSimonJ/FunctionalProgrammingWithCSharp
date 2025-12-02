namespace AdventOfCode._2025
{
    public static class Day02Solution
    {
        public static IEnumerable<string> GenerateRange(string input) =>
            from range in input.Split(",")
            let boundaries = range.Split("-").Select(x => long.Parse(x)).ToArray()
            let length = boundaries[1] - boundaries[0] + 1
            from id in LEnumerable.LRange(boundaries[0], length)
            select id.ToString();

        public static IEnumerable<string> FindInvalidIds(IEnumerable<string> input) =>
            from id in input.Where(x => x.Length % 2 == 0)
            let a = id[..(id.Length / 2)].ToString()
            let b = id[(id.Length / 2)..].ToString()
            let invalidId = string.Equals(a, b)
            from result in invalidId ? [id] : Enumerable.Empty<string>()
            select id;

        public static IEnumerable<string> FindInvalidIds2(IEnumerable<string> input) => (from id in input
            from rangeToCheck in Enumerable.Range(1, id.Length / 2).Where(x => id.Length % x == 0)
            let patternToCheck = id[..rangeToCheck]
            let possibleInvalidId = string.Join("", Enumerable.Repeat(patternToCheck, id.Length / rangeToCheck))
            let isInvalid = string.Equals(id, possibleInvalidId)
            from result in isInvalid ? [id] : Enumerable.Empty<string>()
            select id).Distinct();

        public static long CalculateAnswer(string input) =>
            input
            .Map(GenerateRange)
            .Map(FindInvalidIds)
            .Sum(long.Parse);

        public static long CalculateAnswer2(string input) =>
            input
            .Map(GenerateRange)
            .Map(FindInvalidIds2)
            .Sum(long.Parse);

    }

    public class Day02
    {
        [Fact]
        public void Day02_Test01()
        {
            var input = "11-22";
            var output = Day02Solution.GenerateRange(input);
            output.Should().BeEquivalentTo(["11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22"]);
        }

        [Fact]
        public void Day02_Test02()
        {
            var input = "11-22,95-115";
            var output = Day02Solution.GenerateRange(input);
            output.Should().BeEquivalentTo(["11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "95", "96", "97", "98", "99", "100", "101", "102", "103", "104", "105", "106", "107", "108", "109", "110", "111", "112", "113", "114", "115"]);
        }

        [Theory]
        [InlineData("11-22", new[] { "11", "22"})]
        [InlineData("95-115", new[] { "99"})]
        [InlineData("998-1012", new[] { "1010" })]
        [InlineData("1188511880-1188511890", new[] { "1188511885" })]
        [InlineData("222220-222224", new[] { "222222" })]
        [InlineData("1698522-1698528", new string[0])]
        [InlineData("446443-446449", new[] { "446446" })]
        [InlineData("38593856-38593862", new[] { "38593859" })]
        public void Day02_Test03(string input, IEnumerable<string> expectedOutput)
        {
            var actualOutput = Day02Solution.GenerateRange(input)
                                .Map(Day02Solution.FindInvalidIds);

            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void Day02_Test04()
        {
            var input = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,\r\n1698522-1698528,446443-446449,38593856-38593862,565653-565659,\r\n824824821-824824827,2121212118-2121212124";
            var output = Day02Solution.GenerateRange(input)
                                .Map(Day02Solution.FindInvalidIds);
            output.Should().BeEquivalentTo(["11", "22", "99", "1010", "1188511885", "222222", "446446", "38593859"]);
        }

        [Fact]
        public void Day02_Test05()
        {
            var input = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,\r\n1698522-1698528,446443-446449,38593856-38593862,565653-565659,\r\n824824821-824824827,2121212118-2121212124";
            var output = Day02Solution.CalculateAnswer(input);
            output.Should().Be(1227775554);
        }

        [Fact]
        public void Day02_Part01()
        {
            var input = File.ReadAllText("./2025/Day02input.txt");
            var answer =
                Day02Solution.CalculateAnswer(input);
            answer.Should().Be(30323879646L);
        }

        [Theory]
        [InlineData("11-22", new[] { "11", "22" })]
        [InlineData("95-115", new[] { "99", "111" })]
        [InlineData("998-1012", new[] { "999", "1010" })]
        [InlineData("1188511880-1188511890", new[] { "1188511885" })]
        [InlineData("222220-222224", new[] { "222222" })]
        [InlineData("1698522-1698528", new string[0])]
        [InlineData("446443-446449", new[] { "446446" })]
        [InlineData("38593856-38593862", new[] { "38593859" })]
        [InlineData("565653-565659", new[] { "565656" })]
        [InlineData("824824821-824824827", new[] { "824824824" })]
        [InlineData("2121212118-2121212124", new[] { "2121212121" })]
        public void Day02_Test06(string input, IEnumerable<string> expectedOutput)
        {
            var actualOutput = Day02Solution.GenerateRange(input)
                                .Map(Day02Solution.FindInvalidIds2);

            actualOutput.Should().BeEquivalentTo(expectedOutput);
        }

        [Fact]
        public void Day02_Part02()
        {
            var input = File.ReadAllText("./2025/Day02input.txt");
            var answer =
                Day02Solution.CalculateAnswer2(input);
            answer.Should().Be(43872163557L);
        }
    }
}
