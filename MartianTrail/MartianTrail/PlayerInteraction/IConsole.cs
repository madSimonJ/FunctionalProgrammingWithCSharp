using MartianTrail.Common;

namespace MartianTrail.PlayerInteraction
{
    public interface IConsole
    {
        Operation WriteLine(params string[] message);
        Maybe<string> ReadLine();
    }
}
