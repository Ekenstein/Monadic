using System.Collections.Generic;
using Monadic.Extensions;

namespace Monadic
{
    /// <summary>
    /// A type representing either a <see cref="Result"/>
    /// or a right value of type <see cref="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the right value.</typeparam>
    public class ValidationResult<T> : Either<Result, T>
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
        public Maybe<T> Item => this.MaybeRight<Result, T>();

        private ValidationResult(Result left) : base(left)
        {
        }

        private ValidationResult(T right) : base(right)
        {
        }

        /// <summary>
        /// Creates a failed validation result containing the given <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">The errors that occurred during the validation.</param>
        /// <returns>A <see cref="ValidationResult{T}"/> representing a failed validation.</returns>
        public static ValidationResult<T> Failed(params Error[] errors) => new ValidationResult<T>(Result.Failed(errors));

        /// <summary>
        /// Creates a successful validation result containing the given <paramref name="result"/>.
        /// </summary>
        /// <param name="result">The result of the validation.</param>
        /// <returns>A <see cref="ValidationResult{T}"/> representing a successful validation result.</returns>
        public static ValidationResult<T> Success(T result) => new ValidationResult<T>(result);

        public static implicit operator ValidationResult<T>(Error error) => Failed(error);

        public static implicit operator ValidationResult<T>(T t) => Success(t);

        public static implicit operator Result(ValidationResult<T> result) => result.FromLeft(Result.Success);

        public static implicit operator Maybe<T>(ValidationResult<T> result) => result.Item;
    }
}