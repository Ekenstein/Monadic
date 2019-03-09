using System;
using System.Collections.Generic;
using Monadicsh.Extensions;

namespace Monadicsh
{
    /// <inheritdoc />
    /// <summary>
    /// A type representing either a <see cref="T:Monadicsh.Result" />
    /// or a right value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the successful value.</typeparam>
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
        public IReadOnlyList<Error> Errors => this.MapLeft(new Error[0], result => result.Errors);

        /// <summary>
        /// The item of a successful validation result, or <see cref="Maybe{T}.Nothing"/> if the
        /// validation failed.
        /// </summary>
        public Maybe<T> Item => this.RightOrNothing();

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
        /// Creates a successful validation result containing the given <paramref name="item"/>.
        /// </summary>
        /// <param name="item">The result of the validation.</param>
        /// <returns>A <see cref="Result{T}"/> representing a successful validation result.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="item"/> is null.</exception>
        public static Result<T> Success(T item) => new Result<T>(item);

        /// <summary>
        /// Creates an unsuccessful <see cref="Result{T}"/> which will contain
        /// the given <paramref name="error"/>.
        /// </summary>
        /// <param name="error">The error describing why the result was unsuccessful.</param>
        public static implicit operator Result<T>(Error error) => Failed(error);

        /// <summary>
        /// Returns a <see cref="Result"/> of the given <paramref name="result"/>.
        /// If the given <paramref name="result"/> is indicating of unsuccessful result,
        /// an unsuccessful <see cref="Result"/> will be returned containing the errors of the given <paramref name="result"/>.
        /// Otherwise <see cref="Result.Success"/> will be returned.
        /// </summary>
        /// <param name="result">The result to create a <see cref="Result"/> of.</param>
        public static implicit operator Result(Result<T> result) => result.LeftOrDefault(Result.Success);

        /// <summary>
        /// Returns a <see cref="Maybe{T}"/> representation of the given <paramref name="result"/>.
        /// If the given <paramref name="result"/> is indicating of a unsuccessful result, <see cref="Maybe{T}.Nothing"/>
        /// will be returned, otherwise <see cref="Maybe{T}.Just"/> of the item the result is holding.
        /// </summary>
        /// <param name="result">The result to create a <see cref="Maybe{T}"/> of.</param>
        public static implicit operator Maybe<T>(Result<T> result) => result.Item;

        /// <inheritdoc />
        /// <summary>
        /// Returns a string representation of the current instance of <see cref="T:Monadicsh.Result`1" />.
        /// </summary>
        /// <returns>The string representation of the current instance of <see cref="T:Monadicsh.Result`1" />.</returns>
        public override string ToString() => this.MapEither(
            fromL: l => l.ToString(),
            fromR: r => $"Success: ({r})");
    }
}