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

        public static string JoinWith(this IEnumerable<string> @this, string j) =>
            string.Join(j, @this);

        public static string SectionBetweenCharacters(this string @this, string from, string to) =>
            new Regex($"(?<=\\{from})(.*?)(?=\\{to})").Match(@this).Value;

        public static string SectionAfterCharacter(this string @this, string from) =>
            new Regex($"[^{from}]*$").Match(@this).Value;

        public static int ToInt(this string @this) =>
            int.Parse(@this);

        public static uint ToUInt(this string @this) =>
            uint.Parse(@this);

        public static long ToLong(this string @this) =>
            long.Parse(@this);
    }
}
