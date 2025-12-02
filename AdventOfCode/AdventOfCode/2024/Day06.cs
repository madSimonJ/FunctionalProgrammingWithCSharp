using System.Text;

namespace AdventOfCode._2024
{

    public class Day06Solution
    {
        public readonly record struct Day06State
        {
            public Day06State()
            {
                IsOnBoard = true;
            }

            public required (int col, int row) CurrentPosition { get; init; }
            public required IEnumerable<(int, int, int)> Trail { get; init; }
            public required int Orientation { get; init; }
            public required bool IsOnBoard { get; init; } = true;
            public required bool IsInfiniteLoop { get; init; } = false;
        }

        private readonly IDictionary<(int, int), char> _grid;
        public readonly int NumberOfRows;
        public readonly int NumberOfColumns;
        public readonly (int col, int row) StartingPosition;

        public Day06Solution(string input)
        {
            var lineSplitGrid = input.Split("\r\n").ToArray();
            var cells = lineSplitGrid.SelectMany((x, rowNumber) =>
                x.Select((y, colNumber) => (rowNumber, colNumber, value: y)
            ));

            this.NumberOfColumns = cells.Max(x => x.colNumber)+1;
            this.NumberOfRows = cells.Max(x => x.rowNumber)+1;
            var g = cells.ToDictionary(x => (x.colNumber, x.rowNumber), x => x.value);
            this.StartingPosition = g.First(x => x.Value == '^').Key;
            this._grid = g.Select(x => x.Value == '^' ? new KeyValuePair<(int, int), char>(x.Key, '.') : x)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public Day06State CalculateNextPosition(Day06State state, IDictionary<(int, int), char>? replacementGrid = null)
        {
            var proposedNextLocation = (state.Orientation % 4) switch
            {
                0 => (state.CurrentPosition.col, state.CurrentPosition.row-1),
                1 => (state.CurrentPosition.col + 1, state.CurrentPosition.row),
                2 => (state.CurrentPosition.col, state.CurrentPosition.row +1),
                3 => (state.CurrentPosition.col - 1, state.CurrentPosition.row)
            };

            var proposedNextLocationOnBoard = (replacementGrid ??  this._grid).TryGetValue(proposedNextLocation, out var value);
            var proposedNextValue = proposedNextLocationOnBoard ? value : default;
            var newOrientation = (proposedNextValue == '.' || !proposedNextLocationOnBoard ? state.Orientation : state.Orientation + 1) % 4;
            var isInfiniteLoop = state.Trail.Contains((
                proposedNextLocation.Item1,
                proposedNextLocation.Item2,
                newOrientation
            ));

            var updatedState = state with
            {
                Orientation = newOrientation,
                CurrentPosition = proposedNextValue == '.' && proposedNextLocationOnBoard ? proposedNextLocation : state.CurrentPosition,
                Trail = proposedNextValue == '.' && proposedNextLocationOnBoard ? state.Trail.Append((proposedNextLocation.Item1, proposedNextLocation.Item2, newOrientation)) : state.Trail,
                IsOnBoard = proposedNextLocationOnBoard,
                IsInfiniteLoop = isInfiniteLoop
            };

            
            return updatedState;
        }

        public Day06State CalculateTrail(IDictionary<(int, int), char>? replacementGrid = null) =>
            (new Day06State
            {
                CurrentPosition = this.StartingPosition,
                IsOnBoard = true,
                Orientation = 0,
                Trail = [(this.StartingPosition.col, this.StartingPosition.row, 0)],
                IsInfiniteLoop = false
            }).IterateUntil(x => x.IsOnBoard == false || x.IsInfiniteLoop, x => CalculateNextPosition(x, replacementGrid));

        public int CountNumberOfObstructions()
        {
            var trail = CalculateTrail();
            var validLocations = trail.Trail.Where(x =>
            {
                var newGrid = this._grid.Select(y =>
                 new KeyValuePair<(int, int), char>(
                    y.Key,
                    y.Key.Item1 == x.Item1 && y.Key.Item2 == x.Item2
                        ? '#'
                        : y.Value
                )).ToDictionary(x => x.Key, x => x.Value);
                var newTrail = CalculateTrail(newGrid);
                return newTrail.IsInfiniteLoop;
            }).ToArray();

            return validLocations.Select(x => (x.Item1, x.Item2)).Where(x => x != (4,6)).Distinct().ToArray().Length;
        }
    }

    public class Day06
    {

        [Fact]
        public void Day06_Part01_Test01()
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            solution.StartingPosition.Should().Be((4, 6));
            solution.NumberOfRows.Should().Be(10);
            solution.NumberOfColumns.Should().Be(10);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(4)]
        [InlineData(8)]
        public void Day06_Part01_Test02(int orientation)
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var newPosition = solution.CalculateNextPosition(new Day06Solution.Day06State
            {
                CurrentPosition = (4, 6),
                Orientation = orientation,
                Trail = [],
                IsOnBoard = true,
                IsInfiniteLoop = false
            });

            newPosition.Should().BeEquivalentTo(
                new Day06Solution.Day06State 
                {
                    CurrentPosition = (4, 5),
                    Orientation = 0,
                    Trail = [(4, 5, 0)],
                    IsOnBoard = true,
                    IsInfiniteLoop = false
                }
            );
        }

        [Fact]
        public void Day06_Part01_Test03()
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var newPosition = solution.CalculateNextPosition(new Day06Solution.Day06State
            {
                CurrentPosition = (4, 1),
                Orientation = 0,
                Trail = [(4, 5, 0)],
                IsOnBoard = true,
                IsInfiniteLoop = false
            });

            newPosition.Should().BeEquivalentTo(
                new Day06Solution.Day06State
                {
                    CurrentPosition = (4, 1),
                    Orientation = 1,
                    Trail = [(4, 5, 0)],
                    IsOnBoard = true,
                    IsInfiniteLoop = false
                }
            );
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(9)]
        public void Day06_Part01_Test04(int orientation)
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var newPosition = solution.CalculateNextPosition(new Day06Solution.Day06State
            {
                CurrentPosition = (4, 1),
                Orientation = orientation,
                Trail = [(4, 1, orientation)],
                IsOnBoard = true,
                IsInfiniteLoop = false
            });

            newPosition.Should().BeEquivalentTo(
                new Day06Solution.Day06State
                {
                    CurrentPosition = (5, 1),
                    Orientation = 1,
                    Trail = [(4, 1, orientation), (5, 1, 1)],
                    IsOnBoard = true,
                    IsInfiniteLoop = false
                }
            );
        }

        [Fact]
        public void Day06_Part01_Test05()
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var newPosition = solution.CalculateNextPosition(new Day06Solution.Day06State
            {
                CurrentPosition = (8, 1),
                Orientation = 1,
                Trail = [(8, 1, 1)],
                IsOnBoard = true,
                IsInfiniteLoop = false
            });

            newPosition.Should().BeEquivalentTo(
                new Day06Solution.Day06State
                {
                    CurrentPosition = (8, 1),
                    Orientation = 2,
                    Trail = [(8, 1, 1)],
                    IsOnBoard = true,
                    IsInfiniteLoop = false
                }
            );
        }

        [Theory]
        [InlineData(2)]
        [InlineData(6)]
        [InlineData(10)]
        public void Day06_Part01_Test06(int orientation)
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var newPosition = solution.CalculateNextPosition(new Day06Solution.Day06State
            {
                CurrentPosition = (8, 1),
                Orientation = orientation,
                Trail = [(8, 1, orientation)],
                IsOnBoard = true,
                IsInfiniteLoop = false
            });

            newPosition.Should().BeEquivalentTo(
                new Day06Solution.Day06State
                {
                    CurrentPosition = (8, 2),
                    Orientation = 2,
                    Trail = [(8, 1, orientation), (8, 2, 2)],
                    IsOnBoard = true,
                    IsInfiniteLoop = false
                }
            );
        }

        [Fact]
        public void Day06_Part01_Test07()
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var newPosition = solution.CalculateNextPosition(new Day06Solution.Day06State
            {
                CurrentPosition = (8, 6),
                Orientation = 2,
                Trail = [(8, 6, 2)],
                IsOnBoard = true,
                IsInfiniteLoop = false
            });

            newPosition.Should().BeEquivalentTo(
                new Day06Solution.Day06State
                {
                    CurrentPosition = (8, 6),
                    Orientation = 3,
                    Trail = [(8, 6, 2)],
                    IsOnBoard = true,
                    IsInfiniteLoop = false
                }
            );
        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        [InlineData(11)]
        public void Day06_Part01_Test08(int orientation)
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var newPosition = solution.CalculateNextPosition(new Day06Solution.Day06State
            {
                CurrentPosition = (8, 6),
                Orientation = orientation,
                Trail = [(8, 6, orientation)],
                IsOnBoard = true,
                IsInfiniteLoop = false
            });

            newPosition.Should().BeEquivalentTo(
                new Day06Solution.Day06State
                {
                    CurrentPosition = (7, 6),
                    Orientation = 3,
                    Trail = [(8, 6, orientation), (7, 6, 3)],
                    IsOnBoard = true,
                    IsInfiniteLoop = false
                }
            );
        }

        [Fact]
        public void Day06_Part01_Test09()
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var newPosition = solution.CalculateNextPosition(new Day06Solution.Day06State
            {
                CurrentPosition = (7, 9),
                Orientation = 2,
                Trail = [(7, 9, 2)],
                IsOnBoard = true,
                IsInfiniteLoop = false
            });

            newPosition.Should().BeEquivalentTo(
                new Day06Solution.Day06State
                {
                    CurrentPosition = (7, 9),
                    Orientation = 2,
                    Trail = [(7, 9, 2)],
                    IsOnBoard = false,
                    IsInfiniteLoop = false
                }
            );
        }

        [Fact]
        public void Day06_Part01_Test10()
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var traversedGrid = solution.CalculateTrail();

            traversedGrid.Trail.Select(x => (x.Item1, x.Item2)).Distinct().Count().Should().Be(41);
        }

        [Fact]
        public void Day06_Part01()
        {
            var input = File.ReadAllText("./2024/Day06input.txt");
            var solution = new Day06Solution(input);
            var traversedGrid = solution.CalculateTrail();

            traversedGrid.Trail.Select(x => (x.Item1, x.Item2)).Distinct().Count().Should().Be(5080);
        }

        [Fact]
        public void Day06_Part02_Test01()
        {
            var input = @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...";

            var solution = new Day06Solution(input);
            var traversedGrid = solution.CountNumberOfObstructions();

            traversedGrid.Should().Be(6);
        }

        [Fact]
        public void Day06_Part02()
        {
            var input = File.ReadAllText("./2024/Day06input.txt");
            var solution = new Day06Solution(input);
            var obstructions = solution.CountNumberOfObstructions();

            obstructions.Should().Be(1919);
        }
    }
}
