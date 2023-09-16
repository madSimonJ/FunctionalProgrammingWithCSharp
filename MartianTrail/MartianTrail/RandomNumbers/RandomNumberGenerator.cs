namespace MartianTrail.RandomNumbers
{
    public class RandomNumberGenerator : IRandomNumberGenerator
    {
        public int BetweenZeroAnd(int input) =>
            new Random().Next(0, input);
    }
}
