namespace ConsoleGame.Common
{
    public class PlayerInteractionClient : IPlayerInteraction
    {
        private readonly IPlayerOutput _playerOutput;

        public PlayerInteractionClient(IPlayerOutput playerOutput)
        {
            this._playerOutput = playerOutput;
        }

        public PlayerInput GetInput(params string[] prompt)
        {
            var writeResult = _playerOutput.WriteLine(prompt);

            var readResult = writeResult switch
            {
                Failure f => new Error<string>(f.CapturedException),
                _ => this._playerOutput.ReadLine()
            };

            PlayerInput returnValue = readResult switch
            {
                Something<string> sthInt when int.TryParse(sthInt.Value, out _) => new IntegerInput(int.Parse(sthInt.Value)),
                Something<string> sthStr when !string.IsNullOrWhiteSpace(sthStr.Value) => new TextInput(sthStr.Value),
                Error<string> sthErr => new UserInputError(sthErr.ErrorMessage),
                _ => new EmptyInput(),
            };

            return returnValue;
        }

        public Operation WriteMessage(params string[] prompt) =>
            _playerOutput.WriteLine(prompt);

        public Operation WriteBlankLines(int numberOfLines)
        {
            var commands = Enumerable.Repeat(() => this._playerOutput.WriteLine(""), numberOfLines);
            return commands.Iterate();
        }

        public Operation WriteMessageConditional(bool condition, params string[] prompt) =>
            condition
                ? WriteMessage(prompt)
                : new Success();
    }

}
