using Microsoft.VisualBasic;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode._2025
{
    public static class Day04Solution
    {

        public static int CountAccessibleRolls(string input)
        {
            var grid = new Grid<bool>(input, Environment.NewLine, (_, _, c) => c == "@");
            var accessibleRolls =
                grid.Points
                .Where(x => x.Value)
                .Count(x => grid.GetNeighbours(x.X, x.Y).Count(x => x.Value) < 4);
            return accessibleRolls;
        }

        public static int RemoveRolls(string input)
        {
            var grid = new Grid<bool>(input, Environment.NewLine, (_, _, c) => c == "@");
            var numberOfRolls = grid.Points.Count(x => x.Value);
            var finalGrid = RemoveAccessibleRolls(grid, numberOfRolls);
            var finalNumberOfRolls = finalGrid.Points.Count(x => x.Value);
            return numberOfRolls - finalNumberOfRolls;

            Grid<bool> RemoveAccessibleRolls(Grid<bool> grid, int numberOfRolls)
            {
                var updatedGrid = grid.UpdateGrid((v, x, y) => v switch
                {
                    false => false,
                    true when grid.GetNeighbours(x, y).Count(z => z.Value) < 4 => false,
                    _ => true
                });
                var updatedNumberOfRolls = updatedGrid.Points.Count(x => x.Value);
                return numberOfRolls == updatedNumberOfRolls
                    ? updatedGrid
                    : RemoveAccessibleRolls(updatedGrid, updatedNumberOfRolls);
            }
        }
    }

    public class Day04
    {
        [Fact]
        public void Day04_Test01()
        {
            var input = @"..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.";
            var output = Day04Solution.CountAccessibleRolls(input);
            output.Should().Be(13);
            
        }

        [Fact]
        public void Day04_Part01()
        {
            var input = File.ReadAllText("./2025/Day04input.txt");
            var answer = Day04Solution.CountAccessibleRolls(input);

            answer.Should().Be(1518);
        }

        [Fact]
        public void Day04_Test02()
        {
            var input = @"..@@.@@@@.
@@@.@.@.@@
@@@@@.@.@@
@.@@@@..@.
@@.@@@@.@@
.@@@@@@@.@
.@.@.@.@@@
@.@@@.@@@@
.@@@@@@@@.
@.@.@@@.@.";
            var output = Day04Solution.RemoveRolls(input);
            output.Should().Be(43);

        }

        [Fact]
        public void Day04_Part02()
        {
            var input = File.ReadAllText("./2025/Day04input.txt");
            var answer = Day04Solution.RemoveRolls(input);

            answer.Should().Be(8665);
        }
    }
}
