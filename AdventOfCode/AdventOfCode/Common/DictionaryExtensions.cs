namespace AdventOfCode.Common
{
    public static class DictionaryExtensions
    {
        public static Func<TK, TV> ToLookupWithDefault<TK, TV>(this IDictionary<TK, TV> @this, Func<TK, TV>? defaultFunc) =>
            x => @this.ContainsKey(x)
                ? @this[x]
                : defaultFunc(x);

    }
}
