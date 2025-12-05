namespace AdventOfCode.Common
{
    public static class IntExtensions
    {
        public static bool IsBetween(this int @this, int from, int end) =>
            @this >= from && @this <= end;

        public static bool IsBetween(this uint @this, uint from, uint end) =>
            @this >= from && @this <= end;

        public static bool IsBetween(this ulong @this, ulong from, ulong end) =>
            @this >= from && @this <= end;
    }
}

