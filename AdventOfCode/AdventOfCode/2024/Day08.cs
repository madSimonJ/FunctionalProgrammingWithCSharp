using MoreLinq;

namespace AdventOfCode._2024
{

    public class Day08Solution
    {
        private readonly IDictionary<(int, int), char> _Grid;
        private readonly IDictionary<char, IEnumerable<(int, int)>> _antennaLocations;
        private readonly IEnumerable<char> _Antennae;
        private readonly int NumberOfColumns;
        private readonly int NumberOfRows;

        public Day08Solution(string input)
        {
            var brokenDownGrid = input.Split("\r\n")
                                .SelectMany((rowVal, r) =>
                                    rowVal.Select((colVal, c) => (Col: c, Row: r, Val: colVal))
                                ).ToArray();
            this._Grid = brokenDownGrid.ToDictionary(x => (x.Col, x.Row), x => x.Val);
            this._antennaLocations = brokenDownGrid.Where(x => x.Val != '.')
                                        .GroupBy(x => x.Val)
                                        .ToDictionary(x => x.Key, x => x.Select(y => (y.Col, y.Row)));

            this.NumberOfColumns = brokenDownGrid.Max(x => x.Col);
            this.NumberOfRows = brokenDownGrid.Max(y => y.Row);
            this._Antennae = this._antennaLocations.Select(x => x.Key);
        }

        public IEnumerable<(int Col, int Row)> GetAntinodes()
        {
            var pairings = from a in _Antennae
                           from a1 in this._antennaLocations[a]
                           from a2 in this._antennaLocations[a].Where(x => x != a1)
                           select (From: a1, To: a2);

            var antinodes = pairings.Select(x => (
                Col: x.To.Item1 + (x.To.Item1 - x.From.Item1),
                Row: x.To.Item2 + (x.To.Item2 - x.From.Item2)
            ))
                .Where(x => x.Col >= 0 && x.Col <= this.NumberOfColumns)
                .Where(x => x.Row >= 0 && x.Row <= this.NumberOfRows)
                .ToArray();

            return antinodes;
        }

        public IEnumerable<(int Col, int Row)> GetAntinodesWithResonantHarmonics()
        {
            var pairings = from a in _Antennae
                           from a1 in this._antennaLocations[a]
                           from a2 in this._antennaLocations[a].Where(x => x != a1)
                           select (From: a1, To: a2);

            var deltas = pairings.Select(x => (
                x.From,
                Delta: (x.To.Item1 - x.From.Item1, x.To.Item2 - x.From.Item2)
            )).ToArray();

            var antinodes = deltas.SelectMany(x =>

                (new[] { x }).Repeat()
                                .Select((x, i) => (
                                    x.From.Item1 + (x.Delta.Item1 * i),
                                    x.From.Item2 + (x.Delta.Item2 * i)
                                ))
                                .TakeUntil(x => x.Item1 < 0 || x.Item2 < 0 || x.Item1 > this.NumberOfColumns || x.Item2 > this.NumberOfRows)

            ).Where(x => x.Item1 >= 0 && x.Item1 <= this.NumberOfColumns && x.Item2 >= 0 && x.Item2 <= this.NumberOfRows).Distinct();

            //var antinodes = pairings.Select(x => (
            //    Col: x.To.Item1 + (x.To.Item1 - x.From.Item1),
            //    Row: x.To.Item2 + (x.To.Item2 - x.From.Item2)
            //))
            //    .Where(x => x.Col >= 0 && x.Col <= this.NumberOfColumns)
            //    .Where(x => x.Row >= 0 && x.Row <= this.NumberOfRows)
            //    .ToArray();

            return antinodes;
        }
    }

    public class Day08
    {
        [Fact]
        public void Day08_Part01_Test01()
        {
            var input = @"..........
..........
..........
....a.....
..........
.....a....
..........
..........
..........
..........";

            var day8 = new Day08Solution(input);
            var answer = day8.GetAntinodes();
            answer.Should().BeEquivalentTo([
                (3, 1), (6, 7)
            ]);
        }

        [Fact]
        public void Day08_Part01_Test02()
        {
            var input = @"..........
..........
..........
....a.....
........a.
.....a....
..........
..........
..........
..........";

            var day8 = new Day08Solution(input);
            var answer = day8.GetAntinodes();
            answer.Should().BeEquivalentTo([
                (0,2), (3, 1), (6, 7), (2, 6)
            ]);
        }

        [Fact]
        public void Day08_Part01_Test03()
        {
            var input = @"..........
..........
..........
....a.....
........a.
.....a....
..........
......A...
..........
..........";

            var day8 = new Day08Solution(input);
            var answer = day8.GetAntinodes();
            answer.Should().BeEquivalentTo([
                (0,2), (3, 1), (6, 7), (2, 6)
            ]);
        }

        [Fact]
        public void Day08_Part01_Test04()
        {
            var input = @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............";

            var day8 = new Day08Solution(input);
            var answer = day8.GetAntinodes();
            answer.Distinct().Count().Should().Be(14);
        }

        [Fact]
        public void Day08_Part01()
        {
            var input = File.ReadAllText("./2024/Day08input.txt");
            var day8 = new Day08Solution(input);
            var answer = day8.GetAntinodes();
            answer.Distinct().Count().Should().Be(256);

        }

        [Fact]
        public void Day08_Part02_Test01()
        {
            var input = @"T.........
...T......
.T........
..........
..........
..........
..........
..........
..........
..........";

            var day8 = new Day08Solution(input);
            var answer = day8.GetAntinodesWithResonantHarmonics();
            answer.Distinct().Count().Should().Be(9);
        }

        [Fact]
        public void Day08_Part02()
        {
            var input = File.ReadAllText("./2024/Day08input.txt");
            var day8 = new Day08Solution(input);
            var answer = day8.GetAntinodesWithResonantHarmonics();
            answer.Distinct().Count().Should().Be(1005);

        }
    }
}
