using System.Collections;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Text;
using MartianTrail;
using Microsoft.VisualBasic.CompilerServices;
using FunctionalExtensions = MartianTrail.Common.FunctionalExtensions;

namespace OtherStuff
{


    public class UnitTest1
    {
        public interface IDieRoll
        {
            int Roll();
        }

        public class DieRoll : IDieRoll
        {
            private readonly Random rnd;

            public DieRoll()
            {
                rnd = new Random();
            }

            public int Roll() =>
                this.rnd.Next(1, 6);
        }

        public record Player
        {
            public int Position { get; set; }
            public int Number { get; set; }
        }


        private static readonly Dictionary<int, int> SnakesAndLadders = new Dictionary<int, int>
        {
            { 5, 18 },
            { 14, 29 },
            { 25, 47 },
            { 39, 3 },
            { 43, 62 },
            { 46, 11 },
            { 52, 31 },
            { 71, 91 },
            { 73, 58 },
            { 74, 96 },
            { 80, 40 },
            { 87, 32 },
            { 93, 70 },
            { 96, 79 },
            { 98, 6 }
        };

        public int PlaySnakesAndLaddersImperative(int noPlayers, IDieRoll die)
        {
            var currentPlayer = 1;
            var playerPositions = new Dictionary<int, int>();
            for (var i = 1; i <= noPlayers; i++)
            {
                playerPositions.Add(i, 1);
            }

            while (!playerPositions.Any(x => x.Value >= 100))
            {
                var dieRoll = die.Roll();
                playerPositions[currentPlayer] += dieRoll;
                if (SnakesAndLadders.ContainsKey(playerPositions[currentPlayer]))
                    playerPositions[currentPlayer] = SnakesAndLadders[playerPositions[currentPlayer]];
                if (dieRoll == 6) continue; // another turn for this player if they roll a 6
                    currentPlayer += 1;
                if (currentPlayer > noPlayers)
                    currentPlayer = 1;
            }

            return currentPlayer;

        }

        public class SnakesAndLaddersEnumerator : IEnumerator<GameState>
        {
            // I need this in case of a restart
            private GameState StartState;

            // old game state -> new game state
            private readonly Func<GameState, GameState> iterator;
            // some tricky logic required to ensure the final
            // game state is iterated.  Normal logic is that if
            // the MoveNext function returns false, then there isn't
            // anything pulled from Current, the loop simply terminates
            private bool stopIterating = false;

            public SnakesAndLaddersEnumerator(Func<GameState, GameState> iterator, GameState state)
            {
                this.StartState = state;
                this.Current = state;
                this.iterator = iterator;
            }

            public GameState Current { get; private set; }

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                // Nothing to dispose
            }

            public bool MoveNext()
            {
                var newState = this.iterator(this.Current);
                // Not strictly functional here, but as always with
                // this topic, a compromise is needed
                this.Current = newState;

                // Have we completed the final iteration?  That's done after
                // reaching the end condition
                if (stopIterating)
                    return false;

                var endConditionMet = this.Current.Players.Any(x => x.Position >= 100);
                var lastIteration = !this.stopIterating && endConditionMet;
                this.stopIterating = endConditionMet;
                return !this.stopIterating || lastIteration;
            }

            public void Reset()
            {
                // restore the initial state
                this.Current = this.StartState;
            }
        }

        public class SnakesAndLaddersIterator : IEnumerable<GameState>
        {
            private readonly GameState _startState;
            private readonly Func<GameState, GameState> _iterator;

            public SnakesAndLaddersIterator(GameState startState, Func<GameState, GameState> iterator)
            {
                this._startState = startState;
                this._iterator = iterator;
            }

            public IEnumerator<GameState> GetEnumerator() =>
                new SnakesAndLaddersEnumerator(this._iterator, this._startState);

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public record GameState
        {
            public IEnumerable<Player> Players { get; set; }
            public int CurrentPlayer { get; set; }
            public int NumberOfPlayers { get; set; }
        }

        private static GameState PlaySnakesAndLaddersRecursive(GameState state, IDieRoll die)
        {
            var roll = die.Roll();

            var newState = state with
            {
                CurrentPlayer = roll == 6
                    ? state.CurrentPlayer
                    : state.CurrentPlayer == state.NumberOfPlayers
                        ? 1
                        : state.CurrentPlayer + 1,
                Players = state.Players.Select(x => 
                        x.Number == state.CurrentPlayer
                            ? UpdatePlayer(x, roll)
                            : x
                    ).ToArray()
            };

            return newState.Players.Any(x => x.Position >= 100)
                ? newState
                : PlaySnakesAndLaddersRecursive(newState, die);
        }

        public int PlaySnakesAndLaddersRecursive(int noPlayers, IDieRoll die)
        {
            var state = new GameState
            {
                CurrentPlayer = 1,
                Players = Enumerable.Range(1, noPlayers)
                    .Select(x => (x, 1))
                    .Select(x => new Player
                    {
                        Number = x.Item1,
                        Position = x.Item2
                    }),
                NumberOfPlayers = noPlayers
            };

            var finalState = PlaySnakesAndLaddersRecursive(state, die);
            return finalState.Players.First(x => x.Position >= 100).Number;
        }

        public int PlaySnakesAndLaddersTrampolining(int noPlayers, IDieRoll die)
        {
            var state = new GameState
            {
                CurrentPlayer = 1,
                Players = Enumerable.Range(1, noPlayers)
                    .Select(x => (x, 1))
                    .Select(x => new Player
                    {
                        Number = x.Item1,
                        Position = x.Item2
                    }),
                NumberOfPlayers = noPlayers
            };

            var finalState = FunctionalExtensions.IterateUntil(state, x =>
            {
                var roll = die.Roll();

                var newState = state with
                {
                    CurrentPlayer = roll == 6
                        ? state.CurrentPlayer
                        : state.CurrentPlayer == state.NumberOfPlayers
                            ? 1
                            : state.CurrentPlayer + 1,
                    Players = state.Players.Select(x =>
                        x.Number == state.CurrentPlayer
                            ? UpdatePlayer(x, roll)
                            : x
                    ).ToArray()
                };

                return newState;
            }, x => x.Players.Any(y => y.Position >= 100));

            return finalState.Players.First(x => x.Position >= 100).Number;
        }

        private static Player UpdatePlayer(Player player, int roll)
        {
            var afterDieRoll = player with { Position = player.Position + roll };
            var afterSnakeOrLadder = afterDieRoll with
            {
                Position = SnakesAndLadders.ContainsKey(afterDieRoll.Position)
                    ? SnakesAndLadders[afterDieRoll.Position]
                    : afterDieRoll.Position
            };
            return afterSnakeOrLadder;
        }


        //private static Player UpdatePlayer(Player player, IDieRoll die)
        //{
        //    var currentRoll = die.Roll();
        //    var afterDieRoll = player with { Position = player.Position + currentRoll };
        //    var afterSnakeOrLadder = afterDieRoll with
        //    {
        //        Position = SnakesAndLadders.ContainsKey(afterDieRoll.Position)
        //            ? SnakesAndLadders[afterDieRoll.Position]
        //            : afterDieRoll.Position
        //    };
        //    return currentRoll == 6
        //        ? UpdatePlayer(afterSnakeOrLadder, die)
        //        : afterSnakeOrLadder;
        //}

        public int PlaySnakesAndLadders(int noPlayers, IDieRoll die)
        {
            var players = Enumerable.Range(1, noPlayers)
                .Select(x => new Player { Number = x, Position = 1 });

            var initialState = (
                Players: players,
                CurrentPlayer: 1
            );

            var finalState = FunctionalExtensions.IterateUntil(initialState, x => (
                x.Players.Select(y => y.Number == x.CurrentPlayer ? UpdatePlayer(y, 666) : y).ToArray(),
                (x.CurrentPlayer + 1) % (noPlayers + 1)
            ), x => x.Players.Any(y => y.Position >= 100));

            return finalState.Players.First(x => x.Position >= 100).Number;
        }


        public int PlaySnakesAndLaddersCustomIterator(int noPlayers, IDieRoll die)
        {
            var state = new GameState
            {
                CurrentPlayer = 1,
                Players = Enumerable.Range(1, noPlayers)
                    .Select(x => (x, 1))
                    .Select(x => new Player
                    {
                        Number = x.Item1,
                        Position = x.Item2
                    }),
                NumberOfPlayers = noPlayers
            };

            var update = (GameState g) =>
            {
                var roll = die.Roll();

                var newState = g with
                {
                    CurrentPlayer = roll == 6
                        ? g.CurrentPlayer
                        : g.CurrentPlayer == g.NumberOfPlayers
                            ? 1
                            : g.CurrentPlayer + 1,
                    Players = g.Players.Select(x =>
                        x.Number == g.CurrentPlayer
                            ? UpdatePlayer(x, roll)
                            : x
                    ).ToArray()
                };

                return newState;
            };

            var salIterator = new SnakesAndLaddersIterator(state, update);
            var stateAndMessages = (
                Messages: Enumerable.Empty<string>(),
                State: state
            );

            var result = salIterator.Aggregate(stateAndMessages, (agg, x) =>
            {
                return (
                    agg.Messages.Append("Player " + x.CurrentPlayer + "'s turn." +
                                        "The current winner is: " +
                                        x.Players.First(y => y.Position == x.Players.Max(z => z.Position)).Number),
                    x
                );
            });
            return result.State.Players.First(x => x.Position >= 100).Number;

        }


        [Fact]
        public void Test1()
        {
            var winner = PlaySnakesAndLadders(4, new DieRoll());
        }


        [Fact]
        public void Test2()
        {
            var winner = PlaySnakesAndLaddersImperative(4, new DieRoll());
        }

        [Fact]
        public void Test3()
        {
            var winner = PlaySnakesAndLaddersRecursive(4, new DieRoll());
        }

        [Fact]
        public void Test4()
        {
            var winner = PlaySnakesAndLaddersCustomIterator(4, new DieRoll());
        }
    }






}