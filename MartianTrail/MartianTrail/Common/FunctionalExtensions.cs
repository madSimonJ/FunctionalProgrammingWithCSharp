using MartianTrail.Entities;

namespace MartianTrail.Common
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

        //public static GameState ContinueGame(this GameState @this, Func<GameState, GameState> f) =>
        //    @this.GameOver || @this.IsDead
        //        ? @this
        //        : f(@this);

        public static GameState Message(this GameState @this, Action act)
        {
            act();
            return @this;
        }

        public static TOut Map<TIn, TOut>(this TIn @this, Func<TIn, TOut> f) =>
            f(@this);

    }
}
