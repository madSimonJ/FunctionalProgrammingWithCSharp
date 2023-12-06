using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode._2023
{
    public class Day03
    {
        public Day03()
        {
            
        }

        [Fact]
        public void Day03_Test01()
        {
            const string input = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

            var isNumber = new Regex("([0-9]+)");
            var lines = input.Split(Environment.NewLine).ToArray();
                
                var numbers = lines.Select(
                (x, i) => (
                    Line: i,
                    Matches: isNumber.Matches(x).Where(y => y.Length > 0).Select(y => y.Value)
                )).SelectMany(x => x.Matches.Select(y => (Line: x.Line, Number: y, xIndex: lines[x.Line].IndexOf(y, StringComparison.Ordinal))) );

                var grid = new Grid<string>(input, Environment.NewLine, (x, y, r) => r);

                var ids = numbers.Select(x =>
                {
                    var neighbours = grid.GetNeighbours(x.xIndex, x.Line, x.xIndex + x.Number.Length - 1, x.Line);
                    var isId = neighbours.Any(y => !int.TryParse(y.Value, out var i) && y.Value != ".");
                    return isId ? int.Parse(x.Number) : 0;
                }).Sum(x => x);
                ids.Should().Be(4361);
        }

        [Fact]
        public void Day03_Part01()
        {
            var input = File.ReadAllText("./2023/Day03Input.txt");


            var isNumber = new Regex("([0-9]*)");
            var lines = input.Split(Environment.NewLine).ToArray();

            var numbers = lines.Select(
                (x, i) => (
                    Line: i,
                    Matches: isNumber.Matches(x).Where(y => y.Length > 0).Select(y => y)
                )).SelectMany(x => x.Matches.Select(y =>
                {
                    return (Line: x.Line, Number: y.Value, xIndex: y.Index);
                }))
                .ToArray();

            var grid = new Grid<string>(input, Environment.NewLine, (x, y, r) => r);

            var ids = numbers.Select(x =>
            {
                var neighbours = grid.GetNeighbours(x.xIndex, x.Line, x.xIndex + x.Number.Length - 1, x.Line);
                var isId = neighbours.Any(y => !int.TryParse(y.Value, out var i) && y.Value != ".");
                return isId ? int.Parse(x.Number) : 0;
            }).Where(x => x != 0).ToArray();

            ids.Sum(x => x).Should().Be(539590);
        }

        [Fact]
        public void Day03_Test02()
        {
            const string input = @"467..114..
...*......
..35..633.
......#...
617*......
.....+.58.
..592.....
......755.
...$.*....
.664.598..";

            var isNumber = new Regex("([0-9]+)");
            var lines = input.Split(Environment.NewLine).ToArray();

            var numbers = lines.Select(
                (x, i) => (
                    Line: i,
                    Matches: isNumber.Matches(x).Where(y => y.Length > 0).Select(y => y.Value)
                )).SelectMany(x => x.Matches.Select(y => (Line: x.Line, Number: y, xIndex: lines[x.Line].IndexOf(y, StringComparison.Ordinal))))
                .Select(x => (y: x.Line, x1: x.xIndex, x2: x.xIndex + x.Number.Length - 1, Id: x.Number));

            var stars = input.Split(Environment.NewLine)
                .SelectMany((l, y) => 
                    l.Select((c, x) => (x, y, c)))
                .Where(x => x.c == '*');

            var overlappingNumbes = stars.Select(a => numbers.Where(b => a.y.IsBetween(b.y - 1, b.y + 1) && a.x.IsBetween(b.x1 - 1, b.x2 + 1)).ToArray()).Where(x => x.Count() == 2);
            var answer = overlappingNumbes.Sum(x => int.Parse(x[0].Id) * int.Parse(x[1].Id));
            answer.Should().Be(467835);


        }

        [Fact]
        public void Day03_Part02()
        {
            var input = File.ReadAllText("./2023/Day03Input.txt");

            var isNumber = new Regex("([0-9]+)");
            var lines = input.Split(Environment.NewLine).ToArray();

            var numbers = lines.Select(
                    (x, i) => (
                        Line: i,
                        Matches: isNumber.Matches(x)
                    )).SelectMany(x => x.Matches.Select(y => (x.Line, Number: y.Value, xIndex: y.Index  )))
                .Select(x => (y: x.Line, x1: x.xIndex, x2: x.xIndex + x.Number.Length - 1, Id: x.Number))
                .ToArray();

            var stars = input.Split(Environment.NewLine)
                .SelectMany((l, y) =>
                    l.Select((c, x) => (x, y, c)))
                .Where(x => x.c == '*');

            var overlappingNumbes = stars.Select(a => numbers.Where(b => b.y.IsBetween(a.y - 1, a.y + 1) &&
                                                                         (b.x1.IsBetween(a.x - 1, a.x + 1) || b.x2.IsBetween(a.x - 1, a.x + 1))).ToArray()).Where(x => x.Length == 2).ToArray();
            var answer = overlappingNumbes.Sum(x => int.Parse(x[0].Id) * int.Parse(x[1].Id));
            answer.Should().Be(80703636);


        }

    }


}
