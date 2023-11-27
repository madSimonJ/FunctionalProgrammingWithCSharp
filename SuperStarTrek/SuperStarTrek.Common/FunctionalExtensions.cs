namespace ConsoleGame.Common
{
    public static class FunctionalExtensions
    {
        public static T IterateUntil<T>(this T @this, Func<T, T> updateFunction, Func<T, bool> endCondition)
        {
            var currentThis = @this;

            while (!endCondition(currentThis))
            {
                currentThis = updateFunction(currentThis);
            }

            return currentThis;
        }

        public static GameState ContinueGame(this GameState @this, Func<GameState, GameState> f) =>
            @this.IsGameOver()
                ? @this
                : f(@this);

        public static GameState Message(this GameState @this, Action act)
        {
            act();
            return @this;
        }

        public static TOut Map<TIn, TOut>(this TIn @this, Func<TIn, TOut> f) =>
            f(@this);

        public static Operation Iterate<T>(this IEnumerable<Func<T, Operation>> operations, T input)
        {
            foreach (var o in operations)
            {
                var result = o(input);
                if (result is Failure f) return new Failure(f.CapturedException);
            }

            return new Success();
        }

        public static Operation Iterate(this IEnumerable<Func<Operation>> operations)
        {
            foreach (var o in operations)
            {
                var result = o();
                if (result is Failure f) return new Failure(f.CapturedException);
            }

            return new Success();
        }

    }
}
