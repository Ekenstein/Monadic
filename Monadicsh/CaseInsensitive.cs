using System;

namespace Monadicsh
{
    /// <inheritdoc cref="IEquatable{T}"/>
    /// <summary>
    /// A type that provides a case insensitive comparison for strings.
    /// </summary>
    public struct CaseInsensitive : IEquatable<CaseInsensitive>, IComparable<CaseInsensitive>
    {
        /// <summary>
        /// The original string value.
        /// </summary>
        public string Original { get; }

        /// <summary>
        /// Creates an case insensitive string of the given string <paramref name="s"/>.
        /// </summary>
        /// <param name="s">The string to convert to case insensitive.</param>
        public CaseInsensitive(string s)
        {
            Original = s;
        }

        /// <summary>
        /// Returns a bool flag indicating whether the given case insensitive string
        /// <paramref name="x"/> is equal to the other case insensitive string <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The left case insensitive string.</param>
        /// <param name="y">The right case insensitive string.</param>
        /// <returns>
        /// True if both <paramref name="x"/> and <paramref name="y"/> are equal to each
        /// other, otherwise false.
        /// </returns>
        public static bool operator ==(CaseInsensitive x, CaseInsensitive y) => x.Equals(y);

        /// <summary>
        /// Returns a bool flag indicating whether the given case insensitive string
        /// <paramref name="x"/> is not equal to the other case insensitive string <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The left case insensitive string.</param>
        /// <param name="y">The right case insensitive string.</param>
        /// <returns>
        /// True if <paramref name="x"/> is not equal to <paramref name="y"/>, otherwise true.
        /// </returns>
        public static bool operator !=(CaseInsensitive x, CaseInsensitive y) => !x.Equals(y);

        /// <summary>
        /// Extracts and returns the original string from the given case insensitive string <paramref name="s"/>.
        /// </summary>
        /// <param name="s">The case insensitive string to extract the original string from.</param>
        public static implicit operator string(CaseInsensitive s) => s.Original;

        /// <inheritdoc />
        /// <summary>
        /// Returns a flag indicating whether this instance of <see cref="T:Monadicsh.CaseInsensitive" />
        /// is equal to the <paramref name="other" /> <see cref="T:Monadicsh.CaseInsensitive" />.
        /// Two <see cref="T:Monadicsh.CaseInsensitive" /> are equal to each other if they are equal
        /// in a case insensitive manner.
        /// </summary>
        /// <param name="other">The other case insensitive string.</param>
        /// <returns>
        /// True if the current instance is equal to the <paramref name="other"/> case insensitive string,
        /// otherwise false.
        /// </returns>
        public bool Equals(CaseInsensitive other)
        {
            return string.Equals(Original?.ToLower(), other.Original?.ToLower());
        }

        /// <summary>
        /// Returns a flag indicating whether this instance of <see cref="CaseInsensitive"/>
        /// is equal to the given <paramref name="obj"/>.
        /// They are equal to each other if either the given <paramref name="obj"/> share the same reference,
        /// or if they contain a string that are equal each other in a case insensitive manner.
        /// </summary>
        /// <param name="obj">The object to compare the current instance with.</param>
        /// <returns>
        /// True if the given <paramref name="obj"/> is equal to the current instance, otherwise false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is CaseInsensitive other && Equals(other);
        }

        /// <summary>
        /// Returns the hash code of this instance of <see cref="CaseInsensitive"/> string.
        /// </summary>
        /// <returns>
        /// The hash code of this instance of <see cref="CaseInsensitive"/> string.
        /// </returns>
        public override int GetHashCode()
        {
            return Original != null ? Original.ToLower().GetHashCode() : 0;
        }

        /// <summary>
        /// Returns a string representation of the case insensitive string.
        /// </summary>
        /// <returns>
        /// The string representation of the case insensitive string.
        /// </returns>
        public override string ToString() => $"CI ({Original})";

        /// <summary>
        /// Returns a value indicating whether the current <see cref="CaseInsensitive"/> string
        /// precedes, has the same, or follows the <paramref name="other"/> <see cref="CaseInsensitive"/> string
        /// in order.
        /// </summary>
        /// <param name="other">The other case insensitive string to compare the current instance with.</param>
        /// <returns>
        /// Returns 0 if both case insensitive strings are equal, -1 if the current instance precedes the <paramref name="other"/>
        /// case insensitive string or 1 if the current instance follows the <paramref name="other"/> case insensitive string
        /// in order.
        /// </returns>
        public int CompareTo(CaseInsensitive other)
        {
            return string.Compare(Original, other.Original, StringComparison.OrdinalIgnoreCase);
        }
    }
}
