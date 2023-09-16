namespace MartianTrail.Common
{
    public abstract class Operation
    {

    }

    public class Success : Operation
    {

    }

    public class Failure : Operation
    {
        public Failure(Exception e)
        {
            CapturedException = e;
        }

        public Exception CapturedException { get; set; }
    }

    public static class SuccessOrFailureExtensions
    {
        public static Operation TryOperation<T>(this T @this, Action<T> action)
        {
            try
            {
                action(@this);
                return new Success();
            }
            catch (Exception e)
            {
                return new Failure(e);
            }
        }
    }
}
