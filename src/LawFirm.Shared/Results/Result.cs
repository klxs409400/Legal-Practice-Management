namespace LawFirm.Shared.Results;

public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    // Non-generic Success/Failure: for operations with nothing to hand back on success
    // (e.g. ArchiveAsync/RestoreAsync) - just IsSuccess/Error, no Value.
    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);

    // Generic Success<T>/Failure<T>: for operations that DO return data on success
    // (e.g. CreateAsync). These live here on Result but their return type is Result<T>,
    // so `new(...)` below resolves to Result<T>'s own 3-arg constructor, not Result's
    // 2-arg one - two different constructors on two different classes.
    // Success<T> is its own generic method (separate from the class's <T>) - T is
    // inferred from whatever value you pass in, e.g. Result.Success(dto) infers T=ClientDetailDto.
    public static Result<T> Success<T>(T value) => new(value, true, null);
    // default = the default value for whatever T is (null for reference types) -
    // used because a failure has no real value to store.
    public static Result<T> Failure<T>(Error error) => new(default, false, error);
}

public class Result<T> : Result
{
    // internal: only code inside this same project (LawFirm.Shared) can call this
    // constructor directly - forces every other project to go through the
    // Success()/Failure() factory methods above instead of `new Result<T>(...)`,
    // so you can never construct a Result that's both "success" and carrying an error.
    //
    // isSuccess/error aren't assigned in this constructor's body because IsSuccess/Error
    // are get-only properties declared on the base Result class - only Result's own
    // constructor can set them, so we hand isSuccess/error up via `: base(...)` and only
    // assign Value here, since Value is the one property that belongs to Result<T> itself.
    internal Result(T? value, bool isSuccess, Error? error) : base(isSuccess, error)
    {
        Value = value;
    }

    public T? Value { get; }

    // Implicit conversions: let a method declared to return Result<T> just
    // `return someDto;` or `return someError;` directly - the compiler sees the
    // type mismatch, finds these operators, and silently inserts the conversion.
    // `return client.ToDetail();` (a plain ClientDetailDto) actually triggers a
    // 3-step chain: this operator -> Success<T> above -> Result<T>'s constructor -
    // ending up as new Result<ClientDetailDto>(dto, true, null). The method
    // definitions above don't change; only the argument passed in (dto) does.
    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure<T>(error);
}
