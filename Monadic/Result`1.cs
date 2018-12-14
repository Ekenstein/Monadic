using System.Collections.Generic;
using Monadic.Extensions;

namespace Monadic
{
    /// <summary>
    /// A type representing either a <see cref="Result"/>
    /// or a right value of type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the right value.</typeparam>
    public class Result<T> : Either<Result, T>
    {
        /// <summary>
        /// True if the validation was successful, otherwise false.
        /// </summary>
        public bool Succeeded => IsRight;

        /// <summary>
        /// A list of errors from the validation or an empty list if there were
        /// no validation errors.
        /// </summary>
        public IReadOnlyList<Error> Errors => this.FromLeft(new Error[0], result => result.Errors);

        /// <summary>
        /// The item of a successful validation result, or <see cref="Maybe{T}.Nothing"/> if the
        /// validation failed.
        /// </summary>
        public Maybe<T> Item => this.MaybeRight();

        private Result(Result left) : base(left)
        {
        }

        private Result(T right) : base(right)
        {
        }

        /// <summary>
        /// Creates a failed validation result containing the given <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">The errors that occurred during the validation.</param>
        /// <returns>A <see cref="Result{T}"/> representing a failed validation.</returns>
        public static Result<T> Failed(params Error[] errors) => new Result<T>(Result.Failed(errors));

        /// <summary>
        /// Creates a successful validation result containing the given <paramref name="result"/>.
        /// </summary>
        /// <param name="result">The result of the validation.</param>
        /// <returns>A <see cref="Result{T}"/> representing a successful validation result.</returns>
        public static Result<T> Success(T result) => new Result<T>(result);

        public static implicit operator Result<T>(Error error) => Failed(error);

        public static implicit operator Result<T>(T t) => Success(t);

        public static implicit operator Result(Result<T> result) => result.FromLeft(Result.Success);

        public static implicit operator Maybe<T>(Result<T> result) => result.Item;

        public override string ToString() => this.FromEither(
            fromL: l => l.ToString(),
            fromR: r => $"Success: ({r})");
    }
}