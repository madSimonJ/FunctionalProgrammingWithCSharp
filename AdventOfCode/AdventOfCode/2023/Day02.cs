using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode._2023
{
    public class Day02
    {
        public record GameTurn
        {
            public int Id { get; set; }
            public int MaxBlue { get; set; }
            public int MaxRed { get; set; }
            public int MaxGreen { get; set; }
        }

        private static GameTurn parseGameTurn(string input)
        {
            var id = new Regex("(?<=Game )(.*)(?=:)").Match(input).Map(x => int.Parse(x.Value));

            var turns = input.Split(":")[1].Split(";");
            var balls = turns.Select(x => x.Split(", "))
                .SelectMany(x => x.Select(y => y.Trim().Split(" ")))
                .Select(x => (
                        Qty: int.Parse(x[0]),
                        Colour: x[1]
                    ))
                .ToArray();

            var totals = balls.Aggregate(new GameTurn { Id = id}, (agg, x) => x.Colour switch
            {
                "red" when x.Qty > agg.MaxRed => agg with { MaxRed = x.Qty },
                "blue" when x.Qty > agg.MaxBlue => agg with { MaxBlue = x.Qty },
                "green" when x.Qty > agg.MaxGreen => agg with { MaxGreen = x.Qty },
                _ => agg
            });
                return totals;
        }

        private static int CalculatePower(GameTurn gameTurn) =>
            gameTurn.MaxRed * gameTurn.MaxGreen * gameTurn.MaxBlue;

        private static int CalculatePossibleTurnTotal(IEnumerable<GameTurn> turns, int red = 12, int green = 13, int blue = 14)
        {
            var answer = turns.Where(x => x.MaxRed <= red && x.MaxGreen <= green && x.MaxBlue <= blue)
                .Sum(x => x.Id);
            return answer;
        }




        [Theory]
        [InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 4, 2, 6, 1)]
        [InlineData("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 1, 3, 4, 2)]
        [InlineData("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 20, 13, 6, 3)]
        [InlineData("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 14, 3, 15, 4)]
        [InlineData("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 6, 3, 2, 5)]
        public void Day02_Test01(string input, int red, int green, int blue, int id)
        {
            var gameTurn = parseGameTurn(input);
            gameTurn.MaxRed.Should().Be(red);
            gameTurn.MaxGreen.Should().Be(green);
            gameTurn.MaxBlue.Should().Be(blue);
            gameTurn.Id.Should().Be(id);
        }

        [Fact]
        public void Day02_Test02()
        {
            var input = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green".Split(Environment.NewLine);
            var answer = input.Select(parseGameTurn).Map(x => CalculatePossibleTurnTotal(x));
            answer.Should().Be(8);

        }

        [Fact]
        public void Day02_Part01()
        {
            var input = File.ReadAllLines("./2023/Day02Input.txt");
            var answer = input.Select(parseGameTurn).Map(x => CalculatePossibleTurnTotal(x));
            answer.Should().Be(2265);

        }


        [Theory]
        [InlineData("Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green", 48)]
        [InlineData("Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue", 12)]
        [InlineData("Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red", 1560)]
        [InlineData("Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red", 630)]
        [InlineData("Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green", 36)]
        public void Day02_Test03(string input, int expectedPower)
        {
            var power = parseGameTurn(input).Map(CalculatePower);
            power.Should().Be(expectedPower);

        }

        [Fact]
        public void Day02_Test04()
        {
            var input = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green".Split(Environment.NewLine);
            var answer = input.Select(parseGameTurn).Sum(CalculatePower);
            answer.Should().Be(2286);

        }

        [Fact]
        public void Day02_Part02()
        {
            var input = File.ReadAllLines("./2023/Day02Input.txt");
            var answer = input.Select(parseGameTurn).Sum(CalculatePower);
            answer.Should().Be(64097);

        }



    }
}
