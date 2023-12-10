using System.Collections;

namespace AdventOfCode.Common;

public static class EnumerableExtensions
{

    public static int Product(this IEnumerable<int> @this) =>
        @this.Aggregate(1, (agg, x) => agg * x);

    public static uint Product(this IEnumerable<uint> @this) =>
        @this.Aggregate(1u, (agg, x) => agg * x);

    public static long Product(this IEnumerable<long> @this) =>
        @this.Aggregate(1l, (agg, x) => agg * x);

    public static IEnumerable<T2> Scan<T1, T2>(this IEnumerable<T1> @this, T2 seed, Func<T2, T1, T2> acc)
    {
        var curr = seed;
        yield return curr;

        foreach (var i in @this)
        {
            curr = acc(curr, i);
            yield return curr;
        }
    }

    public static int findIndex<T>(this IEnumerable<T> @this, Func<T, bool> cond)
    {
        var result = @this.Select((x, i) => (x, i)).FirstOrDefault(x => cond(x.x));
        return result.i;
    }

    public static bool ConsecutiveAny<T>(this IEnumerable<T> @this, Func<T, T, bool> condition) =>
        @this.ToArray().Map(x => x.Zip(x.Skip(1)).Select(y => condition(y.First, y.Second)).FirstOrDefault(z => z));

    public static IEnumerable<T2> ConsecutiveSelect<T1, T2>(this IEnumerable<T1> @this, Func<T1, T1, T2> f)
    {
        var thisArr = @this.ToArray();
        var thisArrCompare = thisArr.Skip(1).Append(default(T1));
        var newEnumerable = thisArr.Zip(thisArrCompare).Select(x => f(x.First, x.Second));
        return newEnumerable;
    }

    public static T IterateUntil<T>(this T @this, Func<T, bool> end, Func<T, T> iter)
    {
        var curr = @this;

        while (!end(curr))
        {
            curr = iter(curr);
        }

        return curr;
    }

    public static IEnumerable<T2> Pair<T1, T2>(this IEnumerable<T1> @this, Func<T1, T1, T2> f) =>
        @this.Select((x, i) => (x, i / 2))
            .GroupBy(x => x.Item2)
            .Select(x => f(x.First().x, x.Last().x));


}


public static class UEnumerable
{
    public static IEnumerable<uint> URange(uint start, uint count)
    {
        return count == 0 ? Enumerable.Empty<uint>() : new URangeIterator(start, count);
    }

    private sealed class URangeIterator : IEnumerable<uint>
    {
        private readonly uint _start;
        private readonly uint _count;

        public URangeIterator(uint start, uint count)
        {
            _start = start;
            _count = count;
        }

        public IEnumerator<uint> GetEnumerator() =>
            new URangeEnumerator(_start, _count);

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    private sealed class URangeEnumerator : IEnumerator<uint>
    {
        private uint _start;
        private uint _count;
        private uint iterationNum = 0;
        private bool started = false;

        public URangeEnumerator(uint start, uint count)
        {
            _start = start;
            _count = count;
        }

        public bool MoveNext()
        {
            if (iterationNum >= (_count - 1)) return false;

            if (started)
                iterationNum += 1;
            else
                started = true;

            return true;

        }

        public void Reset()
        {
            iterationNum = 0;
            started = false;
        }

        public uint Current => _start + iterationNum;

        object IEnumerator.Current => Current;

        public void Dispose()
        {

        }
    }
}