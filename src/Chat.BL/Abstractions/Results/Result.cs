using Chat.BL.Abstractions.Errors;

namespace Chat.BL.Abstractions.Results;

public class Result
{
    protected Result(bool isSuccess, Error? error = default)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public Error? Error { get; }

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
}