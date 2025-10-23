namespace Pariqas;

public sealed class OperationResult
{
    private OperationResult(bool succeeded, string message)
    {
        Succeeded = succeeded;
        Message = message;
    }
    
    public bool Succeeded { get; private init; }
    public string Message { get; private init; }

    public static OperationResult Success() => new OperationResult(true, string.Empty);

    public static OperationResult Fail(string message) => new OperationResult(false, message);
}