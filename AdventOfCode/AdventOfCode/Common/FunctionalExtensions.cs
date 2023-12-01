namespace AdventOfCode.Common
{
    public static class FunctionalExtensions
    {
        public static TOut Map<TIn, TOut>(this TIn @this, Func<TIn, TOut> f) => 
            f(@this);

        public static bool Validate<T>(this T @this, params Func<T, bool>[] rules) =>
            rules.All(x => x(@this));
    }
}
