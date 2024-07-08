using Chat.BL.Abstractions.Errors;

namespace Chat.BL.Abstractions.Results;

public sealed class Result<TValue> : Result
{
    private Result(bool isSuccess, Error? error = default, TValue? value = default)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public TValue? Value { get; }

    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(true, value: value);
    }

    public new static Result<TValue> Failure(Error error)
    {
        return new Result<TValue>(false, error);
    }
}