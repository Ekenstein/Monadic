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

        /// <summary>
        /// The default constructor of <see cref="Error"/>.
        /// </summary>
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

        /// <summary>
        /// Returns a string representation of the current instance of <see cref="Error"/>.
        /// </summary>
        /// <returns>The string representation of the current instance of <see cref="Error"/>.</returns>
        public override string ToString() => Code;

        /// <inheritdoc />
        /// <summary>
        /// Returns a flag indicating whether the <paramref name="other" /> error is equal
        /// to the current instance of <see cref="T:Monadicsh.Error" />.
        /// Two errors are equal if they have the same code and the same description, or if
        /// they share the same reference.
        /// </summary>
        /// <param name="other">The other instance of <see cref="T:Monadicsh.Error" /> to compare to the current instance.</param>
        /// <returns>True if the <paramref name="other" /> error is equal to the current instance of <see cref="T:Monadicsh.Error" />.</returns>
        public bool Equals(Error other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Code, other.Code) && string.Equals(Description, other.Description);
        }

        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="obj"/> is equal to the current
        /// instance of <see cref="Error"/>.
        /// The object isn't equal to the current instance if the object is null, or if the object
        /// isn't of type <see cref="Error"/>. Otherwise the same equality check will be made as <see cref="Equals(Monadicsh.Error)"/>.
        /// </summary>
        /// <param name="obj">The object to compare to the current instance of <see cref="Error"/>.</param>
        /// <returns>True if the given <paramref name="obj"/> is equal to the current instance. Otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Error) obj);
        }

        /// <summary>
        /// Returns the hash code of the current instance of <see cref="Error"/>.
        /// </summary>
        /// <returns>The hash code of the current instance <see cref="Error"/>.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (Code?.GetHashCode() ?? 0 * 397) ^ Description?.GetHashCode() ?? 0;
            }
        }
    }
}
