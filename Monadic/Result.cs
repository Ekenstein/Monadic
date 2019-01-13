using System.Collections.Generic;
using System.Linq;

namespace Monadic
{
    /// <summary>
    /// A type representing a result of an operation.
    /// </summary>
    public class Result
    {
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
        public static Result Failed(params Error[] errors) => new Result 
        { 
            Succeeded = false, 
            Errors = errors?.Where(e => e != null)?.ToArray() ?? new Error[0]
        };

        public static implicit operator Result(Error error) => Failed(error);

        public override string ToString() => Succeeded
            ? "Success"
            : $"Failed: {string.Join(",", Errors)}";
    }
}
