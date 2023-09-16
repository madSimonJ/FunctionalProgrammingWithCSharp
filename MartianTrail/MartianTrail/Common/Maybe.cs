namespace MartianTrail.Common
{
    public abstract class Maybe<T>
    {
        public virtual T Value { get; protected set; }

        public static implicit operator Maybe<T>(T @this) => @this.ToMaybe();
        public static implicit operator T(Maybe<T> @this) => @this.Value;
    }

    public class Error<T> : Maybe<T>
    {
        public Error(Exception e)
        {
            ErrorMessage = e;
        }
        public Exception ErrorMessage { get; set; }
        public override T Value => default;

    }

    public class Something<T> : Maybe<T>
    {
        public Something(T value)
        {
            this.Value = value;
        }

        public static implicit operator Something<T>(T @this) => new Something<T>(@this);
        public static implicit operator T(Something<T> @this) => @this.Value;
    }

    public class Nothing<T> : Maybe<T>
    {
        public override T Value => default;
    }

    public abstract class Maybe
    {

    }

    public class Something : Maybe
    {

    }

    public class Nothing : Maybe
    {

    }

    public class Error : Maybe
    {
        public Error(Exception e)
        {
            CapturedError = e;
        }

        public Exception CapturedError { get; set; }
    }


    public static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T @this) => new Something<T>(@this);

        public static Maybe<TToType> Map<TFromType, TToType>(this Maybe<TFromType> @this, Func<TFromType, Maybe<TToType>> f)
        {
            switch (@this)
            {
                case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                    try
                    {
                        return f(sth);
                    }
                    catch (Exception e)
                    {
                        return new Error<TToType>(e);
                    }
                case Error<TFromType> err:
                    return new Error<TToType>(err.ErrorMessage);
                default:
                    return new Nothing<TToType>();
            }
        }


        public static Maybe Map<TFromType>(this Maybe<TFromType> @this, Func<TFromType, Maybe> f)
        {
            switch (@this)
            {
                case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                    try
                    {
                        return f(sth);
                    }
                    catch (Exception e)
                    {
                        return new Error(e);
                    }
                case Error<TFromType> err:
                    return new Error(err.ErrorMessage);
                default:
                    return new Nothing();
            }
        }

        public static Maybe<TToType> Bind<TFromType, TToType>(this Maybe<TFromType> @this, Func<TFromType, TToType> f)
        {
            switch (@this)
            {
                case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                    try
                    {
                        return f(sth).ToMaybe();
                    }
                    catch (Exception e)
                    {
                        return new Error<TToType>(e);
                    }
                case Error<TFromType> err:
                    return new Error<TToType>(err.ErrorMessage);
                default:
                    return new Nothing<TToType>();
            }
        }

        public static async Task<Maybe<TToType>> MapAsync<TFromType, TToType>(this Maybe<TFromType> @this, Func<TFromType, Task<Maybe<TToType>>> f)
        {
            switch (@this)
            {
                case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                    try
                    {
                        var result = await f(sth.Value);
                        return result.ToMaybe();
                    }
                    catch (Exception e)
                    {
                        return new Error<TToType>(e);
                    }
                case Error<TFromType> err:
                    return new Error<TToType>(err.ErrorMessage);
                default:
                    return new Nothing<TToType>();
            }
        }

        public static async Task<Maybe<TToType>> BindAsync<TFromType, TToType>(this Maybe<TFromType> @this, Func<TFromType, Task<TToType>> f)
        {
            switch (@this)
            {
                case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                    try
                    {
                        var result = await f(sth.Value);
                        return result.ToMaybe();
                    }
                    catch (Exception e)
                    {
                        return new Error<TToType>(e);
                    }
                case Error<TFromType> err:
                    return new Error<TToType>(err.ErrorMessage);
                default:
                    return new Nothing<TToType>();
            }
        }

        public static async Task<Maybe<TToType>> BindAsync<TFromType, TToType>(this Maybe<TFromType> @this, Func<TFromType, ValueTask<TToType>> f)
        {
            switch (@this)
            {
                case Something<TFromType> sth when !EqualityComparer<TFromType>.Default.Equals(sth.Value, default):
                    try
                    {
                        var result = await f(sth.Value);
                        return result.ToMaybe();
                    }
                    catch (Exception e)
                    {
                        return new Error<TToType>(e);
                    }
                case Error<TFromType> err:
                    return new Error<TToType>(err.ErrorMessage);
                default:
                    return new Nothing<TToType>();
            }
        }

        public static Maybe<T> OnNothing<T>(this Maybe<T> @this, Action act)
        {
            try
            {
                if (@this is Nothing<T> nth)
                    act();
                return @this;
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static Maybe<T> OnError<T>(this Maybe<T> @this, Action<Exception> act)
        {
            try
            {
                if (@this is Error<T> err)
                    act(err.ErrorMessage);
                return @this;
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }

        }

        public static Maybe<T> OnSomething<T>(this Maybe<T> @this, Action<T> act)
        {
            try
            {
                if (@this is Something<T> err)
                    act(err.Value);
                return @this;
            }
            catch (Exception e)
            {
                return new Error<T>(e);
            }
        }

        public static Maybe Map<T>(this Maybe<T> @this, Action<T> act)
        {
            switch (@this)
            {
                case Something<T> sth when !EqualityComparer<T>.Default.Equals(sth.Value, default):
                    try
                    {
                        act(sth);
                        return new Something();
                    }
                    catch (Exception e)
                    {
                        return new Error(e);
                    }
                case Error<T> err:
                    return new Error(err.ErrorMessage);
                default:
                    return new Something();
            }
        }

        public static Maybe OnError(this Maybe @this, Action<Exception> act)
        {
            try
            {
                if (@this is Error err)
                    act(err.CapturedError);
                return @this;
            }
            catch (Exception e)
            {
                return new Error(e);
            }

        }

        public static Maybe OnSomething(this Maybe @this, Action act)
        {
            try
            {
                if (@this is Something sth)
                    act();
                return @this;
            }
            catch (Exception e)
            {
                return new Error(e);
            }

        }

        public static Maybe OnNothing(this Maybe @this, Action act)
        {
            try
            {
                if (@this is Nothing nth)
                    act();
                return @this;
            }
            catch (Exception e)
            {
                return new Error(e);
            }

        }
    }

}
