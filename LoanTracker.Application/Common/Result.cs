namespace LoanTracker.Application.Common;

/// <summary>
/// Represents the result of an operation that can either succeed or fail
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public Error? Error { get; }

    private Result(bool isSuccess, T? value, Error? error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
    }

    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(Error error) => new(false, default, error);

    /// <summary>
    /// Executes an action based on success or failure
    /// </summary>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }
    public static implicit operator Result<T>(T value)  => Success(value);
    public static implicit operator Result<T>(Error error)  => Failure(error);
}

/// <summary>
/// Represents an error with a code, message, and type
/// </summary>
public record Error(string Code, string Message, ErrorType Type)
{
    public static Error Validation(string message, string code = "VALIDATION_ERROR") =>
        new(code, message, ErrorType.Validation);

    public static Error NotFound(string message, string code = "NOT_FOUND") =>
        new(code, message, ErrorType.NotFound);

    public static Error Conflict(string message, string code = "CONFLICT") =>
        new(code, message, ErrorType.Conflict);

    public static Error ServerError(string message, string code = "SERVER_ERROR") =>
        new(code, message, ErrorType.ServerError);
}

/// <summary>
/// Types of errors that can occur in the application
/// </summary>
public enum ErrorType
{
    Validation,
    NotFound,
    Conflict,
    ServerError
}
