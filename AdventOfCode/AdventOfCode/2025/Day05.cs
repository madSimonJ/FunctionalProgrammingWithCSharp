namespace AdventOfCode._2025
{
    public readonly record struct IdRange(ulong from, ulong to);
    public readonly record struct Inventory(IEnumerable<IdRange> IdRanges, IEnumerable<ulong> IdsToCheck);

    public static class Day05Solution
    {
        public static Inventory ParseInventory(string input) =>
            input.Split($"{Environment.NewLine}{Environment.NewLine}")
            .Map(x =>
                new Inventory
                (
                    x[0].Split(Environment.NewLine).Select(y => y.Split("-").Map(y => new IdRange(ulong.Parse(y[0]), ulong.Parse(y[1])))),
                    [.. x[1].Split(Environment.NewLine).Select(ulong.Parse)]
                )
            );

        public static IEnumerable<ulong> GetFreshGoods(Inventory inventory) =>
            inventory
            .IdsToCheck
            .Where(x => inventory.IdRanges.Any(y => x.IsBetween(y.from, y.to)));

        public static IEnumerable<IdRange> GetFreshIds(Inventory inventory) =>
                inventory
                .IdRanges
                .Aggregate(Array.Empty<IdRange>(),
                (existingIdRanges, newIdRange) =>
                {
                    var proposedItemInsideExistingRange = existingIdRanges.Any(existing => newIdRange.from.IsBetween(existing.from, existing.to) && newIdRange.to.IsBetween(existing.from, existing.to));
                    var overlap = existingIdRanges
                        .Where(existing => newIdRange.from.IsBetween(existing.from, existing.to) || newIdRange.to.IsBetween(existing.from, existing.to));
                    var proposedFrom = overlap.Any() ? overlap.Select(y => y.from).Append(newIdRange.from).Min() : newIdRange.from;
                    var proposedTo = overlap.Any() ? overlap.Select(y => y.to).Append(newIdRange.to).Max() : newIdRange.to;

                    return proposedItemInsideExistingRange
                        ? existingIdRanges
                        : [.. existingIdRanges
                            .Where(y => !overlap.Contains(y))
                            .Append(new IdRange(
                                proposedFrom,
                                proposedTo
                            )).Distinct()];

                });

        public static ulong CountIds(IEnumerable<IdRange> idRanges) =>
            idRanges.Aggregate(0UL, (agg, x) => agg + (x.to - x.from + 1));
        }

    public class Day05
    {
        [Fact]
        public void Day05_Test01()
        {
            var input = "3-5\r\n10-14\r\n16-20\r\n12-18\r\n\r\n1\r\n5\r\n8\r\n11\r\n17\r\n32";
            var parsedInput = Day05Solution.ParseInventory(input);
            parsedInput.Should().BeEquivalentTo(
                new Inventory(
                    [
                        new IdRange(3, 5),
                        new IdRange(10, 14),
                        new IdRange(16, 20),
                        new IdRange(12, 18)
                    ],
                    [1, 5, 8, 11, 17, 32]
                ));
        }

        [Fact]
        public void Day05_Test02()
        {
            var input = File.ReadAllText("./2025/Day05input.txt");
            var act = () => Day05Solution.ParseInventory(input);

            act.Should().NotThrow();
        }

        [Fact]
        public void Day05_Test03()
        {
            var input = "3-5\r\n10-14\r\n16-20\r\n12-18\r\n\r\n1\r\n5\r\n8\r\n11\r\n17\r\n32";
            var parsedInput = Day05Solution.ParseInventory(input);
            var freshGoods = Day05Solution.GetFreshGoods(parsedInput);
            freshGoods.Should().BeEquivalentTo([5, 11, 17]);
        }

        [Fact]
        public void Day05_Part01()
        {
            var input = File.ReadAllText("./2025/Day05input.txt");
            var parsedInput = Day05Solution.ParseInventory(input);
            var freshGoods = Day05Solution.GetFreshGoods(parsedInput);
            freshGoods.Should().HaveCount(726);
        }

        [Fact]
        public void Day05_Test04()
        {
            var input = "3-5\r\n10-14\r\n16-20\r\n12-18\r\n\r\n1\r\n5\r\n8\r\n11\r\n17\r\n32";
            var parsedInput = Day05Solution.ParseInventory(input);
            var freshIds = Day05Solution.GetFreshIds(parsedInput);
            var idCount = Day05Solution.CountIds(freshIds);
            idCount.Should().Be(14);
        }

        [Fact]
        public void Day05_Part02()
        {
            var input = File.ReadAllText("./2025/Day05input.txt");
            var parsedInput = Day05Solution.ParseInventory(input);
            var freshIds = Day05Solution.GetFreshIds(parsedInput).OrderBy(x => x.from).ToArray();
            var idCount = Day05Solution.CountIds(freshIds);
            idCount.Should().Be(354226555270043UL);
        }
    }
}