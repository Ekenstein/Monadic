using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Extensions
{
    /// <summary>
    /// Provides a set of static extensions for the types <see cref="Result"/> and <see cref="Result{T}"/>.
    /// </summary>
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
        /// <param name="inner">The inner result.</param>
        /// <param name="outer">The outer result to and with the inner.</param>
        /// <returns>
        /// Either a successful result if both results were successful, or an unsuccessful result
        /// if either the inner or outer result was unsuccessful.
        /// </returns>
        public static Result And(this Result inner, Result outer)
        {
            if (inner.Succeeded && outer.Succeeded)
            {
                return Result.Success;
            }

            var errors = inner
                .Errors
                .Union(outer.Errors)
                .ToArray();

            return Result.Failed(errors);
        }

        /// <summary>
        /// Returns a result indicating whether the given <paramref name="inner"/> or the
        /// <paramref name="outer"/> result was successful.
        /// If none of the results are successful, <see cref="Result.Failed"/> will be returned
        /// containing errors from both of the results.
        /// </summary>
        /// <param name="inner">The inner result.</param>
        /// <param name="outer">The outer result to or with the inner result.</param>
        /// <returns>
        /// A successful result if either the inner or outer result was successful, otherwise
        /// an unsuccessful result containing errors from both results.
        /// </returns>
        public static Result Or(this Result inner, Result outer)
        {
            return inner.Or(() => outer);
        }

        /// <summary>
        /// Returns a result indicating whether the given <paramref name="inner"/> or the result
        /// produced by the given <paramref name="outerSelector"/> was successful.
        /// If none of the results are successful, <see cref="Result.Failed"/> will be returned
        /// containing errors from both of the results.
        /// </summary>
        /// <param name="inner">The inner result that may or may not be successful.</param>
        /// <param name="outerSelector">The function producing the outer result that may or may not be successful.</param>
        /// <returns>
        /// A <see cref="Result"/> where a successful result means that one or both results were successful.
        /// If both results were failed, a failed result will be returned containing both of the results' errors.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="outerSelector"/> is null.</exception>
        public static Result Or(this Result inner, Func<Result> outerSelector)
        {
            if (outerSelector == null)
            {
                throw new ArgumentNullException(nameof(outerSelector));
            }

            if (inner.Succeeded)
            {
                return Result.Success;
            }

            var outerResult = outerSelector();
            return outerResult.Succeeded 
                ? Result.Success 
                : Result.Failed(outerResult.Errors.Union(inner.Errors).ToArray());
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
            .GetRightOrThrow(l => exception(l.Errors));
    }
}
