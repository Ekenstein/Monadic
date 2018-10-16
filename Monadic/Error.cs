using System;

namespace Monadic
{
    /// <summary>
    /// A type representing a structured and descriptive error.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// The code for the error.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// A user friendly description of the error.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Creates an error which has the given error-<paramref name="code"/> and the given <paramref name="description"/>.
        /// </summary>
        /// <param name="code">The code describing the error.</param>
        /// <param name="description">A user friendly description of the error.</param>
        /// <exception cref="ArgumentException">If either code or description are null or white space.</exception>
        public Error(string code, string description)
        {
            if (string.IsNullOrWhiteSpace(nameof(code)))
            {
                throw new ArgumentException("Code must not be null or white space.");
            }

            if (string.IsNullOrWhiteSpace(nameof(description)))
            {
                throw new ArgumentException("Description must not be null or white space.");
            }

            Code = code;
            Description = description;
        }
    }
}
