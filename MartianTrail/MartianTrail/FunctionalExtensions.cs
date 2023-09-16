using MartianTrail.Entities;

namespace MartianTrail
{
    public static class FunctionalExtensions
    {
        public static T IterateUntil<T>(this T @this, Func<T, T> updateFunction, Func<T, bool> endCondition)
        {
            var currentThis = @this;

            LoopBeginning:
                
                if(endCondition(currentThis))
                    goto LoopEnding;

                currentThis = updateFunction(currentThis);
                goto LoopBeginning;

            LoopEnding:

            return currentThis;
        }

        public static GameState ContinueTurn(this GameState @this, Func<GameState, GameState> f) =>
            @this.ReachedDestination || @this.PlayerIsDead
                ? @this
                : f(@this);

        public static GameState Message(this GameState @this, Action act)
        {
            act();
            return @this;
        } 

        public static TOut Map<TIn, TOut>(this TIn @this, Func<TIn, TOut> f) =>
            f(@this);

    }
}
