using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Common;

namespace AdventOfCode._2023
{
    public class Day10
    {

        private static char DetermineTypeOfS(Grid<char> grid)
        {
            var sLoc = grid.GetValueLocation('S');

            var north = grid[sLoc.X, sLoc.Y - 1];
            var northCon = north is { Value: '|' or '7' or 'F' };

            var east = grid[sLoc.X + 1, sLoc.Y];
            var eastCon = east is { Value: '-' or 'J' or '7' };

            var south = grid[sLoc.X, sLoc.Y + 1];
            var southCon =  south is { Value: '|' or 'L' or 'J' };

            var west = grid[sLoc.X - 1, sLoc.Y];
            var westCon = west is { Value: '-' or 'L' or 'F' };

            var sType = (northCon, eastCon, southCon, westCon) switch
            {
                (true, false, true, false) => '|',
                (false, true, false, true) => '-',
                (true, true, false, false) => 'L',
                (true, false, false, true) => 'J',
                (false, false, true, true) => '7',
                (false, true, true, false) => 'F'
            };

            return sType;
        }

        public char FirstMove(char sType) => sType switch
        {
            '|' or 'L' or 'J' => 'N',
            '7' or 'F' => 'S',
            _ => 'E'
        };

        [Fact]
        public void Day10_Test01()
        {
            const string input = @".....
.S-7.
.|.|.
.L-J.
.....";

            var grid = new Grid<char>(input, Environment.NewLine, (a, b, c) => c[0]);
            var sType = DetermineTypeOfS(grid);
            sType.Should().Be('F');
        }

        [Fact]
        public void Day10_Test02()
        {
            const string input = @"-L|F7
7S-7|
L|7||
-L-J|
L|-JF";

            var grid = new Grid<char>(input, Environment.NewLine, (a, b, c) => c[0]);
            var sType = DetermineTypeOfS(grid);
            sType.Should().Be('F');
        }

        [Fact]
        public void Day10_Test03()
        {
            const string input = @".....
.S-7.
.|.|.
.L-J.
.....";

            var grid = new Grid<char>(input, Environment.NewLine, (a, b, c) => c[0]);
            var sType = DetermineTypeOfS(grid);
            var sLoc = grid.GetValueLocation('S');
            var firstMove = FirstMove(sType);

            var startLoc = firstMove switch
            {
                'N' => new Coord { X = sLoc.X, Y = sLoc.Y - 1 },
                'E' => new Coord { X = sLoc.X + 1, Y = sLoc.Y },
                'W' => new Coord { X = sLoc.X - 1, Y = sLoc.Y },
                'S' => new Coord { X = sLoc.X, Y = sLoc.Y + 1 }
            };

            var pipeTraverse = (Loc: grid[startLoc.X, startLoc.Y], LastMove: firstMove, length: 0).IterateUntil(x => x.Loc.Value == 'S',
                x => x switch
                {
                    { Loc.Value: '|', LastMove: 'N' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: '|', LastMove: 'S' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),

                    { Loc.Value: '-', LastMove: 'E' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1),
                    { Loc.Value: '-', LastMove: 'W' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: 'L', LastMove: 'W' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: 'L', LastMove: 'S' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1),

                    { Loc.Value: 'J', LastMove: 'E' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: 'J', LastMove: 'S' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: '7', LastMove: 'E' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),
                    { Loc.Value: '7', LastMove: 'N' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: 'F', LastMove: 'W' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),
                    { Loc.Value: 'F', LastMove: 'N' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1)
                }
            );

            var answer = (pipeTraverse.length + 1) / 2;
            answer.Should().Be(4);
        }

        [Fact]
        public void Day10_Test04()
        {
            const string input = @"..F7.
.FJ|.
SJ.L7
|F--J
LJ...";

            var grid = new Grid<char>(input, Environment.NewLine, (a, b, c) => c[0]);
            var sType = DetermineTypeOfS(grid);
            var sLoc = grid.GetValueLocation('S');
            var firstMove = FirstMove(sType);

            var startLoc = firstMove switch
            {
                'N' => new Coord { X = sLoc.X, Y = sLoc.Y - 1 },
                'E' => new Coord { X = sLoc.X + 1, Y = sLoc.Y },
                'W' => new Coord { X = sLoc.X - 1, Y = sLoc.Y },
                'S' => new Coord { X = sLoc.X, Y = sLoc.Y + 1 }
            };

            var pipeTraverse = (Loc: grid[startLoc.X, startLoc.Y], LastMove: firstMove, length: 0).IterateUntil(x => x.Loc.Value == 'S',
                x => x switch
                {
                    { Loc.Value: '|', LastMove: 'N' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: '|', LastMove: 'S' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),

                    { Loc.Value: '-', LastMove: 'E' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1),
                    { Loc.Value: '-', LastMove: 'W' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: 'L', LastMove: 'W' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: 'L', LastMove: 'S' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1),

                    { Loc.Value: 'J', LastMove: 'E' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: 'J', LastMove: 'S' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: '7', LastMove: 'E' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),
                    { Loc.Value: '7', LastMove: 'N' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: 'F', LastMove: 'W' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),
                    { Loc.Value: 'F', LastMove: 'N' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1)
                }
            );

            var answer = (pipeTraverse.length + 1) / 2;
            answer.Should().Be(8);
        }

        [Fact]
        public void Day10_Part01()
        {
            var input = File.ReadAllText("./2023/Day10Input.txt");
            var grid = new Grid<char>(input, Environment.NewLine, (a, b, c) => c[0]);
            var sType = DetermineTypeOfS(grid);
            var sLoc = grid.GetValueLocation('S');
            var firstMove = FirstMove(sType);

            var startLoc = firstMove switch
            {
                'N' => new Coord { X = sLoc.X, Y = sLoc.Y - 1 },
                'E' => new Coord { X = sLoc.X + 1, Y = sLoc.Y },
                'W' => new Coord { X = sLoc.X - 1, Y = sLoc.Y },
                'S' => new Coord { X = sLoc.X, Y = sLoc.Y + 1 }
            };

            var pipeTraverse = (Loc: grid[startLoc.X, startLoc.Y], LastMove: firstMove, length: 0).IterateUntil(x => x.Loc.Value == 'S',
                x => x switch
                {
                    { Loc.Value: '|', LastMove: 'N' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: '|', LastMove: 'S' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),

                    { Loc.Value: '-', LastMove: 'E' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1),
                    { Loc.Value: '-', LastMove: 'W' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: 'L', LastMove: 'W' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: 'L', LastMove: 'S' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1),

                    { Loc.Value: 'J', LastMove: 'E' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.length + 1),
                    { Loc.Value: 'J', LastMove: 'S' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: '7', LastMove: 'E' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),
                    { Loc.Value: '7', LastMove: 'N' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.length + 1),

                    { Loc.Value: 'F', LastMove: 'W' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.length + 1),
                    { Loc.Value: 'F', LastMove: 'N' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.length + 1)
                }
            );

            var answer = (pipeTraverse.length + 1) / 2;
            answer.Should().Be(6599);
        }


        [Fact]
        public void Day10_Test05()
        {
            const string input = @"...........
.S-------7.
.|F-----7|.
.||.....||.
.||.....||.
.|L-7.F-J|.
.|..|.|..|.
.L--J.L--J.
...........";

            var grid = new Grid<char>(input, Environment.NewLine, (a, b, c) => c[0]);
            var sType = DetermineTypeOfS(grid);
            var sLoc = grid.GetValueLocation('S');
            var firstMove = FirstMove(sType);

            var startLoc = firstMove switch
            {
                'N' => new Coord { X = sLoc.X, Y = sLoc.Y - 1 },
                'E' => new Coord { X = sLoc.X + 1, Y = sLoc.Y },
                'W' => new Coord { X = sLoc.X - 1, Y = sLoc.Y },
                'S' => new Coord { X = sLoc.X, Y = sLoc.Y + 1 }
            };


            var pipeTraverse = (Loc: grid[startLoc.X, startLoc.Y], LastMove: firstMove, Trail: Enumerable.Empty<Coord>()).IterateUntil(x => x.Loc.Value == 'S',
                x => (x switch
                {
                    { Loc.Value: '|', LastMove: 'N' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.Trail),
                    { Loc.Value: '|', LastMove: 'S' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.Trail),
                    { Loc.Value: '-', LastMove: 'E' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.Trail),
                    { Loc.Value: '-', LastMove: 'W' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.Trail),
                    { Loc.Value: 'L', LastMove: 'W' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.Trail),
                    { Loc.Value: 'L', LastMove: 'S' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.Trail),
                    { Loc.Value: 'J', LastMove: 'E' } => (grid[x.Loc.X, x.Loc.Y - 1], 'N', x.Trail),
                    { Loc.Value: 'J', LastMove: 'S' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.Trail),
                    { Loc.Value: '7', LastMove: 'E' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.Trail),
                    { Loc.Value: '7', LastMove: 'N' } => (grid[x.Loc.X - 1, x.Loc.Y], 'W', x.Trail),
                    { Loc.Value: 'F', LastMove: 'W' } => (grid[x.Loc.X, x.Loc.Y + 1], 'S', x.Trail),
                    { Loc.Value: 'F', LastMove: 'N' } => (grid[x.Loc.X + 1, x.Loc.Y], 'E', x.Trail)
                }).Map(y => (x.Loc, x.LastMove, x.Trail.Append(new Coord { X = x.Loc.X, Y = x.Loc.Y })) )
            );

            var pipeLocations = pipeTraverse.Trail.ToArray();
            var containedLocations = grid.Points.Where(x =>
            {
                var neighbours = new[] { grid[x.X + 1, x.Y], grid[x.X - 1, x.Y], grid[x.X,  x.Y + 1], grid[x.X, x.Y - 1] };
                var contained = 
            });

        }
    }
}
