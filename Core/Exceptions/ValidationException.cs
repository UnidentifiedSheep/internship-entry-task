namespace Core.Exceptions;

public class ValidationException : BaseValuedException
{
    public ValidationException(string message) : base(message) { }
    
    public ValidationException(string message, object relatedData) : base(message, relatedData) { }
}