using Newtonsoft.Json.Linq;

namespace AdventOfCode.Common
{


    public record GridPoint<T>
    {
        public int X { get; set; }
        public int Y { get; set; }
        public T Value { get; set; }
    }

    public record Grid<T>
    {
        public GridPoint<T> this[int x, int y] => Points.SingleOrDefault(z => z.X == x && z.Y == y);

        public Grid(string rawInput, string lineSplit, string cellSplit, Func<int, int, string, T> valueFactor)
        {
            var g = rawInput.Split(lineSplit)
                .SelectMany((l, y) => (
                    l.Split(cellSplit).Select((r, x) => new GridPoint<T>()
                        { Value = valueFactor(x, y, r), X = x, Y = y })
                )).ToArray();

            Points = g;
            MaxX = g.Max(x => x.X);
            MaxY = g.Max(y => y.Y);
        }


        public Grid(string rawInput, string lineSplit, Func<int, int, string, T> valueFactor)
        {
            var g = rawInput.Split(lineSplit)
                .SelectMany((l, y) => (
                    l.ToArray().Select(x => x.ToString()).Select((r, x) => new GridPoint<T>()
                        { Value = valueFactor(x, y, r), X = x, Y = y })
                )).ToArray();

            Points = g;
            MaxX = g.Max(x => x.X);
            MaxY = g.Max(y => y.Y);
        }

        public Grid(int sizeX, int sizeY, Func<int, int, T> valueFactory)
        {
            MaxX = sizeX;
            MaxY = sizeY;

            this.Points = Enumerable.Range(0, sizeX)
                .SelectMany(x => Enumerable.Range(0, sizeY)
                    .Select(y => new GridPoint<T>
                    {
                        Value = valueFactory(x, y),
                        X = x,
                        Y = y
                    }));
        }
        public IEnumerable<GridPoint<T>> Points { get; init; }
        public int MaxX { get; set; }
        public int MaxY { get; set; }
    }

    public static class GridExtensions
    {
        public static IEnumerable<GridPoint<T>> GetNeighbours<T>(this Grid<T> @this, int x, int y) =>
            @this.Points.Where(z =>
               z.X.IsBetween(x - 1, x + 1) &&
               z.Y.IsBetween(y - 1, y + 1)
                &&
                !(z.X == x && z.Y == y)
            );

        public static IEnumerable<GridPoint<T>> GetNeighbours<T>(this Grid<T> @this, int x1, int y1, int x2, int y2) =>
            @this.Points.Where(z =>
                z.X.IsBetween(x1 - 1, x2 + 1) &&
                z.Y.IsBetween(y1 - 1, y2 + 1) &&
                !(z.X >= x1 && z.Y >= y1 && z.X <= x2 && z.Y <= y2)
            );

        public static Grid<T> UpdateGrid<T>(this Grid<T> @this, Coord from, Coord to, Func<T, int, int, T> update)
        {
            var returnValue = @this with
            {
                Points = @this.Points.Select(x =>
                    x with
                    {
                        Value = x.X.IsBetween(from.X, to.X) &&
                                x.Y.IsBetween(from.Y, to.Y)
                            ? update(x.Value, x.X, x.Y)
                            : x.Value
                    }
                ).ToArray()
            };
            var s = returnValue.Points.Sum(x => x.Value as int?);
            return returnValue;
        }

        public static Coord GetValueLocation<T>(this Grid<T> @this, T value) =>
            @this.Points.FirstOrDefault(x => EqualityComparer<T>.Default.Equals(x.Value, value))
                .Map(x => new Coord { X = x.X, Y = x.Y });
    }
}
