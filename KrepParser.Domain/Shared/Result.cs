using KrepParser.Domain.Enums;
using System.Security;

namespace KrepParser.Domain.Shared
{
    public sealed record Error(ErrorTypes errorType, string description = "")
    {
        public static readonly Error Ok = new(ErrorTypes.Ok);
    }

    public sealed record ParseError(ParseErrors errorType, string description = "")
    {
        public static readonly ParseError Ok = new(ParseErrors.Ok);
    }

    public abstract class BaseResult
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
    }

    public class Result : BaseResult
    {
        private Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.Ok
                || !isSuccess && error == Error.Ok)
            {
                throw new ArgumentException("Ошибка недопустима", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public Error? Error { get; private set; }

        public static Result SuccessWithoutValue() => new(true, Error.Ok);
        public static Result FailureWithotValue(Error error) => new(false, error);
    }

    public class Result<T> : BaseResult
    {
        private Result(T? value, bool isSuccess, Error error)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"Объект {value} не может быть равен null");
            }

            if (isSuccess && error != Error.Ok
                || !isSuccess && error == Error.Ok)
            {
                throw new ArgumentException("Ошибка недопустима", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            TValue = value;
        }

        public Error? Error { get; private set; }
        public T? TValue { get; private set; }

        public static Result<T> SuccessWithoutValue() => new(default!, true, Error.Ok);
        public static Result<T> FailureWithotValue(Error error) => new(default!, false, error);

        public static Result<T> Succes(T value) => new(value, true, Error.Ok);
        public static Result<T> Failure(T value, Error error) => new(value, false, error);
    }

    public class ParseResult<T> : BaseResult
    {
        private ParseResult(T? value, bool isSuccess, ParseError error)
        {
            if (value == null)
            {
                throw new ArgumentNullException($"Объект {value} не может быть равен null");
            }

            if (isSuccess && error != ParseError.Ok
                || !isSuccess && error == ParseError.Ok)
            {
                throw new ArgumentException("Ошибка недопустима", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            TValue = value;
        }

        public ParseError? Error { get; private set; }
        public T? TValue { get; private set; }

        public static ParseResult<T> SuccessWithoutValue() => new(default!, true, ParseError.Ok);
        public static ParseResult<T> FailureWithotValue(ParseError error) => new(default!, false, error);

        public static ParseResult<T> Succes(T value) => new(value, true, ParseError.Ok);
        public static ParseResult<T> Failure(T value, ParseError error) => new(value, false, error);
    }
}
