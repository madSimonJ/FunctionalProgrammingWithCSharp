namespace ConsoleGame.Common
{
    public interface IGame<TGs> where TGs : GameState
    {
        Operation Introduction();
        TGs InitialGameState();
        TGs GameTurn(TGs oldState);
        Operation EndGame();
    }
}
