using MartianTrail.Common;

namespace MartianTrail.PlayerInteraction
{
    public class PlayerInteractionClient : IPlayerInteraction
    {
        private readonly IConsole console;

        public PlayerInteractionClient(IConsole console)
        {
            this.console = console;
        }

        public PlayerInput GetInput(params string[] prompt)
        {
            var writeResult = console.WriteLine(prompt);

            var readResult = writeResult switch
            {
                Failure f => new Error<string>(f.CapturedException),
                _ => this.console.ReadLine()
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
            console.WriteLine(prompt);

        public Operation WriteMessageConditional(bool condition, params string[] prompt) =>
            condition
                ? WriteMessage(prompt)
                : new Success();
    }
}
