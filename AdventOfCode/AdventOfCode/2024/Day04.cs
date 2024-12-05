namespace AdventOfCode._2024
{
    public static class Day04Solution
    {
        private static IEnumerable<IEnumerable<(int, int, char)>> XmasSearchPatterns = [[
            (1, 0, 'M'),
            (2, 0, 'A'),
            (3, 0, 'S'),
        ],
        [
            (-1, 0, 'M'),
            (-2, 0, 'A'),
            (-3, 0, 'S'),
        ],
        [
            (0, 1, 'M'),
            (0, 2, 'A'),
            (0, 3, 'S'),
        ],
        [
            (0, -1, 'M'),
            (0, -2, 'A'),
            (0, -3, 'S'),
        ],
        [
            (1, 1, 'M'),
            (2, 2, 'A'),
            (3, 3, 'S'),
        ],
        [
            (-1, -1, 'M'),
            (-2, -2, 'A'),
            (-3, -3, 'S'),
        ],
        [
            (-1, 1, 'M'),
            (-2, 2, 'A'),
            (-3, 3, 'S'),
        ],
        [
            (1, -1, 'M'),
            (2, -2, 'A'),
            (3, -3, 'S'),
        ]];

        private static IEnumerable<IEnumerable<(int, int, char)>> XDashMaxSearchPatterns = [[
            (-1,-1,'M'),
            (-1,1,'M'),
            (1,1,'S'),
            (1,-1,'S')
        ],[
            (-1,1,'M'),
            (1,1,'M'),
            (1,-1,'S'),
            (-1,-1,'S')
        ],[
            (1,1,'M'),
            (1,-1,'M'),
            (-1,1,'S'),
            (-1,-1,'S')
        ],[
            (-1,-1,'M'),
            (1,-1,'M'),
            (1,1,'S'),
            (-1,1,'S')
        ]];

        public static IDictionary<(int, int), char> ParseGrid(string input) =>
            input.Split("\r\n")
                .SelectMany((s, y) => s.Select((v, x) => (X: x, Y: y, V: v)))
                .ToDictionary(x => (x.X, x.Y), x => x.V);
        public static int CountXmases(IDictionary<(int,int),char> grid, IEnumerable<(int,int)> startingPoints, int maxX, int maxY, bool isSearchXDashMas = false)
        {
            var searches = from x in startingPoints
                           from sps in (isSearchXDashMas ? XDashMaxSearchPatterns : XmasSearchPatterns)
                           select (x, sps);

            var searchLocations = searches.Select(x =>
                x.sps.Select(y => (
                    X: x.x.Item1 + y.Item1,
                    Y: x.x.Item2 + y.Item2,
                    ExpectedChar: y.Item3,
                    Original: x.x
                ))).Where(x => x.All(y => y.X >= 0 && y.Y >= 0 && y.X <= maxX && y.Y <= maxY)).ToArray();

            var xmasFound = searchLocations.Count(x => x.All(y => grid[(y.X, y.Y)] == y.ExpectedChar));

            return xmasFound;
        }


    }


    public class Day04
    {
        [Fact]
        public void Day04_Part01_Test01()
        {
            const string input = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

            var grid = Day04Solution.ParseGrid(input)
                .Where(x => x.Value == 'X')
                .ToArray()
                .Length
                .Should().Be(19);
        }

        [Fact]
        public void Day04_Part01_Test02()
        {
            const string input = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

            var grid = Day04Solution.ParseGrid(input);
            var xes = grid.Where(x => x.Value == 'X').Select(x => (x.Key));
            var xmases = Day04Solution.CountXmases(grid, xes, 9, 9);
            xmases.Should().Be(18);
        }

        [Fact]
        public void Day04_Part01()
        {
            var input = File.ReadAllText("./2024/Day04input.txt");

            var grid = Day04Solution.ParseGrid(input);
            var xes = grid.Where(x => x.Value == 'X').Select(x => (x.Key));
            var xmases = Day04Solution.CountXmases(grid, xes, 139, 139);
            xmases.Should().Be(2344);
        }

        [Fact]
        public void Day04_Part02_Test01()
        {
            const string input = @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX";

            var grid = Day04Solution.ParseGrid(input);
            var xes = grid.Where(x => x.Value == 'A').Select(x => (x.Key));
            var xmases = Day04Solution.CountXmases(grid, xes, 9, 9, true);
            xmases.Should().Be(9);
        }

        [Fact]
        public void Day04_Part02()
        {
            var input = File.ReadAllText("./2024/Day04input.txt");

            var grid = Day04Solution.ParseGrid(input);
            var xes = grid.Where(x => x.Value == 'A').Select(x => (x.Key));
            var xmases = Day04Solution.CountXmases(grid, xes, 139, 139, true);
            xmases.Should().Be(1815);
        }
    }
}
