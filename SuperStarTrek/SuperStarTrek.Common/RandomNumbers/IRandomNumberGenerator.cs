namespace ConsoleGame.Common.RandomNumbers
{
    public interface IRandomNumberGenerator
    {
        int BetweenZeroAnd(int input);
        int Between(int a, int b);
    }
}
