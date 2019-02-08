using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Monadicsh.Extensions;

namespace Monadicsh
{
    /// <summary>
    /// Provides a static set of methods for creating <see cref="Maybe{T}"/>.
    /// </summary>
    public static class Maybe
    {
        /// <summary>
        /// Will create an <see cref="Maybe{T}"/> representing a Just <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type that the Maybe will wrap.</typeparam>
        /// <param name="value">The value that should be wrapped inside a maybe.</param>
        /// <returns>An <see cref="Maybe{T}"/> representing a Just <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="value"/> is null.</exception>
        public static Maybe<T> Just<T>(T value) => Maybe<T>.Just(value);

        /// <summary>
        /// Creates an <see cref="Maybe{T}"/> of the given <paramref name="value"/>.
        /// If the value is null, <see cref="Maybe{T}.Nothing"/> is returned, otherwise
        /// <see cref="Maybe{T}.Just(T)"/> where the value is the extracted value of the given <see cref="Nullable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type that the Maybe will wrap.</typeparam>
        /// <param name="value">The value that the maybe will wrap.</param>
        /// <returns>Either Nothing, if the <paramref name="value"/> is null, or Just <paramref name="value"/>.</returns>
        public static Maybe<T> Create<T>(T? value)
            where T : struct => value == null ? Maybe<T>.Nothing : Just(value.Value);

        /// <summary>
        /// Creates an <see cref="Maybe{T}"/> of the given <paramref name="value"/>.
        /// If the value is null, <see cref="Maybe{T}.Nothing"/> is returned, otherwise
        /// <see cref="Maybe{T}.Just(T)"/>.
        /// </summary>
        /// <typeparam name="T">The type that the Maybe will encapsulate.</typeparam>
        /// <param name="value">The value that the maybe will wrap.</param>
        /// <returns>Either Nothing, if the <paramref name="value"/> is null, or Just <paramref name="value"/>.</returns>
        public static Maybe<T> Create<T>(T value)
        {
            return value == null ? Maybe<T>.Nothing : Just(value);
        }

        /// <summary>
        /// Creates an <see cref="Maybe{T}"/> of the given string <paramref name="value"/>.
        /// If the string is null or white space, Nothing will be returned, otherwise
        /// Just <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The string to create a maybe.</param>
        /// <returns>
        /// Either Nothing if the string is null or white space, or Just <paramref name="value"/> if
        /// the string is non empty.
        /// </returns>
        public static Maybe<string> CreateNonEmpty(string value) => string.IsNullOrWhiteSpace(value)
            ? Maybe<string>.Nothing
            : Just(value);
    }

    /// <summary>
    /// The Maybe type encapsulates an optional value. A value of type <see cref="T:Monadicsh.Maybe`1" /> either contains a value of type <see cref="!:T" /> 
    /// (represented as Just <see cref="!:T" />), or it is empty (represented as Nothing).
    /// </summary>
    /// <typeparam name="T">The type of value the Maybe is wrapping.</typeparam>
    public struct Maybe<T> : IEquatable<Maybe<T>>, IEnumerable<T>
    {
        private readonly IEnumerable<T> _item;

        /// <summary>
        /// Creates an <see cref="Maybe{T}"/> that Just contains the given non-null
        /// <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The value to wrap.</param>
        /// <returns>An <see cref="Maybe{T}"/> which contains Just the given <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="value"/> null.</exception>
        public static Maybe<T> Just(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return new Maybe<T>(new[] { value });
        }

        /// <summary>
        /// Creates an <see cref="Maybe{T}"/> representing Nothing.
        /// </summary>
        public static Maybe<T> Nothing => new Maybe<T>(new T[0]);

        private Maybe(IEnumerable<T> item)
        {
            _item = item;
        }

        /// <summary>
        /// Returns the value the Maybe is representing or throws an <see cref="InvalidOperationException"/>
        /// if the Maybe is representing nothing.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if maybe doesn't contain a value.</exception>
        public T Value => IsJust
            ? _item.Single()
            : throw new InvalidOperationException("The Maybe<T> is representing Nothing.");

        /// <summary>
        /// True if the Maybe is representing nothing, otherwise false.
        /// </summary>
        public bool IsNothing => _item == null || !_item.Any();

        /// <summary>
        /// True if the Maybe is representing a value, otherwise false.
        /// </summary>
        public bool IsJust => !IsNothing;

        /// <summary>
        /// Creates a <see cref="Maybe{T}"/> of the given <paramref name="value"/>.
        /// If the value is null, <see cref="Maybe{T}.Nothing"/> will be returned,
        /// otherwise <see cref="Maybe{T}.Just"/> value.
        /// </summary>
        /// <param name="value">The value to create a maybe of.</param>
        public static implicit operator Maybe<T>(T value) => Maybe.Create(value);

        /// <summary>
        /// Returns a string representation of the Maybe.
        /// </summary>
        /// <returns>The string representation of the maybe.</returns>
        public override string ToString() => this.Map("Nothing", v => $"Just ({v})");

        /// <inheritdoc />
        /// <summary>
        /// Returns a flag indicating whether the <paramref name="other"/> maybe equals the instance.
        /// Two maybe's are equal to each other if either both maybes' are Nothing,
        /// or if the value they are holding are equal to each other.
        /// </summary>
        /// <param name="other">The other maybe to compare with the instance.</param>
        /// <returns>True if the <paramref name="other"/> maybe equals the instance.</returns>
        public bool Equals(Maybe<T> other)
        {
            if (IsNothing && other.IsNothing) return true;
            if (IsJust && other.IsJust)
            {
                return Equals(Value, other.Value);
            }

            return false;
        }

        /// <summary>
        /// Checks equality between the given <paramref name="obj"/> and the instance.
        /// Returns false if the given <paramref name="obj"/> is either null or if
        /// the object isn't of type <see cref="Maybe{T}"/>. Otherwise,
        /// the same equality check as <see cref="Equals(Monadicsh.Maybe{T})"/>
        /// will be performed.
        /// </summary>
        /// <param name="obj">The object to check if it is equal to the instance.</param>
        /// <returns>True if the given <paramref name="obj"/> is equal to the instance, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Maybe<T> other && Equals(other);
        }

        /// <summary>
        /// Returns a hash code for this instance of <see cref="Maybe{T}"/>.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return IsNothing ? 0 : Value.GetHashCode() * 397;
            }
        }

        /// <summary>
        /// Returns an enumerator representing the Maybe. If the maybe represents
        /// Just a value, the enumerator will contain exactly one element. If the maybe
        /// represents Nothing, the enumerator will contain no elements.
        /// </summary>
        /// <returns>
        /// An <see cref="IEnumerator{T}"/> that contains either zero or one element.
        /// </returns>
        public IEnumerator<T> GetEnumerator() => _item.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
