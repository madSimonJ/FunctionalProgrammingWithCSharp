using MartianTrail.Common;
using MartianTrail.Entities;
using MartianTrail.PlayerInteraction;
using MartianTrail.RandomNumbers;

namespace MartianTrail.GamePhases
{
    public class UpdateProgress : IGamePhase
    {
        private readonly IRandomNumberGenerator _rnd;

        public UpdateProgress(IRandomNumberGenerator rnd)
        {
            _rnd = rnd;
        }

        public GameState DoPhase(IPlayerInteraction playerInteraction, GameState oldState)
        {
            playerInteraction.WriteMessage("End of Sol " + oldState.CurrentSol);
            var distanceTraveled = oldState.Inventory.NumberOfBatteries *
                (oldState.UserActionSelectedThisTurn == PlayerActions.PushOn ? 100 : 50);

            var batteriesUsedUp = this._rnd.BetweenZeroAnd(4);

            var foodUsedUp = this._rnd.BetweenZeroAnd(5) * 20;

            var newState = oldState with
            {
                DistanceTraveled = oldState.DistanceTraveled + distanceTraveled,
                Inventory = oldState.Inventory with
                {
                    NumberOfBatteries = (oldState.Inventory.NumberOfBatteries - batteriesUsedUp)
                        .Map(x => x >= 0 ? x : 0),
                    Food = (oldState.Inventory.Food - foodUsedUp)
                        .Map(x => x >= 0 ? x : 0)
                }
            };
            
            playerInteraction.WriteMessage("You have traveled " + distanceTraveled + " this Sol.",
                "That's a total distance of " + newState.DistanceTraveled);

            playerInteraction.WriteMessageConditional(batteriesUsedUp > 0,
                "You have " + newState.Inventory.NumberOfBatteries + " remaining");

            playerInteraction.WriteMessageConditional(foodUsedUp > 0,
                "You have " + newState.Inventory.Food + " remaining");

            return newState;
        }
    }
}
