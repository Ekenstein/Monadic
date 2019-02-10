using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh
{
    /// <summary>
    /// A type representing a result of an operation.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Creates a successful result for the given <paramref name="item"/>.
        /// </summary>
        /// <typeparam name="T">The type of the item to create successful result for.</typeparam>
        /// <param name="item">The item that was successful.</param>
        /// <returns>A successful result containing the given <paramref name="item"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the given <paramref name="item"/> is null.</exception>
        public static Result<T> Create<T>(T item) => Result<T>.Success(item);

        /// <summary>
        /// Whether the operation was successful or not.
        /// </summary>
        public bool Succeeded { get; private set; }

        /// <summary>
        /// The errors that occurred during an unsuccessful operation, if any.
        /// If the operation was successful, an empty list will be returned.
        /// </summary>
        public IReadOnlyList<Error> Errors { get; private set; }

        private Result() {}

        /// <summary>
        /// Represents a successful result of the operation.
        /// </summary>
        public static readonly Result Success = new Result 
        {
            Succeeded = true, 
            Errors = new Error[0]
        };

        /// <summary>
        /// Creates an unsuccessful result of the operation containing the given <paramref name="errors"/>.
        /// </summary>
        /// <param name="errors">The errors that occurred during the operation.</param>
        /// <returns>A <see cref="Result"/> representing an operation that was unsuccessful.</returns>
        /// <remarks>Will filter out all the errors that are null.</remarks>
        public static Result Failed(params Error[] errors) => new Result 
        { 
            Succeeded = false, 
            Errors = errors?.Where(e => e != null).ToArray() ?? new Error[0]
        };

        /// <summary>
        /// Creates an unsuccessful <see cref="Result"/> which will have the given
        /// <paramref name="error"/> as reason to why it was unsuccessful.
        /// </summary>
        /// <param name="error">The error describing why the result was unsuccessful.</param>
        public static implicit operator Result(Error error) => Failed(error);

        /// <summary>
        /// Returns a string representation of the current instance of <see cref="Result"/>.
        /// </summary>
        /// <returns>The string representation of the current instance of <see cref="Result"/>.</returns>
        public override string ToString() => Succeeded
            ? "Success"
            : $"Failed: {string.Join(",", Errors)}";
    }
}
