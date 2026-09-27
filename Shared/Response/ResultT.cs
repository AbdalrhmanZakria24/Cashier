using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Response
{
    public class ResultT<T> :Result
    {
        private readonly T? _value;

        protected ResultT(T value) : base(true, Array.Empty<Error>())
        {
            _value = value;
        }

        protected ResultT(IReadOnlyList<Error> error) : base(false, error)
        {
            _value = default;
        }
        protected ResultT(Error error) : base(false, new[] { error })
        {
            _value = default;
        }
        public T Value
           => IsSuccess ? _value! : throw new InvalidOperationException();

        public static ResultT<T> Success(T value) => new(value);

        public new static ResultT<T> Failure(Error error) => new(error);

        public static ResultT<T> Failures(IEnumerable<Error> errors) => new(errors.ToList());
    }
}
