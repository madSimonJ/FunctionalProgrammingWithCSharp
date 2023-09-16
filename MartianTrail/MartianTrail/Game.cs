using MartianTrail.Entities;
using MartianTrail.PlayerInteraction;

namespace MartianTrail
{
    public class Game
    {
        public GameState Play(GameState initialState, IPlayerInteraction playerInteraction, params IGamePhase[] gamePhases)
        {
            var gp = gamePhases.ToArray();

            var finalState = initialState.IterateUntil(x =>
                gp.Aggregate(x, (acc, y) => acc.ContinueTurn( z => y.DoPhase(playerInteraction, z))),
                x => x.PlayerIsDead || x.ReachedDestination
            );

            return finalState;
        }
    }

    public interface IGamePhase
    {
        GameState DoPhase(IPlayerInteraction playerInteraction, GameState oldState);
    }
}
