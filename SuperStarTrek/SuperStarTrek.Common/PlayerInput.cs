namespace ConsoleGame.Common;

public abstract class PlayerInput
{
}

public class EmptyInput : PlayerInput
{

}

public class TextInput : PlayerInput
{
    public TextInput(string textFromUser)
    {
        TextFromUser = textFromUser;
    }

    public string TextFromUser { get; set; }
}

public class IntegerInput : PlayerInput
{
    public IntegerInput(int integerFromUser)
    {
        IntegerFromUser = integerFromUser;
    }

    public int IntegerFromUser { get; set; }
}

public class UserInputError : PlayerInput
{
    public UserInputError(Exception exceptionRaised)
    {
        ExceptionRaised = exceptionRaised;
    }

    public Exception ExceptionRaised { get; set; }
}