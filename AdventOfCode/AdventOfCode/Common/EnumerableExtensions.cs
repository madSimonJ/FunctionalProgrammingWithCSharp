namespace AdventOfCode.Common
{
    public static class EnumerableExtensions
    {
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
    }
}
