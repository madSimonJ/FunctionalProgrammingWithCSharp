using MartianTrail.Common;

namespace MartianTrail.PlayerInteraction
{
    public class ConsoleShim : IConsole
    {
        public Operation WriteLine(params string[] message) =>
            message.TryOperation(x => {
                foreach (var m in message)
                {
                    Console.WriteLine(m ?? string.Empty);
                }
            });

        public void WriteLine(IEnumerable<string> message)
        {
            foreach (var m in message)
            {
                Console.WriteLine(m);
            }
        }

        public Maybe<string> ReadLine()
        {
            try
            {
                var result = Console.ReadLine();

                return string.IsNullOrWhiteSpace(result)
                    ? new Nothing<string>()
                    : new Something<string>(result);
            }
            catch (Exception e)
            {
                return new Error<string>(e);
            }
        }
    }
}
