using AdventOfCode.Common;

namespace AdventOfCode._2023
{
    public class Day11
    {
        [Theory]
        [InlineData(1, 374)]
        [InlineData(10, 1030)]
        [InlineData(100, 8410)]
        public void Test01(int galaxySize, int expectedAnswer)
        {
            var input = @"...#......
.......#..
#.........
..........
......#...
.#........
.........#
..........
.......#..
#...#.....";

            var grid = new Grid<bool>(input, Environment.NewLine, (x, y, z) => z == "#");
            var emptyRows = Enumerable.Range(0, grid.MaxX)
                .Select(x => (row: x, elements: grid.GetRow(x)))
                .Where(x =>
                {
                    return x.elements.All(y => !y.Value);
                })
                .Select(x => x.row)
                .ToArray();

            var emptyCols = Enumerable.Range(0, grid.MaxY)
                .Select(x => (col: x, elements: grid.GetColumn(x)))
                .Where(x =>
                {
                    return x.elements.All(y => !y.Value);
                })
                .Select(x => x.col)
                .ToArray();

            var galaxies = grid.Points.Where(x => x.Value).ToArray();
            var expandedGalaxy = galaxies.Select(x => x with
            {
                X = x.X + (emptyCols.Count(y => y < x.X) * (galaxySize == 1 ? 1 : galaxySize -1)),
                Y = x.Y + (emptyRows.Count(y => y < x.Y) * (galaxySize == 1 ? 1 : galaxySize - 1))
            }).ToArray();

            var pairs = from g1 in expandedGalaxy
                         from g2 in expandedGalaxy
                         select (From: g1, To: g2);

            var pairsWithoutDoubles = pairs
                .Where(x => !(x.From.X == x.To.X && x.From.Y == x.To.Y))
                .Where(x => x.To.Y > x.From.Y || (x.To.Y == x.From.Y && x.To.X > x.From.X))
                .ToArray();

            var distances = pairsWithoutDoubles.Select(x =>
                Math.Abs(x.From.X - x.To.X) + Math.Abs(x.From.Y - x.To.Y)
            ).ToArray();
            var totalSubstance = distances.Sum();
            totalSubstance.Should().Be(expectedAnswer);
        }

        [Fact]
        public void Part01()
        {
            var input = File.ReadAllText("./2023/Day11Input.txt");
            var grid = new Grid<bool>(input, Environment.NewLine, (x, y, z) => z == "#");
            var emptyRows = Enumerable.Range(0, grid.MaxX)
                .Select(x => (row: x, elements: grid.GetRow(x)))
                .Where(x =>
                {
                    return x.elements.All(y => !y.Value);
                })
                .Select(x => x.row)
                .ToArray();

            var emptyCols = Enumerable.Range(0, grid.MaxY)
                .Select(x => (col: x, elements: grid.GetColumn(x)))
                .Where(x =>
                {
                    return x.elements.All(y => !y.Value);
                })
                .Select(x => x.col)
                .ToArray();

            var galaxies = grid.Points.Where(x => x.Value).ToArray();
            var expandedGalaxy = galaxies.Select(x => x with
            {
                X = x.X + emptyCols.Count(y => y < x.X),
                Y = x.Y + emptyRows.Count(y => y < x.Y)
            }).ToArray();

            var pairs = from g1 in expandedGalaxy
                        from g2 in expandedGalaxy
                        select (From: g1, To: g2);

            var pairsWithoutDoubles = pairs
                .Where(x => !(x.From.X == x.To.X && x.From.Y == x.To.Y))
                .Where(x => x.To.Y > x.From.Y || (x.To.Y == x.From.Y && x.To.X > x.From.X))
                .ToArray();

            var distances = pairsWithoutDoubles.Select(x =>
                Math.Abs(x.From.X - x.To.X) + Math.Abs(x.From.Y - x.To.Y)
            ).ToArray();
            var totalSubstance = distances.Sum();
            totalSubstance.Should().Be(9445168);
        }

        [Fact]
        public void Part02()
        {
            var input = File.ReadAllText("./2023/Day11Input.txt");
            var grid = new Grid<bool>(input, Environment.NewLine, (x, y, z) => z == "#");
            var emptyRows = Enumerable.Range(0, grid.MaxX)
                .Select(x => (row: x, elements: grid.GetRow(x)))
                .Where(x =>
                {
                    return x.elements.All(y => !y.Value);
                })
                .Select(x => (long)x.row)
                .ToArray();

            var emptyCols = Enumerable.Range(0, grid.MaxY)
                .Select(x => (col: x, elements: grid.GetColumn(x)))
                .Where(x =>
                {
                    return x.elements.All(y => !y.Value);
                })
                .Select(x => (long)x.col)
                .ToArray();

            var galaxies = grid.Points.Where(x => x.Value)
                .Select(x => (X: (long)x.X, Y: (long)x.Y))
                .ToArray();

            var expandedGalaxy = galaxies.Select(x => x with
            {
                X = x.X + ((long)emptyCols.Count(y => y < x.X) * (1000000u - 1)),
                Y = x.Y + ((long)emptyRows.Count(y => y < x.Y) * (1000000u - 1))
            }).ToArray();

            var pairs = from g1 in expandedGalaxy
                        from g2 in expandedGalaxy
                        select (From: g1, To: g2);

            var pairsWithoutDoubles = pairs
                .Where(x => !(x.From.X == x.To.X && x.From.Y == x.To.Y))
                .Where(x => x.To.Y > x.From.Y || (x.To.Y == x.From.Y && x.To.X > x.From.X))
                .ToArray();

            var distances = pairsWithoutDoubles.Select(x =>
                Math.Abs(x.From.X - x.To.X) + Math.Abs(x.From.Y - x.To.Y)
            ).ToArray();
            var totalSubstance = distances.Sum();
            totalSubstance.Should().Be(742305960572L);
        }

    }


}
