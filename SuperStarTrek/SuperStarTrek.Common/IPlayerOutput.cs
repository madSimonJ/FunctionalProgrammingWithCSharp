namespace ConsoleGame.Common
{
    public interface IPlayerOutput
    {
        Operation WriteLine(params string[] message);
        Maybe<string> ReadLine();
    }
}
