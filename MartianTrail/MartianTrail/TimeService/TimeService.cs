namespace MartianTrail.TimeService
{
    public class TimeService : ITimeService
    {
        public DateTime Now() => DateTime.Now;
    }
}
