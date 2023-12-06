using AdventOfCode.Common;

namespace AdventOfCode._2023
{
    public class Day04
    {
        private static int CalculateNumberOfWinners(string input)
        {
            var numbers = input.SectionBetweenCharacters(":", "|").Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var cards = input.SectionAfterCharacter("|").Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var winners = cards.Count(x => numbers.Contains(x));
            return winners;
        }

        private static int CalculateScore(string input)
        {
            var winners = CalculateNumberOfWinners(input);
            var answer = (int)Math.Pow(2, winners - 1);
            return answer;
        }


        [Theory]
        [InlineData("Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53", 8)]
        [InlineData("Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19", 2)]
        [InlineData("Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1", 2)]
        [InlineData("Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83", 1)]
        [InlineData("Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36", 0)]
        [InlineData("Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11", 0)]
        public void Day04_Test01(string input, int expected)
        {
            var answer = CalculateScore(input);
            answer.Should().Be(answer);
        }

        [Fact]
        public void Day04_Part01()
        {
            var input = File.ReadAllLines("./2023/Day04Input.txt");
            var scores = input.Sum(CalculateScore);
            scores.Should().Be(20855);

        }

        [Fact]
        public void Day04_Test02()
        {
            var cards = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11".Split(Environment.NewLine)
                .Select((x, i) => (Card: x, Index: i))
                .ToArray();

            var progress = (cards: cards, Index: 0);

            var scoringCards = progress.IterateUntil(x => x.cards.Length - 1 == x.Index,
                (agg) =>
                {
                    var card = agg.cards[agg.Index].Card;
                    var score = CalculateNumberOfWinners(card);
                    var id = int.Parse(card.Substring(5, card.IndexOf(":") - 5));
                    var duplicateCards = cards.Skip(id).Take(score);
                    var newCards = agg.cards.Concat(duplicateCards).OrderBy(x => x.Card)
                        .Select((x, i) => (x.Card, i)).ToArray();
                    return (
                        newCards,
                        agg.Index + 1
                    );
                });

            scoringCards.cards.Length.Should().Be(30);
        }


        [Fact]
        public void Day04_Test03()
        {
            var cards = @"Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11".Split(Environment.NewLine)
                .Select((x, i) => (i + 1, CalculateNumberOfWinners(x)))
                .ToDictionary(x => x.Item1, x => x.Item2);

            var cardCounts = cards.Select(x => (Id: x.Key, Count: 1)).ToArray();

            var finalTotal = cards.Aggregate(cardCounts, (agg, x) =>
            {
                var score = x.Value;
                var count = agg.Single(y => y.Id == x.Key).Count;
                var updatedCounts = agg.Select(y => (y.Id, y.Id.IsBetween(x.Key + 1, x.Key + score) ? y.Count + count : y.Count));
                return updatedCounts.ToArray();
            });

            finalTotal.Sum(x => x.Count).Should().Be(30);

        }

        [Fact]
        public void Day04_Part02()
        {
            var cards = File.ReadAllLines("./2023/Day04Input.txt")
                .Select((x, i) => (i + 1, CalculateNumberOfWinners(x)))
                .ToDictionary(x => x.Item1, x => x.Item2);

            var cardCounts = cards.Select(x => (Id: x.Key, Count: 1)).ToArray();

            var finalTotal = cards.Aggregate(cardCounts, (agg, x) =>
            {
                var score = x.Value;
                var count = agg.Single(y => y.Id == x.Key).Count;
                var updatedCounts = agg.Select(y => (y.Id, y.Id.IsBetween(x.Key + 1, x.Key + score) ? y.Count + count : y.Count));
                return updatedCounts.ToArray();
            });

            finalTotal.Sum(x => x.Count).Should().Be(5489600);
        }
    }
}
