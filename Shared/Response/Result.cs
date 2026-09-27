using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Response
{
    public enum ErrorType
    {
        Failure,
        Validation,
        NotFound,
        Conflict,
        Unauthorized,
        Forbidden,
        None,
        BadRequest
    }
    public class Result
    {
        protected Result(bool isSuccess, IReadOnlyList<Error> error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Error { get; }

        public static Result Success() => new(true, Array.Empty<Error>());

        public static Result Failure(Error error) => new(false, new[] { error });

        public static Result Failure(IEnumerable<Error> errors) => new(false, errors.ToList());
    }
}
