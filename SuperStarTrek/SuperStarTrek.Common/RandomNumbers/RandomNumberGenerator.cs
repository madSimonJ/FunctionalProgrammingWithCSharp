namespace ConsoleGame.Common.RandomNumbers
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        public int BetweenZeroAnd(int input) =>
            new Random().Next(0, input);

        public int Between(int a, int b) =>
            new Random().Next(a, b);
    }
}
