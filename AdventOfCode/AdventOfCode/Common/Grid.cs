using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
