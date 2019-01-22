using System;

namespace Monadicsh
{
    /// <summary>
    /// A type representing a structured and descriptive error.
    /// </summary>
    public class Error : IEquatable<Error>
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
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code must not be null or white space.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Description must not be null or white space.");
            }

            Code = code;
            Description = description;
        }

        public override string ToString() => Code;

        public bool Equals(Error other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Code, other.Code) && string.Equals(Description, other.Description);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Error) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Code.GetHashCode() * 397) ^ Description.GetHashCode();
            }
        }
    }
}
