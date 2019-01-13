using System;
using System.Collections.Generic;
using System.Linq;
using Monadic.Extensions;

namespace Monadic
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
        public Either(T1 left)
        {
            _left = new[] {left};
            _right = new T2[0];
        }

        /// <summary>
        /// Creates an Either representing a <paramref name="right"/> value.
        /// </summary>
        /// <param name="right">The right value.</param>
        public Either(T2 right)
        {
            _left = new T1[0];
            _right = new[] {right};
        }

        public static implicit operator Either<T1, T2>(T1 left) => new Either<T1, T2>(left);
        public static implicit operator Either<T1, T2>(T2 right) => new Either<T1, T2>(right);

        public static implicit operator Maybe<T1>(Either<T1, T2> either) => either.MaybeLeft();
        public static implicit operator Maybe<T2>(Either<T1, T2> either) => either.MaybeRight();

        public override string ToString() => this.FromEither(l => $"Left ({l})", r => $"Right ({r})");

        public bool Equals(Either<T1, T2> other)
        {
            if (ReferenceEquals(this, other)) return true;
            if (IsLeft && other.IsLeft) return Equals(Left, other.Left);
            if (IsRight && other.IsRight) return Equals(Right, other.Right);

            return false;
        }
    }
}
