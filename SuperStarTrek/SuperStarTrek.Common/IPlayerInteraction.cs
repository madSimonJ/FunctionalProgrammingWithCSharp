namespace ConsoleGame.Common
{
    public interface IPlayerInteraction
    {
        PlayerInput GetInput(params string[] prompt);
        Operation WriteMessage(params string[] prompt);
        Operation WriteBlankLines(int numberOfLines);
        Operation WriteMessageConditional(bool condition, params string[] promtp);
    }
}
