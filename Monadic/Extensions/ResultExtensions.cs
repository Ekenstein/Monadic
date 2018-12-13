using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadic.Extensions
{
    public static class ResultExtensions
    {
        /// <summary>
        /// Produces and throws the given exception iff the result was unsuccessful.
        /// </summary>
        /// <param name="result">The result to base the exception of.</param>
        /// <param name="ex">The func that produces the exception.</param>
        public static void ThrowIfUnsuccessful(this Result result, Func<IEnumerable<Error>, Exception> ex)
        {
            if (!result.Succeeded)
            {
                throw ex(result.Errors);
            }
        }

        /// <summary>
        /// Ands together the inner <see cref="Result"/> with the
        /// outer <see cref="Result"/>. If one of the results' are
        /// unsuccessful, an unsuccessful result will be returned
        /// containing the errors of both the results.
        /// </summary>
        /// <param name="self">The inner result.</param>
        /// <param name="result">The outer result to and with the inner.</param>
        /// <returns>
        /// Either a successful result if both results were successful, or an unsuccessful result
        /// if either the inner or outer result was unsuccessful.
        /// </returns>
        public static Result And(this Result self, Result result)
        {
            if (self.Succeeded && result.Succeeded)
            {
                return Result.Success;
            }

            var errors = self
                .Errors
                .Concat(result.Errors)
                .ToArray();

            return Result.Failed(errors);
        }

        public static Result Or(this Result self, Result outer)
        {
            if (self.Succeeded || outer.Succeeded)
            {
                return Result.Success;
            }

            var errors = self
                .Errors
                .Concat(outer.Errors)
                .ToArray();

            return Result.Failed(errors);
        }

        public static T OrThrow<T>(this Result<T> result, Func<Result, Exception> exception) => result
            .RightOrThrow(exception);
    }
}
