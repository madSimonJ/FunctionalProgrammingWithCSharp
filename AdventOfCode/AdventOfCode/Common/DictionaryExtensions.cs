namespace AdventOfCode.Common
{
    public static class DictionaryExtensions
    {
        public static Func<TK, TV> ToLookupWithDefault<TK, TV>(this IDictionary<TK, TV> @this, Func<TK, TV>? defaultFunc) =>
            x => @this.ContainsKey(x)
                ? @this[x]
                : defaultFunc(x);

        public static TValue TryGetKey<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, Func<TKey,TValue> f)
        {
            if(@this.ContainsKey(key)) 
                return @this[key];
            var newValue = f(key);
            @this.Add(key, newValue);
            return newValue;
        }

    }

    public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue> 
        where TKey : notnull
    {
        public new TValue this[TKey key]
        {
            get
            {
                if (!TryGetValue(key, out var val))
                {
                    val = default;
                    Add(key, val);
                }
                return val;
            }
            set { base[key] = value; }
        }
    }
}
