using System.Text.RegularExpressions;
using AdventOfCode.Common;
using MoreLinq;

namespace AdventOfCode._2023
{
    public class Day05
    {
        private readonly string testInput = @"seeds: 79 14 55 13

seed-to-soil map:
50 98 2
52 50 48

soil-to-fertilizer map:
0 15 37
37 52 2
39 0 15

fertilizer-to-water map:
49 53 8
0 11 42
42 0 7
57 7 4

water-to-light map:
88 18 7
18 25 70

light-to-temperature map:
45 77 23
81 45 19
68 64 13

temperature-to-humidity map:
0 69 1
1 0 69

humidity-to-location map:
60 56 37
56 93 4";

        private static IEnumerable<uint> GetSeeds(string input) =>
            new Regex("(?<=seeds: )(.*)(?=\n)").Match(input).Value
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(uint.Parse);

        private static IEnumerable<uint> GetSeeds2(string input) =>
            new Regex("(?<=seeds: )(.*)(?=\n)").Match(input).Value
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(uint.Parse)
                .Pair((x, y) => (start: x, count: y))
                .SelectMany(x => UEnumerable.URange(x.start, x.count));

        private static IDictionary<string, IEnumerable<Map>> GetMap(string input)
        {
            var mapStrings = input.Split("\r\n\r\n")
                .Select(x => x.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
                .Skip(1);

            var maps = mapStrings.Select(x => (
                Name: x[0].Replace(" map", "").Replace(":", "").Trim(),
                Value: x.Skip(1).Select(y => y.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                    .Select(y => new Map
                    {
                        DestinationRangeStart = uint.Parse(y[0]),
                        SourceRangeStart = uint.Parse(y[1]),
                        RangeLength = uint.Parse(y[2])
                    })
            ));

            var mapsDict = maps.ToDictionary(x => x.Name, x => x.Value);

            return mapsDict;
        }

        private static Func<uint, uint> CreateTransformer(IEnumerable<Map> maps)
        {
            uint f(uint x)
            {
                var rule = maps.FirstOrDefault(y =>
                    x.IsBetween(y.SourceRangeStart, y.SourceRangeStart + y.RangeLength));
                return rule == default
                    ? x
                    : x - rule.SourceRangeStart + rule.DestinationRangeStart;
            }

            return f;
        }

        [Fact]
        public void Day05_Test01()
        {
            var seeds = GetSeeds(testInput);
            seeds.Should().BeEquivalentTo(new[] { 79, 14, 55, 13 });
        }

        [Fact]
        public void Day05_Test02()
        {
            var seeds = GetMap(testInput);
            var seedToSoil = seeds["seed-to-soil"].ToArray();
            seedToSoil[0].DestinationRangeStart.Should().Be(50);
            seedToSoil[0].SourceRangeStart.Should().Be(98);
            seedToSoil[0].RangeLength.Should().Be(2);

            seedToSoil[1].DestinationRangeStart.Should().Be(52);
            seedToSoil[1].SourceRangeStart.Should().Be(50);
            seedToSoil[1].RangeLength.Should().Be(48);

            seedToSoil.Should().HaveCount(2);

            var humidityToLocation = seeds["humidity-to-location"].ToArray();
            humidityToLocation[0].DestinationRangeStart.Should().Be(60);
            humidityToLocation[0].SourceRangeStart.Should().Be(56);
            humidityToLocation[0].RangeLength.Should().Be(37);

            humidityToLocation[1].DestinationRangeStart.Should().Be(56);
            humidityToLocation[1].SourceRangeStart.Should().Be(93);
            humidityToLocation[1].RangeLength.Should().Be(4);

            humidityToLocation.Should().HaveCount(2);

        }

        private static uint CalculateLocationValue(uint seed, IDictionary<string, Func<uint, uint>> transformations) =>
            seed
                .Map(x => transformations["seed-to-soil"](x))
                .Map(x => transformations["soil-to-fertilizer"](x))
                .Map(x => transformations["fertilizer-to-water"](x))
                .Map(x => transformations["water-to-light"](x))
                .Map(x => transformations["light-to-temperature"](x))
                .Map(x => transformations["temperature-to-humidity"](x))
                .Map(x => transformations["humidity-to-location"](x));

        public static Func<uint, uint> MakeLocationCalculator(string input)
        {
            var maps = GetMap(input);
            var transformers = maps.Select(x => (x.Key, CreateTransformer(x.Value)))
                .ToDictionary(x => x.Key, x => x.Item2);
            uint LocCalc(uint x) => CalculateLocationValue(x, transformers);
            return LocCalc;
        }

        [Theory]
        [InlineData(79, 81, 81, 81, 74, 78, 78, 82)]
        [InlineData(14, 14, 53, 49, 42, 42, 43, 43)]
        [InlineData(55, 57, 57, 53, 46, 82, 82, 86)]
        [InlineData(13, 13, 52, 41, 34, 34, 35, 35)]
        public void Day05_Test03(uint seed, uint soil, uint fertiliser, uint water, uint light, uint temp,
            uint humidity, uint loc)
        {
            var transformations = testInput.Map(GetMap).Select(x => (
                x.Key,
                CreateTransformer(x.Value)
            )).ToDictionary(x => x.Key, x => x.Item2);

            var soilValue = seed.Map(x => transformations["seed-to-soil"](x));
            soilValue.Should().Be(soil);

            var fertiliserValue = soilValue.Map(x => transformations["soil-to-fertilizer"](x));
            fertiliserValue.Should().Be(fertiliser);

            var waterValue = fertiliserValue.Map(x => transformations["fertilizer-to-water"](x));
            waterValue.Should().Be(water);

            var lightValue = waterValue.Map(x => transformations["water-to-light"](x));
            lightValue.Should().Be(light);

            var temperatureValue = lightValue.Map(x => transformations["light-to-temperature"](x));
            temperatureValue.Should().Be(temp);

            var humidityValue = temperatureValue.Map(x => transformations["temperature-to-humidity"](x));
            humidityValue.Should().Be(humidity);

            var locationValue = humidityValue.Map(x => transformations["humidity-to-location"](x));
            locationValue.Should().Be(loc);

        }

        [Theory]
        [InlineData(79, 82)]
        [InlineData(14, 43)]
        [InlineData(55, 86)]
        [InlineData(13, 35)]
        public void Day05_Test04(uint seed, uint location)
        {
            var calc = MakeLocationCalculator(testInput);
            calc(seed).Should().Be(location);
        }

        [Fact]
        public void Day05_Part01()
        {
            var input = File.ReadAllText("./2023/Day05Input.txt");
            var seeds = GetSeeds(input);
            var calc = MakeLocationCalculator(input);
            var locations = seeds.Select(x => calc(x));
            locations.Min().Should().Be(240320250u);
        }

        [Fact]
        public void Day05_Test05()
        {
            var range = UEnumerable.URange(100, 100).ToArray();
            range.Should().HaveCount(100);
            range.First().Should().Be(100);
            range.Last().Should().Be(199);
        }

        [Fact]
        public void Day05_Test06()
        {
            var seeds = GetSeeds2(testInput).ToArray();
            seeds.Should().HaveCount(27);
        }

        [Fact]
        public void Day05_Part02()
        {
            var input = File.ReadAllText("./2023/Day05Input.txt");
            var seeds = GetSeeds2(input).ToArray();
            var calc = MakeLocationCalculator(input);
            var locations = seeds.Select(x => calc(x));
            locations.Min().Should().Be(240320250u);
        }

        [Fact]
        public void Day05_Test07()
        {
            var maps = GetMap(testInput);
            var hToLMap = maps["humidity-to-location"].ToArray();
            var hToLSources = hToLMap.Select(x => x.DestinationRangeStart).Append(0U).OrderBy(x => x);
            var lRanges = hToLSources.Pairwise((a, b) => (start: a, end: b - 1));

            var reversedMaps = maps.Select(x => x.Value)
                .Reverse().ToArray();

            var finalRanges = reversedMaps.Aggregate(lRanges, (agg, x) => 
                agg.SelectMany(z  =>
                {
                    var xArr = x.ToArray();

                    var reversedStart = xArr.SingleOrDefault(y => z.start.IsBetween(y.DestinationRangeStart, y.DestinationRangeStart + y.RangeLength - 1))
                        .Map(y => y == null ? z.start : z.start - y.DestinationRangeStart + y.SourceRangeStart);

                    var length = z.end - z.start;

                    var overlapLeft = xArr.Where(y =>
                            y.SourceRangeStart < reversedStart &&
                            (y.SourceRangeStart + y.RangeLength) > reversedStart)
                        .Select(y => (reversedStart, y.SourceRangeStart + y.RangeLength - 1));

                    var overlapRight = xArr.Where(y =>
                            y.SourceRangeStart < (reversedStart + length - 1) &&
                            (y.SourceRangeStart + y.RangeLength - 1) > reversedStart
                        )
                        .Select(y => (y.SourceRangeStart, reversedStart + length - 1))
                        .ToArray();

                    var completelyContained = x.Where(y =>
                            y.SourceRangeStart >= reversedStart &&
                            (y.SourceRangeStart + y.RangeLength - 1) <= (reversedStart + length))
                        .Select(y => (y.SourceRangeStart, y.SourceRangeStart + y.RangeLength - 1))
                        .ToArray();

                    var combinedRanages = new[]
                    {
                        overlapLeft,
                        completelyContained,
                        overlapRight
                    }.SelectMany(y => y).ToArray();

                    var missingValues = combinedRanages.Select(a => a.Item1)
                        .Concat(combinedRanages.Select(a => a.Item2))
                        .Distinct()
                        .OrderBy(x => x)
                        .Pairwise((a, b) => (a, b))
                        .ToArray()
                        .Except(combinedRanages);

                    //var mergedValues = 

                    return combinedRanages.Any()
                        ? combinedRanages
                        : new[] { (reversedStart, reversedStart + length - 1) };


                }).ToArray()).ToArray();
        }


        public record Map
        {
            public uint DestinationRangeStart { get; set; }
            public uint SourceRangeStart { get; set; }
            public uint RangeLength { get; set; }
        }

    }
}
