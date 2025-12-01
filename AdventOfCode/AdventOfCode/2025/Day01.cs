using AdventOfCode.Common;

namespace AdventOfCode._2025;

public static class Day01Solution
{
    public static IEnumerable<int> ParseInstructions(string instructions, bool includeAllZeroPasses = false) =>
               from line in instructions.Split(Environment.NewLine)
               let direction = line[0] == 'R' ? 1 : -1
               let quantity = int.Parse(line[1..])
               from delta in Enumerable.Repeat(0, includeAllZeroPasses ? quantity : 1)
               select direction * (includeAllZeroPasses ? 1 : quantity);

    public static IEnumerable<int> ScanDialLocations(int startingLocation, IEnumerable<int> instructions) =>
        instructions
        .Scan(startingLocation, (acc, x) => (acc + x + 100) % 100);

    public static int CalculatePassword(string instructions, bool includeAllZeroPasses = false) =>
        ParseInstructions(instructions, includeAllZeroPasses)
        .Map(x => ScanDialLocations(50, x))
        .Count(x => x == 0);
}

public class Day01_Tests
{
    [Fact]
    public void Day01_Test01()
    {
        var input = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82";
        var parsedInput = Day01Solution.ParseInstructions(input);
        parsedInput.Should().BeEquivalentTo([ -68, -30, 48, -5, 60, -55, -1, -99, 14, -82]);
    }


    [Fact]
    public void Day01_Test02()
    {
        var input = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82";
        var answer = 
            Day01Solution.ParseInstructions(input)
            .Map(x => Day01Solution.ScanDialLocations(50, x))
            .ToArray();

        answer.Should().BeEquivalentTo([50, 82, 52, 0, 95, 55, 0, 99, 0, 14, 32]);
    }

    [Fact]
    public void Day01_Test03()
    {
        var input = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82";
        var answer =
            Day01Solution.CalculatePassword(input);

        answer.Should().Be(3);
    }

    [Fact]
    public void Day01_Part01()
    {
        var input = File.ReadAllText("./2025/Day01input.txt");
        var answer =
            Day01Solution.CalculatePassword(input);
        answer.Should().Be(1123);
    }

    [Fact]
    public void Day01_Test04()
    {
        var answer =
            Day01Solution.ParseInstructions("L68", true);

        answer.Should().AllSatisfy(x => x.Should().Be(-1));
        answer.Should().HaveCount(68);
    }


    [Fact]
    public void Day01_Test05()
    {
        var input = @"L68
L30
R48
L5
R60
L55
L1
L99
R14
L82";
        var answer =
            Day01Solution.CalculatePassword(input, true);

        answer.Should().Be(6);
    }

    [Fact]
    public void Day01_Part02()
    {
        var input = File.ReadAllText("./2025/Day01input.txt");
        var answer =
            Day01Solution.CalculatePassword(input, true);
        answer.Should().Be(6695);
    }
}
