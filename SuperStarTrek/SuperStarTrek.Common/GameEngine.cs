using ConsoleGame.Common.RandomNumbers;

namespace ConsoleGame.Common
{
    public class GameEngine<TGs> where TGs : GameState
    {
        private readonly IRandomNumberGenerator _rnd;
        private Func<IPlayerInteraction> _playerInteraction;
        private readonly Maybe<TGs> _gameState;
        private IEnumerable<Func<IPlayerInteraction, Operation>> _introMessages = Enumerable.Empty<Func<IPlayerInteraction, Operation>>();
        private Func<IRandomNumberGenerator, TGs> _createInitialInventory = x => default;
        private IEnumerable<Func<IPlayerInteraction, TGs, TGs>> _setUpInventory = new [] {(IPlayerInteraction x, TGs y) => y};
        private IEnumerable<Func<TGs, TGs>> _gameTurn = Enumerable.Empty<Func<TGs, TGs>>();
        private IEnumerable<Func<IPlayerInteraction, Operation>> _endGame = Enumerable.Empty<Func<IPlayerInteraction, Operation>>();


        public GameEngine(IRandomNumberGenerator rnd)
        {
            _rnd = rnd;
            this._gameState = new Nothing<TGs>();
        }

        public GameEngine<TGs> IntroductionMessage(params Func<IPlayerInteraction, Operation>[] introMessages)
        {
            this._introMessages = introMessages;
            return this;
        }

        public GameEngine<TGs> InitialInventory(Func<IRandomNumberGenerator, TGs> inventory)
        {
            this._createInitialInventory = inventory;
            return this;
        }

        public GameEngine<TGs> SetUpInventory(params Func<IPlayerInteraction, TGs, TGs>[] gs)
        {
            this._setUpInventory = gs;
            return this;
        }


        public GameEngine<TGs> TakeTurn(params Func<TGs, TGs>[] turn)
        {
            this._gameTurn = turn;
            return this;
        }

        public GameEngine<TGs> EndGame(params Func<IPlayerInteraction, Operation>[] actions)
        {
            this._endGame = actions;
            return this;
        }

        public TGs Play()
        {
            var interaction = this._playerInteraction();
            var initialInventory = this._createInitialInventory(this._rnd);
            this._introMessages.Iterate(interaction);

            return initialInventory;
        }

        public GameEngine<TGs> SetUpInteraction(Func<SetUpInteractions, IPlayerOutput> func)
        {
            var setup = new SetUpInteractions();
            this._playerInteraction = () => new PlayerInteractionClient(func(setup));
            return this;
        }
    }

    public class SetUpInteractions
    {
        public IPlayerOutput Console = new ConsoleOutput();
    }
}
