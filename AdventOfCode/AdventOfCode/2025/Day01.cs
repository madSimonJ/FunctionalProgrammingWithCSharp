using AdventOfCode.Common;

namespace AdventOfCode._2025;


public static class Day01Solution
{
    public static IEnumerable<int> ParseInstructions(string instructions) =>
        instructions.Split(Environment.NewLine)
        .Select(x => (Direction: x[0], Quantity: x[1..]))
        .Select(x => int.Parse(x.Quantity) * (x.Direction == 'R' ? 1 : -1));

    public static IEnumerable<int> ParseInstructions2(string instructions) =>
        from line in instructions.Split(Environment.NewLine)
        let direction = line[0] == 'R' ? 1 : -1
        let quantity = int.Parse(line[1..])
        from delta in Enumerable.Range(0, quantity)
        select direction;

    public static int UpdateDial(int currentValue, int instruction) =>
        (currentValue + instruction + 100) % 100;

    public static IEnumerable<int> ScanDialLocations(int startingLocation, IEnumerable<int> instructions) =>
        instructions
        .Scan(startingLocation, UpdateDial);

    public static int CalculatePassword(string instructions) =>
        Day01Solution.ParseInstructions(instructions)
        .Map(x => Day01Solution.ScanDialLocations(50, x))
        .Count(x => x == 0);

    public static int CalculatePassword2(string instructions) =>
        Day01Solution.ParseInstructions2(instructions)
        .Map(x => Day01Solution.ScanDialLocations(50, x))
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


    [Theory]
    [InlineData(11, 8, 19)]
    [InlineData(19, -19, 0)]
    [InlineData(0, -1, 99)]
    [InlineData(99, 1, 0)]
    [InlineData(5, -10, 95)]
    [InlineData(95, 5, 0)]
    public void Day01_Test02(int currentDial, int move, int expectedDial)
    {
        var calculatedDial = Day01Solution.UpdateDial(currentDial, move);
        calculatedDial.Should().Be(expectedDial);
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
            Day01Solution.ParseInstructions(input)
            .Map(x => Day01Solution.ScanDialLocations(50, x))
            .ToArray();

        answer.Should().BeEquivalentTo([50, 82, 52, 0, 95, 55, 0, 99, 0, 14, 32]);
    }

    [Fact]
    public void Day01_Test04()
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
    public void Day01_Test05()
    {
        var answer =
            Day01Solution.ParseInstructions2("L68");

        answer.Should().AllSatisfy(x => x.Should().Be(-1));
        answer.Should().HaveCount(68);
    }


    [Fact]
    public void Day01_Test06()
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
            Day01Solution.CalculatePassword2(input);

        answer.Should().Be(6);
    }

    [Fact]
    public void Day01_Part02()
    {
        var input = File.ReadAllText("./2025/Day01input.txt");
        var answer =
            Day01Solution.CalculatePassword2(input);
        answer.Should().Be(6695);
    }
}
