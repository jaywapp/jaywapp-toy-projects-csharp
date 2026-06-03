namespace AIInstaller.Core.Models;

public sealed class OperationResult
{
    public required bool IsSuccess { get; init; }

    public required string Message { get; init; }

    public static OperationResult Success(string message)
    {
        return new OperationResult { IsSuccess = true, Message = message };
    }

    public static OperationResult Failure(string message)
    {
        return new OperationResult { IsSuccess = false, Message = message };
    }
}
