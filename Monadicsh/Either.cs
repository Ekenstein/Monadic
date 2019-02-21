using System;
using System.Collections.Generic;
using System.Linq;
using Monadicsh.Extensions;

namespace Monadicsh
{
    /// <summary>
    /// The Either type represents values with two possibilities: either a left value, or a right value.
    /// The left value is of the type <typeparamref name="T1"/> and the right value is of the type <typeparamref name="T2"/>.
    /// </summary>
    /// <typeparam name="T1">The left type.</typeparam>
    /// <typeparam name="T2">The right type.</typeparam>
    public class Either<T1, T2> : IEquatable<Either<T1, T2>>
    {
        private readonly IEnumerable<T1> _left;
        private readonly IEnumerable<T2> _right;

        /// <summary>
        /// Extracts the left value or throws an <see cref="InvalidOperationException"/>
        /// if the either is representing a right value.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the either is representing a right value.</exception>
        public T1 Left => IsLeft
            ? _left.Single()
            : throw new InvalidOperationException("The Either<T1, T2> doesn't represent a left value.");

        /// <summary>
        /// Extracts the right value or throws an <see cref="InvalidOperationException"/>
        /// if the either is representing a left value.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the either is representing a left value.</exception>
        public T2 Right => IsRight
            ? _right.Single()
            : throw new InvalidOperationException("The Either<T1, T2> doesn't represent a right value.");

        /// <summary>
        /// Returns true iff the Either represents a left value, otherwise false.
        /// </summary>
        public bool IsLeft => _left.Any();

        /// <summary>
        /// Returns true iff the Either represents a right value, otherwise false.
        /// </summary>
        public bool IsRight => _right.Any();

        /// <summary>
        /// Creates an Either representing a <paramref name="left"/> value.
        /// </summary>
        /// <param name="left">The left value.</param>
        /// <exception cref="ArgumentNullException">If the left value is null.</exception>
        public Either(T1 left)
        {
            if (left == null)
            {
                throw new ArgumentNullException(nameof(left));
            }

            _left = new[] {left};
            _right = new T2[0];
        }

        /// <summary>
        /// Creates an Either representing a <paramref name="right"/> value.
        /// </summary>
        /// <param name="right">The right value.</param>
        /// <exception cref="ArgumentNullException">If the right value is null.</exception>
        public Either(T2 right)
        {
            if (right == null)
            {
                throw new ArgumentNullException(nameof(right));
            }

            _left = new T1[0];
            _right = new[] {right};
        }

        /// <summary>
        /// Implicitly creates an <see cref="Either{T1,T2}"/> which will represent
        /// a left side with the given <paramref name="left"/> value.
        /// </summary>
        /// <param name="left">The value of the left side of the <see cref="Either{T1,T2}"/>.</param>
        public static implicit operator Either<T1, T2>(T1 left) => new Either<T1, T2>(left);

        /// <summary>
        /// Implicitly creates an <see cref="Either{T1,T2}"/> which will represent
        /// a right side with the given <paramref name="right"/> value.
        /// </summary>
        /// <param name="right">The value of the right side of the <see cref="Either{T1,T2}"/>.</param>
        public static implicit operator Either<T1, T2>(T2 right) => new Either<T1, T2>(right);

        /// <summary>
        /// Implicitly creates an <see cref="Maybe{T}"/> of the left side of the given <paramref name="either"/>.
        /// If the given <paramref name="either"/> is representing a left side, <see cref="Maybe{T}.Just"/> will
        /// be returned, otherwise <see cref="Maybe{T}.Nothing"/>.
        /// </summary>
        /// <param name="either">The either to extract the left side of.</param>
        public static implicit operator Maybe<T1>(Either<T1, T2> either) => either.LeftOrNothing();

        /// <summary>
        /// Implicitly creates an <see cref="Maybe{T}"/> of the right side of the given <paramref name="either"/>.
        /// If the given <paramref name="either"/> is representing a right side, <see cref="Maybe{T}.Just"/> will
        /// be returned, otherwise <see cref="Maybe{T}.Nothing"/>.
        /// </summary>
        /// <param name="either">The either to extract the right side of.</param>
        public static implicit operator Maybe<T2>(Either<T1, T2> either) => either.RightOrNothing();

        /// <summary>
        /// Returns a string representation of the current instance of <see cref="Either{T1,T2}"/>.
        /// </summary>
        /// <returns>The string representation of the <see cref="Either{T1,T2}"/>.</returns>
        public override string ToString() => this.FromEither(l => $"Left ({l})", r => $"Right ({r})");

        /// <inheritdoc />
        /// <summary>
        /// Returns a flag indicating whether the <paramref name="other" /> <see cref="T:Monadicsh.Either`2" />
        /// equals the current instance of <see cref="T:Monadicsh.Either`2" />.
        /// Two instances of <see cref="T:Monadicsh.Either`2" /> are equal if they both represent
        /// the same side and the values of that side equals each other.
        /// </summary>
        /// <param name="other">The other instance of <see cref="T:Monadicsh.Either`2" /> to compare with.</param>
        /// <returns>
        /// True if the <paramref name="other" /> instance of <see cref="T:Monadicsh.Either`2" /> is equal
        /// to the current instance of <see cref="T:Monadicsh.Either`2" />, otherwise false.
        /// </returns>
        public bool Equals(Either<T1, T2> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (IsLeft && other.IsLeft) return Equals(Left, other.Left);
            if (IsRight && other.IsRight) return Equals(Right, other.Right);
            return false;
        }

        /// <summary>
        /// Returns a flag indicating whether the given <paramref name="obj"/> is equal to
        /// the current instance of <see cref="Either{T1,T2}"/>.
        /// If the given <paramref name="obj"/> is null or isn't of the type <see cref="Either{T1,T2}"/>,
        /// false will be returned. Otherwise the same equality check will be made as in <see cref="Equals(Monadicsh.Either{T1,T2})"/>.
        /// </summary>
        /// <param name="obj">The object to check if it is equal the current instance of <see cref="Either{T1,T2}"/>.</param>
        /// <returns>
        /// True if the given <paramref name="obj"/> is equal to the current instance of <see cref="Either{T1,T2}"/>,
        /// otherwise false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Either<T1, T2>) obj);
        }

        /// <summary>
        /// Returns the hash code of the current instance of <see cref="Either{T1,T2}"/>.
        /// </summary>
        /// <returns>The hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return this.FromEither(l => l.GetHashCode() * 397, r => r.GetHashCode() * 397);
            }
        }
    }
}
