using System;

namespace Monadicsh
{
    /// <inheritdoc />
    /// <summary>
    /// A type representing a structured and descriptive error.
    /// </summary>
    public class Error : IEquatable<Error>
    {
        /// <summary>
        /// The code for the error.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// A user friendly description of the error.
        /// </summary>
        public string Description { get; set; }

        public Error() { }

        /// <summary>
        /// Creates an Error with the given <paramref name="code"/> and <paramref name="description"/>.
        /// </summary>
        /// <param name="code">The code for the error.</param>
        /// <param name="description">The description of the error.</param>
        public Error(string code, string description) : this()
        {
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
