using System;

namespace Monadic.Extensions
{
    public static class ValidationResultExtensions
    {
        public static T OrThrow<T>(this ValidationResult<T> result, Func<Result, Exception> exception) => result
            .RightOrThrow(exception);
    }
}
