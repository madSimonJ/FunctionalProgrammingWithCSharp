namespace AdventOfCode.Common
{
    public static class IntExtensions
    {
        public static bool IsBetween(this int @this, int from, int end) =>
            @this >= from && @this <= end;
    }
}
