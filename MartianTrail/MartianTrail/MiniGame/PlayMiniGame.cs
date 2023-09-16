using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MartianTrail.PlayerInteraction;
using MartianTrail.RandomNumbers;
using MartianTrail.TimeService;

namespace MartianTrail.MiniGame
{
    public class PlayMiniGame : IPlayMiniGame
    {
        private readonly IRandomNumberGenerator _rnd;
        private readonly IPlayerInteraction _playerInteraction;
        private readonly ITimeService _timeService;

        public PlayMiniGame(IRandomNumberGenerator rnd, IPlayerInteraction playerInteraction, ITimeService timeService)
        {
            _rnd = rnd;
            _playerInteraction = playerInteraction;
            _timeService = timeService;
        }

        private static decimal RateAccuracy(string expected, string actual)
        {
            var charByCharComparison = expected.Zip(actual,
                (x, y) => char.ToUpper(x) == char.ToUpper(y));
            var numberCorrect = charByCharComparison.Sum(x => x ? 1 : 0);
            var accuracyScore = (decimal)numberCorrect / 4;
            return accuracyScore;
        }

        public decimal PlayMiniGameForSuccessFactor()
        {
            // I don't care what the user enters, I'm just getting them ready to
            // play.
            _ = this._playerInteraction.GetInput("Get ready, the mini-game is about to begin.",
                "Press enter to begin....");

            var lettersToSelect = Enumerable.Repeat(0, 4)
                .Select(_ => this._rnd.BetweenZeroAnd(25))
                .Select(x => (char)('A' + x))
                .ToArray();

            var textToSelect = string.Join("", lettersToSelect);

            var timeStart = this._timeService.Now();

            var userAttempt = this._playerInteraction.GetInput(
                "Please enter the following as accurately as you can: " +
                textToSelect);
            
            var nonErrorInput = userAttempt is not UserInputError
                ? userAttempt
                : userAttempt.IterateUntil(
                x => this._playerInteraction.GetInput(
                    "Please enter the following as accurately as you can: " +
                    textToSelect),
                x => x is not UserInputError
            );

            var timeEnd = this._timeService.Now();

            var textAccuracy =
                nonErrorInput is TextInput { TextFromUser.Length: 4 } ti
                    ? RateAccuracy(textToSelect, ti.TextFromUser)
                    : 0M;

            var timeTaken = (timeEnd - timeStart).TotalSeconds;
            var timeAccuracy = 1M * (decimal)Math.Pow(0.99, timeTaken);

            return textAccuracy * timeAccuracy;
        }
    }
}
