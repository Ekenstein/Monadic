using System;
using System.Linq;

namespace Monadicsh
{
    /// <summary>
    /// A string that only contains alpha-numeric chars.
    /// </summary>
    public struct AlphaNumericString : IEquatable<AlphaNumericString>
    {
        /// <summary>
        /// The original string value. Can be null.
        /// </summary>
        public string OriginalString { get; }

        /// <summary>
        /// Converts the given string <paramref name="s"/>
        /// to an alpha-numeric only string.
        /// </summary>
        /// <param name="s">The string to convert to an alpha numeric string.</param>
        public AlphaNumericString(string s)
        {
            OriginalString = s;
        }

        /// <summary>
        /// Returns either <see cref="string.Empty"/> or a string
        /// that only contains letters or digits.
        /// </summary>
        /// <returns>
        /// The non-null alpha numeric string.
        /// </returns>
        public string GetValue() => string.IsNullOrWhiteSpace(OriginalString)
            ? string.Empty
            : string.Join(string.Empty, OriginalString.Where(char.IsLetterOrDigit));

        /// <summary>
        /// Returns the alpha numeric string as a <see cref="string"/>.
        /// </summary>
        /// <param name="s">The alpha numeric string to convert to a <see cref="string"/>.</param>
        public static implicit operator string(AlphaNumericString s) => s.GetValue();

        /// <summary>
        /// Checks whether the two instances of <see cref="AlphaNumericString"/> are equal to each other.
        /// </summary>
        /// <returns>
        /// True if they are equal to each other, otherwise false.
        /// </returns>
        public static bool operator ==(AlphaNumericString s1, AlphaNumericString s2) => s1.Equals(s2);

        /// <summary>
        /// Checks whether the two instances of <see cref="AlphaNumericString"/> aren't equal to each other.
        /// </summary>
        /// <returns>True if they aren't equal to each other, otherwise false.</returns>
        public static bool operator !=(AlphaNumericString s1, AlphaNumericString s2) => !s1.Equals(s2);

        /// <summary>
        /// Returns a flag indicating whether the current alpha numeric string
        /// is equal to the <paramref name="other"/> alpha numeric string.
        /// </summary>
        /// <param name="other">The other alpha numeric string to compare with.</param>
        /// <returns>
        /// True if the current alpha numeric string is equal to the <paramref name="other"/>
        /// alpha numeric string, otherwise false.
        /// </returns>
        public bool Equals(AlphaNumericString other)
        {
            return string.Equals(other.GetValue(), other.GetValue());
        }

        /// <summary>
        /// Returns a flag indicating whether the current alpha numeric string
        /// is equal to the given <paramref name="obj"/> or not.
        /// </summary>
        /// <param name="obj">The object to compare the current alpha numeric string with.</param>
        /// <returns>
        /// True if the given <paramref name="obj"/> is equal to the current alpha numeric string, otherwise false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AlphaNumericString other && Equals(other);
        }

        /// <summary>
        /// Returns the hash code of the alpha numeric string.
        /// </summary>
        public override int GetHashCode()
        {
            return GetValue().GetHashCode();
        }
    }
}
