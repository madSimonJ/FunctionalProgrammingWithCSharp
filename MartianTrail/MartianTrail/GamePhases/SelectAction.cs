using MartianTrail.Common;
using MartianTrail.Entities;
using MartianTrail.MiniGame;
using MartianTrail.PlayerInteraction;
using MartianTrail.RandomNumbers;

namespace MartianTrail.GamePhases
{
    public class SelectAction : IGamePhase
    {
        private readonly IRandomNumberGenerator _rnd;
        private readonly IPlayerInteraction _playerInteraction;
        private readonly IPlayMiniGame _playMiniGame;


        public SelectAction(IRandomNumberGenerator rnd, IPlayerInteraction playerInteraction, IPlayMiniGame playMiniGame)
        {
            _rnd = rnd;
            _playerInteraction = playerInteraction;
            _playMiniGame = playMiniGame;
        }

        public GameState DoPhase(IPlayerInteraction playerInteraction, GameState oldState)
        {
            var isWilderness = this._rnd.BetweenZeroAnd(100) > 33;
            var isTradingOutpost = this._rnd.BetweenZeroAnd(100) > (isWilderness ? 90 : 10);
            var isHuntingArea = this._rnd.BetweenZeroAnd(100) > (isWilderness ? 10 : 20);

            var canHuntForSkins = isHuntingArea && this._rnd.BetweenZeroAnd(100) > (33);
            var canHuntForFood = isHuntingArea && this._rnd.BetweenZeroAnd(100) > (33);

            var options = new[]
                {
                    isTradingOutpost ? PlayerActions.TradeAtOutpost : PlayerActions.Unavailable,
                    canHuntForFood ? PlayerActions.HuntForFood : PlayerActions.Unavailable,
                    canHuntForSkins ? PlayerActions.HuntForSkins : PlayerActions.Unavailable,
                    PlayerActions.PushOn
                }.Where(x => x != PlayerActions.Unavailable)
                .Select((x, i) => (
                    Action: x,
                    ChoiceNumber: i + 1
                )).ToArray();

            var messageToPlayer = string.Join(Environment.NewLine,
                new[]
                {
                    "The area you are passing through is " + (isWilderness ? " wilderness" : "a small settlement"),
                    "Here are your options for what you can do:"
                }.Concat(
                    options.Select(x => "\t" + x.ChoiceNumber + " - " + x.Action switch
                    {
                        PlayerActions.TradeAtOutpost => "Trade at the nearby outpost",
                        PlayerActions.HuntForFood => "Hunt for food",
                        PlayerActions.HuntForSkins => "Hunt for Lophroll furs to sell later",
                        PlayerActions.PushOn => "Just push on to travel faster"
                    })
                )
            );

            this._playerInteraction.WriteMessage(messageToPlayer);

            var playerChoice = this._playerInteraction.GetInput("What would you like to do? ");
            var validatedPlayerChoice = playerChoice.IterateUntil(
                x => this._playerInteraction.GetInput("That's not a valid choice.  Please try again."),
                x => x is IntegerInput i && options.Any(y => y.ChoiceNumber == i.IntegerFromUser)
            );

            var playerChoiceInt = (validatedPlayerChoice as IntegerInput).IntegerFromUser;
            var actionToDo = options.Single(x => x.ChoiceNumber == playerChoiceInt).Action;

            Func<GameState, GameState> actionFunc = actionToDo switch
            {
                PlayerActions.TradeAtOutpost => DoTrading,
                PlayerActions.HuntForFood => DoHuntingForFood,
                PlayerActions.HuntForSkins => DoHuntingForFurs,
                PlayerActions.PushOn => DoPushOn
            };

            var updatedState = actionFunc(oldState);

            return updatedState with
            {
                UserActionSelectedThisTurn = actionToDo
            };
        }

        private static GameState DoTrading(GameState state)
        {
            return state;
        }

        private GameState DoHuntingForFood(GameState state)
        {
            this._playerInteraction.WriteMessage("You've decided to hunt Vrolids for food.",
                "For that you'll have to play the mini-game...");

            var accuracy = this._playMiniGame.PlayMiniGameForSuccessFactor();

            var message = accuracy switch
            {
               >= 0.9M => new[]
               {
                   "Great shot!  You brought down a whole load of the things!",
                   "Vrolid burgers are on you today!"
               },
               0 => new[]
               {
                   "You missed.  Were you taking a nap?"
               },
               _ => new [] 
               {
                   "Not a bad shot",
                   "You brought down at least a couple",
                   "Don't go too crazy eating tonight"
               }
            };

            this._playerInteraction.WriteMessage(message);

            var laserChargesUsed = 50 * (1 - accuracy);
            var foodGained = 100 * accuracy;

            return state with
            {
                Inventory = state.Inventory with
                {
                    LaserCharges = state.Inventory.LaserCharges - (int)laserChargesUsed,
                    Food = state.Inventory.Food + (int)foodGained
                }
            };
        }

        private static GameState DoHuntingForFurs(GameState state)
        {
            return state;
        }

        private static GameState DoPushOn(GameState state) => state;

    }
}