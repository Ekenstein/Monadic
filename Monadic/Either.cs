using System;
using System.Collections.Generic;
using System.Linq;
using Monadic.Extensions;

namespace Monadic
{
    /// <summary>
    /// The Either type represents values with two possibilities: a value of type 
    /// Either <see cref="T1"/> <see cref="T2"/> is either Left <see cref="T1"/> or Right <see cref="T2"/>.
    /// </summary>
    /// <typeparam name="T1">The left type.</typeparam>
    /// <typeparam name="T2">The right type.</typeparam>
    public class Either<T1, T2>
    {
        private readonly IEnumerable<T1> _left;
        private readonly IEnumerable<T2> _right;

        public T1 Left => IsLeft
            ? _left.Single()
            : throw new InvalidOperationException("The Either<T1, T2> doesn't represent a left value.");

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

        public static implicit operator Maybe<T1>(Either<T1, T2> either) => either.FromLeft(Maybe<T1>.Nothing, Maybe<T1>.Just);
        public static implicit operator Maybe<T2>(Either<T1, T2> either) => either.FromRight<T1, T2, Maybe<T2>>(Maybe<T2>.Nothing, Maybe<T2>.Just);

        public override string ToString() => this.FromEither(l => $"Left ({l})", r => $"Right ({r})");
    }
}
