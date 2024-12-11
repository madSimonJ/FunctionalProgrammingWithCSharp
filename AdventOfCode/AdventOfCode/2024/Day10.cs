using AdventOfCode.Common;

namespace AdventOfCode._2024
{

    public static class Day10Solution
    {
        public readonly record struct GridLocation(int Col, int Row, int Value);

        public static IEnumerable<GridLocation> ParseInput(string input) =>
            input.Split("\r\n")
                .SelectMany((splitRow, col) =>
                    splitRow.Select((value, row) => new GridLocation(col, row, int.Parse(value.ToString()))));

        public static int ManhattanDistance(GridLocation a, GridLocation b) =>
            Math.Abs(a.Row - b.Row) + Math.Abs(a.Col - b.Col);

        public static int CalculateAnswers(IEnumerable<GridLocation> grid)
        {
            var gridArr = grid.ToArray();
            var groupedGrid = gridArr.GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.ToArray());

            var trails = groupedGrid[0].Select(x =>

                Enumerable.Range(1, 9)
                    .Aggregate(new[] { x } , (acc, y) =>
                    {
                        GridLocation[] e = [
                            .. acc,
                            .. from trail in acc.Where(z => z.Value == y-1)
                            from newsquare in groupedGrid[y].Where(z =>  ManhattanDistance(trail, z) == 1).Distinct()
                            select newsquare
                        ];
                        return e.Distinct().ToArray();
                    })).ToArray();

            var answer = trails.Select(x => x.Count(y => y.Value == 9));
            return answer.Sum();
        }

        public static int CalculateAnswers2(IEnumerable<GridLocation> grid)
        {
            var gridArr = grid.ToArray();
            var groupedGrid = gridArr.GroupBy(x => x.Value).ToDictionary(x => x.Key, x => x.ToArray());

            var trails = groupedGrid[0].Select(x =>

                Enumerable.Range(1, 9)
                    .Aggregate(new[] { x }, (acc, y) =>
                    {
                        GridLocation[] e = [
                            .. acc,
                            .. from trail in acc.Where(z => z.Value == y-1)
                            from newsquare in groupedGrid[y].Where(z =>  ManhattanDistance(trail, z) == 1)
                            select newsquare
                        ];
                        return e.ToArray();
                    })).ToArray();

            var answer = trails.Select(x => x.Count(y => y.Value == 9));
            return answer.Sum();
        }
    }



    public class Day10
    {
        [Fact]
        public void Day10_Part01_Test01()
        {
            var input = @"0123
1234
8765
9876";
            var answer = Day10Solution.ParseInput(input).Map(Day10Solution.CalculateAnswers);
            answer.Should().Be(1);


        }

        [Fact]
        public void Day10_Part01_Test02()
        {
            var input = @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732";
            var answer = Day10Solution.ParseInput(input).Map(Day10Solution.CalculateAnswers);
            answer.Should().Be(36);
        }

        [Fact]
        public void Day10_Part01()
        {
            var input = File.ReadAllText("./2024/Day10input.txt");
            var answer = Day10Solution.ParseInput(input).Map(Day10Solution.CalculateAnswers);
            answer.Should().Be(593);
        }

        [Fact]
        public void Day10_Part02_Test01()
        {
            var input = @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732";
            var answer = Day10Solution.ParseInput(input).Map(Day10Solution.CalculateAnswers2);
            answer.Should().Be(81);
        }

        [Fact]
        public void Day10_Part02()
        {
            var input = File.ReadAllText("./2024/Day10input.txt");
            var answer = Day10Solution.ParseInput(input).Map(Day10Solution.CalculateAnswers2);
            answer.Should().Be(1192);
        }
    }
}
