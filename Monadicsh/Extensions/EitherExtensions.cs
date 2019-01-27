﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Extensions
{
    public static class EitherExtensions
    {
        /// <summary>
        /// Returns the left value of the given <paramref name="either"/>. If the <paramref name="either"/>
        /// represents a right value, the exceptionSelector produced by the given <paramref name="exceptionSelector"/>
        /// will be thrown.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the left value of.</param>
        /// <param name="exceptionSelector">The function that produces the exception that should be thrown if the either is a right value.</param>
        /// <returns>
        /// The left value of the given <paramref name="either"/>. If the <paramref name="either"/> represents
        /// a right value, the exception produced by the given <paramref name="exceptionSelector"/> will be thrown.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="exceptionSelector"/> is null.</exception>
        public static T1 LeftOrThrow<T1, T2>(this Either<T1, T2> either, Func<T2, Exception> exceptionSelector)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (exceptionSelector == null)
            {
                throw new ArgumentNullException(nameof(exceptionSelector));
            }

            return either.FromEither(l => l, r => throw exceptionSelector(r));
        }

        /// <summary>
        /// Returns the right value of the given <paramref name="either"/>. If the <paramref name="either"/>
        /// represents a left value, the exception produced by the given <paramref name="exceptionSelector"/>
        /// will be thrown.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the left value of.</param>
        /// <param name="exceptionSelector">The function that produces the exception that should be thrown if the either is a right value.</param>
        /// <returns>
        /// The left value of the given <paramref name="either"/>. If the <paramref name="either"/> represents
        /// a right value, the exception produced by the given <paramref name="exceptionSelector"/> will be thrown.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="exceptionSelector"/> is null.</exception>
        public static T2 RightOrThrow<T1, T2>(this Either<T1, T2> either, Func<T1, Exception> exceptionSelector)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (exceptionSelector == null)
            {
                throw new ArgumentNullException(nameof(exceptionSelector));
            }

            return either.FromEither(l => throw exceptionSelector(l), r => r);
        }

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
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static Maybe<T1> MaybeLeft<T1, T2>(this Either<T1, T2> either)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.MapLeft(Maybe<T1>.Nothing, Maybe.Just);
        }

        /// <summary>
        /// Returns the <see cref="Maybe{T}.Just(T)"/> of the right value or <see cref="Maybe{T}.Nothing"/>
        /// if the <paramref name="either"/> represents a left value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the right value from.</param>
        /// <returns><see cref="Maybe{T}.Just(T)"/> of the right value or <see cref="Maybe{T}.Nothing"/> if the either represents a left value.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static Maybe<T2> MaybeRight<T1, T2>(this Either<T1, T2> either)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.MapRight(Maybe<T2>.Nothing, Maybe.Just);
        }

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
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static T1 FromLeft<T1, T2>(this Either<T1, T2> either, T1 defaultValue)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.FromLeft(() => defaultValue);
        }

        /// <summary>
        /// Returns the left value of the given <paramref name="either"/> or the value produced by the given 
        /// <paramref name="defaultValueSelector"/> if the <paramref name="either"/> isn't representing a Left value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either which the left value should be extracted from.</param>
        /// <param name="defaultValueSelector">The function producing the default value to use if the either isn't representing a left value.</param>
        /// <returns>
        /// Either the left value of the given <paramref name="either"/> or the value produced by <paramref name="defaultValueSelector"/>
        /// if the <paramref name="either"/> is representing a right value.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="defaultValueSelector"/> is null.</exception>
        public static T1 FromLeft<T1, T2>(this Either<T1, T2> either, Func<T1> defaultValueSelector)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (defaultValueSelector == null)
            {
                throw new ArgumentNullException(nameof(defaultValueSelector));
            }

            return either.FromEither(l => l, r => defaultValueSelector());
        }

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
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static T2 FromRight<T1, T2>(this Either<T1, T2> either, T2 defaultValue)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.FromRight(() => defaultValue);
        }

        /// <summary>
        /// Returns the right value of the given <paramref name="either"/> or the value produced by the
        /// given <paramref name="defaultValueSelector"/> if the <paramref name="either"/> isn't representing a Right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either which the right value should be extracted from.</param>
        /// <param name="defaultValueSelector">The function producing the default value iff the either is representing a left value.</param>
        /// <returns>
        /// Either the right value of the given <paramref name="either"/> or the value produced by the given <paramref name="defaultValueSelector"/>
        /// if the <paramref name="either"/> is representing a left value.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="defaultValueSelector"/> is null.</exception>
        public static T2 FromRight<T1, T2>(this Either<T1, T2> either, Func<T2> defaultValueSelector)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (defaultValueSelector == null)
            {
                throw new ArgumentNullException(nameof(defaultValueSelector));
            }

            return either.FromEither(l => defaultValueSelector(), r => r);
        }

        /// <summary>
        /// Returns a mapped value <typeparamref name="T3"/> from either the left value of the
        /// given <paramref name="either"/> or the right value depending on which value the
        /// either is representing.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <typeparam name="T3">The type that either left or right will be mapped to.</typeparam>
        /// <param name="either">The either containing either a left or a right side that should be mapped to another value.</param>
        /// <param name="fromL">The function mapping the left value to <typeparamref name="T3"/>.</param>
        /// <param name="fromR">The function mapping the right value to <typeparamref name="T3"/>.</param>
        /// <returns>
        /// A value of type <typeparamref name="T3"/> that is either mapped from the left or the right side of
        /// the given <paramref name="either"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="either"/>, <paramref name="fromL"/> or <paramref name="fromR"/> are null.
        /// </exception>
        public static T3 FromEither<T1, T2, T3>(this Either<T1, T2> either, Func<T1, T3> fromL, Func<T2, T3> fromR)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (fromL == null)
            {
                throw new ArgumentNullException(nameof(fromL));
            }

            if (fromR == null)
            {
                throw new ArgumentNullException(nameof(fromR));
            }

            return either.IsLeft
                ? fromL(either.Left)
                : fromR(either.Right);
        }


        /// <summary>
        /// Executes <paramref name="fromL"/> if the given <paramref name="either"/> contains a left value,
        /// otherwise <paramref name="fromR"/> will be executed.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract values from.</param>
        /// <param name="fromL">The action to be executed if the Either represents a left value.</param>
        /// <param name="fromR">The action to be executed if the Either represents a right value.</param>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="either"/>, <paramref name="fromL"/> or <paramref name="fromR"/> are null.
        /// </exception>
        public static void DoEither<T1, T2>(this Either<T1, T2> either, Action<T1> fromL, Action<T2> fromR)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (fromL == null)
            {
                throw new ArgumentNullException(nameof(fromL));
            }

            if (fromR == null)
            {
                throw new ArgumentNullException(nameof(fromR));
            }

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
        /// Returns a mapped left value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the <paramref name="either"/> isn't representing a left value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <typeparam name="T3">The type that the left value should be mapped to.</typeparam>
        /// <param name="either">The either which the left value should be extracted and mapped from.</param>
        /// <param name="defaultValue">The default value to use if the either isn't representing a left value.</param>
        /// <param name="map">The function that will map the left value to a new type.</param>
        /// <returns>
        /// Either the mapped left value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the given <paramref name="either"/> is representing a right value.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="map"/> is null</exception>
        public static T3 MapLeft<T1, T2, T3>(this Either<T1, T2> either, T3 defaultValue, Func<T1, T3> map)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return either.MapLeft(() => defaultValue, map);
        }

        /// <summary>
        /// Returns a mapped left value of the given <paramref name="either"/> or the value produced
        /// by the given <paramref name="defaultValueSelector"/> iff the either is representing a right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <typeparam name="T3">The type that the left value will be mapped to.</typeparam>
        /// <param name="either">The either which the left value should be extracted and mapped from.</param>
        /// <param name="defaultValueSelector">The function producing a mapped default value.</param>
        /// <param name="map">The function producing a mapped left value of the either.</param>
        /// <returns>
        /// Either the mapped left value of the given <paramref name="either"/> or the value produced
        /// by the given <paramref name="defaultValueSelector"/> iff the either represents a right value.
        /// </returns>
        public static T3 MapLeft<T1, T2, T3>(this Either<T1, T2> either, Func<T3> defaultValueSelector, Func<T1, T3> map)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (defaultValueSelector == null)
            {
                throw new ArgumentNullException(nameof(defaultValueSelector));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return either.FromEither(map, right => defaultValueSelector());
        }

        /// <summary>
        /// Returns a mapped right value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the <paramref name="either"/> isn't representing a right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <typeparam name="T3">The type that the left value should be mapped to.</typeparam>
        /// <param name="either">The either which the right value should be extracted and mapped from.</param>
        /// <param name="defaultValue">The default value to use if the either isn't representing a right value.</param>
        /// <param name="map">The function that will map the right value to a new type.</param>
        /// <returns>
        /// Either the mapped right value of the given <paramref name="either"/> or the <paramref name="defaultValue"/>
        /// if the given <paramref name="either"/> is representing a left value.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="map"/> is null.</exception>
        public static T3 MapRight<T1, T2, T3>(this Either<T1, T2> either, T3 defaultValue, Func<T2, T3> map)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return either.MapRight(() => defaultValue, map);
        }

        /// <summary>
        /// Returns a mapped right value of the given <paramref name="either"/> or the value produced by the given
        /// <paramref name="defaultValueSelector"/> iff the <paramref name="either"/> isn't representing a right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <typeparam name="T3">The type that the left value should be mapped to.</typeparam>
        /// <param name="either">The either which the right value should be extracted and mapped from.</param>
        /// <param name="defaultValueSelector">The default value to use if the either isn't representing a right value.</param>
        /// <param name="map">The function that will map the right value to a new type.</param>
        /// <returns>
        /// Either the mapped right value of the given <paramref name="either"/> or the value produced by the given
        /// <paramref name="defaultValueSelector"/> iff the given <paramref name="either"/> is representing a left value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="either"/>, <paramref name="defaultValueSelector"/> or <paramref name="map"/> is null.
        /// </exception>
        public static T3 MapRight<T1, T2, T3>(this Either<T1, T2> either, Func<T3> defaultValueSelector,
            Func<T2, T3> map)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (defaultValueSelector == null)
            {
                throw new ArgumentNullException(nameof(defaultValueSelector));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return either.FromEither(l => defaultValueSelector(), map);
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
        /// <exception cref="ArgumentNullException">If <paramref name="eithers"/> is null.</exception>
        public static (T1[] lefts, T2[] rights) Partition<T1, T2>(this IEnumerable<Either<T1, T2>> eithers)
        {
            if (eithers == null)
            {
                throw new ArgumentNullException(nameof(eithers));
            }

            var arr = eithers as Either<T1, T2>[] ?? eithers.ToArray();
            if (!arr.Any())
            {
                return (new T1[0], new T2[0]);
            }

            return arr.Aggregate((new List<T1>(), new List<T2>()), (seed, either) =>
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
        /// <exception cref="ArgumentNullException">If <paramref name="eithers"/> is null.</exception>
        public static T1[] Lefts<T1, T2>(this IEnumerable<Either<T1, T2>> eithers)
        {
            if (eithers == null)
            {
                throw new ArgumentNullException(nameof(eithers));
            }

            return eithers.Partition().lefts;
        }

        /// <summary>
        /// Extracts from a list of <see cref="Either{T1, T2}"/> all the right elements. All the
        /// right elements are extracted in order.
        /// </summary>
        /// <typeparam name="T1">The left type.</typeparam>
        /// <typeparam name="T2">The right type.</typeparam>
        /// <param name="eithers">The list of <see cref="Either{T1, T2}"/>.</param>
        /// <returns>All the right elements.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="eithers"/> is null.</exception>
        public static T2[] Rights<T1, T2>(this IEnumerable<Either<T1, T2>> eithers)
        {
            if (eithers == null)
            {
                throw new ArgumentNullException(nameof(eithers));
            }

            return eithers.Partition().rights;
        }
    }
}
