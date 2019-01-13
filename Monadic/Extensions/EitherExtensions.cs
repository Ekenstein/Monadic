using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadic.Extensions
{
    public static class EitherExtensions
    {
        /// <summary>
        /// Returns the left value of the given <paramref name="either"/>. If the <paramref name="either"/>
        /// represents a right value, the exception produced by the given lambda <paramref name="exception"/>
        /// will be thrown.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the left value of.</param>
        /// <param name="exception">The exception to produce if the either is a right value.</param>
        /// <returns>
        /// The left value of the given <paramref name="either"/>. If the <paramref name="either"/> represents
        /// a right value, the exception produced by the lambda <paramref name="exception"/> will be thrown.
        /// </returns>
        public static T1 LeftOrThrow<T1, T2>(this Either<T1, T2> either, Func<T2, Exception> exception) =>
            either.FromEither(l => l, r => throw exception(r));

        /// <summary>
        /// Returns the right value of the given <param name="either"/>. If the <paramref name="either"/>
        /// represents a left value, an exception will be thrown produced by the given lambda <paramref name="exception"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the right value of.</param>
        /// <param name="exception">The exception to produce if the either is a left value.</param>
        /// <returns>
        /// The right value of the given <paramref name="either"/>. If the <paramref name="either"/> represents
        /// a left value, the exception produced by the lambda <paramref name="exception"/> will be thrown.
        /// </returns>
        public static T2 RightOrThrow<T1, T2>(this Either<T1, T2> either, Func<T1, Exception> exception) =>
            either.FromEither(l => throw exception(l), r => r);

        /// <summary>
        /// Returns <see cref="Maybe{T}.Just(T)"/> of the left value or <see cref="Maybe{T}.Nothing"/>
        /// if the <paramref name="either"/> represents a right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the left value from.</param>
        /// <returns>
        /// <see cref="Maybe{T}.Just(T)"/> of the left value or <see cref="Maybe{T}.Nothing"/>
        /// if the <paramref name="either"/> represents a right value.
        /// </returns>
        public static Maybe<T1> MaybeLeft<T1, T2>(this Either<T1, T2> either) => either
            .FromLeft(Maybe<T1>.Nothing, Maybe.Just);

        /// <summary>
        /// Returns the <see cref="Maybe{T}.Just(T)"/> of the right value or <see cref="Maybe{T}.Nothing"/>
        /// if the <paramref name="either"/> represents a left value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the right value from.</param>
        /// <returns><see cref="Maybe{T}.Just(T)"/> of the right value or <see cref="Maybe{T}.Nothing"/> if the either represents a left value.</returns>
        public static Maybe<T2> MaybeRight<T1, T2>(this Either<T1, T2> either) => either
            .FromRight(Maybe<T2>.Nothing, Maybe.Just);

        /// <summary>
        /// Returns the left value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the <paramref name="either"/> isn't representing a Left value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either which the left value should be extracted from.</param>
        /// <param name="defaultValue">The default value to use if the either isn't representing a left value.</param>
        /// <returns>
        /// Either the left value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the <paramref name="either"/> is representing a right value.
        /// </returns>
        public static T1 FromLeft<T1, T2>(this Either<T1, T2> either, T1 defaultValue) => either
            .FromEither(l => l, r => defaultValue);

        /// <summary>
        /// Returns the right value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the <paramref name="either"/> isn't representing a Right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either which the right value should be extracted from.</param>
        /// <param name="defaultValue">The default value to use if the either isn't representing a right value.</param>
        /// <returns>
        /// Either the right value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the <paramref name="either"/> is representing a left value.
        /// </returns>
        public static T2 FromRight<T1, T2>(this Either<T1, T2> either, T2 defaultValue) => either
            .FromEither(l => defaultValue, r => r);

        /// <summary>
        /// Returns a mapped left value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the <paramref name="either"/> isn't representing a left value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <typeparam name="T3">The type that the left value should be mapped to.</typeparam>
        /// <param name="either">The either which the left value should be extracted and mapped from.</param>
        /// <param name="defaultValue">The default value to use if the either isn't representing a left value.</param>
        /// <returns>
        /// Either the mapped left value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the given <paramref name="either"/> is representing a right value.
        /// </returns>
        public static T3 FromLeft<T1, T2, T3>(this Either<T1, T2> either, T3 defaultValue, Func<T1, T3> func) => either
            .FromEither(func, right => defaultValue);

        /// <summary>
        /// Returns a mapped right value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the <paramref name="either"/> isn't representing a right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <typeparam name="T3">The type that the left value should be mapped to.</typeparam>
        /// <param name="either">The either which the right value should be extracted and mapped from.</param>
        /// <param name="defaultValue">The default value to use if the either isn't representing a right value.</param>
        /// <returns>
        /// Either the mapped right value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the given <paramref name="either"/> is representing a left value.
        /// </returns>
        public static T3 FromRight<T1, T2, T3>(this Either<T1, T2> either, T3 defaultValue, Func<T2, T3> func) => either
            .FromEither(left => defaultValue, func);

        public static T2 FromRight<T1, T2>(this Either<T1, T2> either, Func<T2> defaultValue) => 
            FromEither(either, l => defaultValue(), r => r);

        public static T1 FromLeft<T1, T2>(this Either<T1, T2> either, Func<T1> defaultValue) =>
            either.FromEither(l => l, r => defaultValue());

        public static T3 FromEither<T1, T2, T3>(this Either<T1, T2> either, Func<T1, T3> fromL, Func<T2, T3> fromR) => 
            either.IsLeft 
                ? fromL(either.Left) 
                : fromR(either.Right);

        /// <summary>
        /// Executes <paramref name="fromL"/> if the given <paramref name="either"/> contains a left value,
        /// otherwise <paramref name="fromR"/> will be executed.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract values from.</param>
        /// <param name="fromL">The action to be executed if the Either represents a left value.</param>
        /// <param name="fromR">The action to be executed if the Either represents a right value.</param>
        public static void FromEither<T1, T2>(this Either<T1, T2> either, Action<T1> fromL, Action<T2> fromR)
        {
            if (either.IsLeft)
            {
                fromL(either.Left);
            }
            else
            {
                fromR(either.Right);
            }
        }

        /// <summary>
        /// Partitions a list of Either into two lists. 
        /// All the Left elements are extracted, in order, to the first component of the output. 
        /// Similarly the Right elements are extracted to the second component of the output.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="eithers">The list of <see cref="Either{T1, T2}"/>.</param>
        /// <returns>
        /// A <see cref="Tuple{T1, T2}"/> where <see cref="Tuple{T1, T2}.Item1"/> is all the lefts and
        /// <see cref="Tuple{T1, T2}.Item2"/> is all the rights.
        /// </returns>
        public static (T1[] lefts, T2[] rights) Partition<T1, T2>(this IEnumerable<Either<T1, T2>> eithers)
        {
            return eithers.Aggregate((new List<T1>(), new List<T2>()), (seed, either) =>
            {
                if (either.IsLeft)
                {
                    seed.Item1.Add(either.Left);
                }
                else
                {
                    seed.Item2.Add(either.Right);   
                }

                return seed;
            }, acc => (acc.Item1.ToArray(), acc.Item2.ToArray()));
        }

        /// <summary>
        /// Extracts from a list of <see cref="Either{T1, T2}"/> all the left elements. All the
        /// left elements are extracted in order.
        /// </summary>
        /// <typeparam name="T1">The left type.</typeparam>
        /// <typeparam name="T2">The right type.</typeparam>
        /// <param name="eithers">The list of <see cref="Either{T1, T2}"/>.</param>
        /// <returns>All the left elements.</returns>
        public static T1[] Lefts<T1, T2>(this IEnumerable<Either<T1, T2>> eithers) => eithers
            .Partition().lefts;

        /// <summary>
        /// Extracts from a list of <see cref="Either{T1, T2}"/> all the right elements. All the
        /// right elements are extracted in order.
        /// </summary>
        /// <typeparam name="T1">The left type.</typeparam>
        /// <typeparam name="T2">The right type.</typeparam>
        /// <param name="eithers">The list of <see cref="Either{T1, T2}"/>.</param>
        /// <returns>All the right elements.</returns>
        public static T2[] Rights<T1, T2>(this IEnumerable<Either<T1, T2>> eithers) => eithers
            .Partition().rights;
    }
}
