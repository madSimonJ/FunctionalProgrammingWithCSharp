namespace AdventOfCode.Common
{
    public static class MemoiseExtensions
    {
        public static Func<T1, T2> Memoise<T1,T2>(this Func<T1, T2> @this) where T1 : notnull
        {
            var memos = new Dictionary<T1,T2>();
            return x =>
            {
                if (memos.TryGetValue(x, out T2? value))
                    return value;
                memos.Add(x, @this(x));
                return memos[x];
            };
        }

        public static Func<T1, T2, T3> Memoise<T1, T2, T3>(this Func<T1, T2, T3> @this) 
            where T1 : notnull  
            where T2: notnull
        {
            var memos = new Dictionary<(T1,T2), T3>();
            return (x, y) =>
            {
                if (memos.TryGetValue((x,y), out T3 value))
                    return value;
                memos.Add((x, y), @this(x, y));
                return memos[(x, y)];
            };
        }
    }
}
