namespace Domain.Models;

public class Result
{
    public bool IsSuccess { get; }
    public ErrorCode? ErrorCode { get; }
    public IReadOnlyList<string> Messages { get; }

    protected Result(bool isSuccess, ErrorCode? errorCode, IReadOnlyList<string> messages)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        Messages = messages;
    }

    public static Result Success() =>
        new(true, null, Array.Empty<string>());

    public static Result Failure(ErrorCode errorCode, string error) =>
        new(false, errorCode, new[] { error });

    public static Result Failure(ErrorCode errorCode, IEnumerable<string> errors) =>
        new(false, errorCode, errors.ToList());
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected Result(
        T? value,
        bool isSuccess,
        ErrorCode? errorCode,
        IReadOnlyList<string> messages
    ) : base(isSuccess, errorCode, messages)
    {
        Value = value;
    }

    public static Result<T> Success(T value) =>
        new(value, true, null, Array.Empty<string>());

    public static new Result<T> Failure(ErrorCode errorCode, string error) =>
        new(default, false, errorCode, new[] { error });

    public static new Result<T> Failure(ErrorCode errorCode, IEnumerable<string> errors) =>
        new(default, false, errorCode, errors.ToList());
}
