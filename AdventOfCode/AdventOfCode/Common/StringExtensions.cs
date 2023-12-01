using System.Text.RegularExpressions;

namespace AdventOfCode.Common
{
    public static class StringExtensions
    {
        public static IEnumerable<(string, string, string)> Split3(this string @this, string recordSplit,
            string fieldSplit) =>
            @this.Split(recordSplit).Select(x => x.Split(fieldSplit)).Select(x => (x[0], x[1], x[2]));

        public static string Replace(this string @this, Regex reg, string replacement, int times = -1) =>
            times == -1
                ? reg.Replace(@this, replacement)
                : reg.Replace(@this, replacement, times);
    }
}
