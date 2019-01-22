using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Extensions
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
        /// outer <see cref="Result"/>. If one or both of the results' are
        /// unsuccessful, an unsuccessful result will be returned
        /// containing unique errors of both the results, otherwise
        /// a successful result will be returned.
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
                .Union(result.Errors)
                .ToArray();

            return Result.Failed(errors);
        }

        /// <summary>
        /// Ors together the inner <see cref="Result"/> with the
        /// outer <see cref="Result"/>. If one of the results are
        /// successful, a successful result will be returned, otherwise
        /// an unsuccessful result will be returned containing unique errors of both
        /// results.
        /// </summary>
        /// <param name="self">The inner result.</param>
        /// <param name="outer">The outer result to or with the inner result.</param>
        /// <returns>
        /// A successful result if either the inner or outer result is successful, otherwise
        /// an unsuccessful result containing errors from both results.
        /// </returns>
        public static Result Or(this Result self, Result outer)
        {
            if (self.Succeeded || outer.Succeeded)
            {
                return Result.Success;
            }

            var errors = self
                .Errors
                .Union(outer.Errors)
                .ToArray();

            return Result.Failed(errors);
        }

        /// <summary>
        /// Returns the value of the given <paramref name="result"/> iff the result is representing
        /// a successful result, otherwise the exception produced by the given <paramref name="exception"/> will be thrown.
        /// </summary>
        /// <typeparam name="T">The type the result is encapsulating.</typeparam>
        /// <param name="result">The result to extract the value from.</param>
        /// <param name="exception">The lambda producing the exception if the result was unsuccessful.</param>
        /// <returns>
        /// The value of the given <paramref name="result"/> iff the result is representing a successful result.
        /// Otherwise the exception produced by the given <paramref name="exception"/> will be thrown.
        /// </returns>
        public static T OrThrow<T>(this Result<T> result, Func<IEnumerable<Error>, Exception> exception) => result
            .RightOrThrow(l => exception(l.Errors));
    }
}
