using AdventOfCode.Common;

namespace AdventOfCode._2015
{
    public record LightInstruction
    {
        public Coord From{ get; set; }
        public Coord To { get; set; }
        public Func<int, int, int, int> Change { get; set; }
    }

    public class Day06
    {
        private static LightInstruction ParseInstructions(string input) =>
            input.Split(" ") switch
            {
                [ "turn", var onOrOff, var from, _, var to ] => 
                    new LightInstruction
                    {
                        From = from.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        To = to.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        Change = (v, x, y) => onOrOff == "on" ? 1 : 0
                    },
                ["toggle", var from, _, var to ] => 
                    new LightInstruction
                    {
                        From = from.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        To = to.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        Change = (v, x,y) => v == 0 ? 1 : 0
                    }
            };

        private static LightInstruction ParseInstructions2(string input) =>
            input.Split(" ") switch
            {
                ["turn", "on", var from, _, var to] =>
                    new LightInstruction
                    {
                        From = from.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        To = to.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        Change = (v, x, y) => (v +  1)
                    },
                ["turn", "off", var from, _, var to] =>
                    new LightInstruction
                    {
                        From = from.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        To = to.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        Change = (v, x, y) => (v - 1).Map(v => v < 0 ? 0 : v)
                    },
                ["toggle", var from, _, var to] =>
                    new LightInstruction
                    {
                        From = from.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        To = to.Split(",").Map(x => new Coord { X = int.Parse(x[0]), Y = int.Parse(x[1]) }),
                        Change = (v, x, y) => v + 2
                    }
            };


        [Fact]
        public void Day06_test01()
        {
            var grid = new Grid<int>(1000, 1000, (x, y) => 0);
            var updatedGrid = grid.UpdateGrid(new Coord { X = 0, Y = 0 }, new Coord { X = 999, Y = 999 }, (_, _, _) => 1);
            updatedGrid.Points.Sum(x => x.Value).Should().Be(1000 * 1000);
        }

        [Fact]
        public void Day06_test02()
        {
            var grid = new Grid<int>(1000, 1000, (x, y) => 0);
            var updatedGrid = grid.UpdateGrid(new Coord { X = 0, Y = 0 }, new Coord { X = 999, Y = 0 }, (_, _, _) => 1);
            updatedGrid.Points.Sum(x => x.Value).Should().Be(1000);
        }

        [Fact]
        public void Day06_test03()
        {
            var instruction = ParseInstructions("turn on 0,0 through 999,999");
            instruction.From.X.Should().Be(0);
            instruction.From.Y.Should().Be(0);

            instruction.To.X.Should().Be(999);
            instruction.To.Y.Should().Be(999);

            instruction.Change(0, 0, 0).Should().Be(1);
            instruction.Change(1, 0, 0).Should().Be(1);
        }

        [Fact]
        public void Day06_test04()
        {
            var instruction = ParseInstructions("toggle 0,0 through 999,0");
            instruction.From.X.Should().Be(0);
            instruction.From.Y.Should().Be(0);

            instruction.To.X.Should().Be(999);
            instruction.To.Y.Should().Be(0);

            instruction.Change(0, 0, 0).Should().Be(1);
            instruction.Change(1, 0, 0).Should().Be(0);
        }

        [Fact]
        public void Day06_test05()
        {
            var instruction = ParseInstructions("turn off 499,499 through 500,500");
            instruction.From.X.Should().Be(499);
            instruction.From.Y.Should().Be(499);

            instruction.To.X.Should().Be(500);
            instruction.To.Y.Should().Be(500);

            instruction.Change(0, 0, 0).Should().Be(0);
            instruction.Change(1, 0, 0).Should().Be(0);
        }

        [Fact]
        public void Day06_test06()
        {
            var input = new[]
            {
                "turn on 0,0 through 999,999",
                "toggle 0,0 through 999,0",
                "turn off 499,499 through 500,500"
            };
            var instructions = input.Select(ParseInstructions);
            var grid = new Grid<int>(1000, 1000, (_, _) => 0);
            var finalGrid = instructions.Aggregate(grid, (agg, x) => agg.UpdateGrid(x.From, x.To, x.Change));
            finalGrid.Points.Sum(x => x.Value).Should().Be((1000 * 1000) - 1000 - 4);


        }

        [Fact]
        public void Day06_Part01()
        {
            var input = File.ReadAllLines("./2015/Day06Input.txt");
            var instructions = input.Select(ParseInstructions);
            var grid = new Grid<int>(1000, 1000, (_,_) => 0);
            var finalGrid = instructions.Aggregate(grid, (agg, x) => agg.UpdateGrid(x.From, x.To, x.Change));
            finalGrid.Points.Sum(x => x.Value).Should().Be(377891);


        }

        [Fact]
        public void Day06_test07()
        {
            var input = new[]
            {
                "turn on 0,0 through 0,0"
            };
            var instructions = input.Select(ParseInstructions);
            var grid = new Grid<int>(1000, 1000, (_, _) => 0);
            var finalGrid = instructions.Aggregate(grid, (agg, x) => agg.UpdateGrid(x.From, x.To, x.Change));
            finalGrid.Points.Sum(x => x.Value).Should().Be(1);
        }

        [Fact]
        public void Day06_test08()
        {
            var input = new[]
            {
                "toggle 0,0 through 999,999"
            };
            var instructions = input.Select(ParseInstructions2);
            var grid = new Grid<int>(1000, 1000, (_, _) => 0);
            var finalGrid = instructions.Aggregate(grid, (agg, x) => agg.UpdateGrid(x.From, x.To, x.Change));
            finalGrid.Points.Sum(x => x.Value).Should().Be(2000000);
        }

        [Fact]
        public void Day06_test09()
        {
            var input = new[]
            {
                "turn on 0,0 through 0,0",
                "toggle 0,0 through 999,999"
            };
            var instructions = input.Select(ParseInstructions2);
            var grid = new Grid<int>(1000, 1000, (_, _) => 0);
            var finalGrid = instructions.Aggregate(grid, (agg, x) => agg.UpdateGrid(x.From, x.To, x.Change));
            finalGrid.Points.Sum(x => x.Value).Should().Be(2000001);
        }

        [Fact]
        public void Day06_Part02()
        {
            var input = File.ReadAllLines("./2015/Day06Input.txt");
            var instructions = input.Select(ParseInstructions2);
            var grid = new Grid<int>(1000, 1000, (_, _) => 0);
            var finalGrid = instructions.Aggregate(grid, (agg, x) => agg.UpdateGrid(x.From, x.To, x.Change));
            finalGrid.Points.Sum(x => x.Value).Should().Be(14110788);


        }
    }
}
