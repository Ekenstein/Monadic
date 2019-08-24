using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Extensions
{
    /// <summary>
    /// Provides a set of static extensions for the type <see cref="Either{T1,T2}"/>.
    /// </summary>
    public static class EitherExtensions
    {
        /// <summary>
        /// Returns the left value of the given <paramref name="either"/>. If the <paramref name="either"/>
        /// represents a right value, the exception produced by the given <paramref name="exceptionSelector"/>
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
        public static T1 GetLeftOrThrow<T1, T2>(this Either<T1, T2> either, Func<T2, Exception> exceptionSelector)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (exceptionSelector == null)
            {
                throw new ArgumentNullException(nameof(exceptionSelector));
            }

            return either.MapEither(l => l, r => throw exceptionSelector(r));
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
        public static T2 GetRightOrThrow<T1, T2>(this Either<T1, T2> either, Func<T1, Exception> exceptionSelector)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (exceptionSelector == null)
            {
                throw new ArgumentNullException(nameof(exceptionSelector));
            }

            return either.MapEither(l => throw exceptionSelector(l), r => r);
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
        public static Maybe<T1> GetLeftOrNothing<T1, T2>(this Either<T1, T2> either)
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
        public static Maybe<T2> GetRightOrNothing<T1, T2>(this Either<T1, T2> either)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.MapRight(Maybe<T2>.Nothing, Maybe.Just);
        }

        /// <summary>
        /// Returns the left value of the given <paramref name="either"/> or the default value
        /// of the type <typeparamref name="T1"/> if the <paramref name="either"/> is representing
        /// a right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the left value of.</param>
        /// <returns>
        /// The left value of the given <paramref name="either"/> iff the either is representing
        /// a default value. Otherwise the default value of <typeparamref name="T1"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static T1 GetLeftOrDefault<T1, T2>(this Either<T1, T2> either)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.GetLeftOrDefault(default(T1));
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
        public static T1 GetLeftOrDefault<T1, T2>(this Either<T1, T2> either, T1 defaultValue)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.GetLeftOrDefault(() => defaultValue);
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
        public static T1 GetLeftOrDefault<T1, T2>(this Either<T1, T2> either, Func<T1> defaultValueSelector)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (defaultValueSelector == null)
            {
                throw new ArgumentNullException(nameof(defaultValueSelector));
            }

            return either.MapEither(l => l, r => defaultValueSelector());
        }

        /// <summary>
        /// Returns the right value of the given <paramref name="either"/> or the default value
        /// of <typeparamref name="T2"/> if the either is representing a right value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value.</typeparam>
        /// <typeparam name="T2">The type of the right value.</typeparam>
        /// <param name="either">The either to extract the right value of.</param>
        /// <returns>
        /// The right value of the given <paramref name="either"/> or the default value
        /// of <typeparamref name="T2"/> iff the either is representing a right value.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static T2 GetRightOrDefault<T1, T2>(this Either<T1, T2> either)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.GetRightOrDefault((T2)(default));
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
        public static T2 GetRightOrDefault<T1, T2>(this Either<T1, T2> either, T2 defaultValue)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.GetRightOrDefault(() => defaultValue);
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
        public static T2 GetRightOrDefault<T1, T2>(this Either<T1, T2> either, Func<T2> defaultValueSelector)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (defaultValueSelector == null)
            {
                throw new ArgumentNullException(nameof(defaultValueSelector));
            }

            return either.MapEither(l => defaultValueSelector(), r => r);
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
        public static T3 MapEither<T1, T2, T3>(this Either<T1, T2> either, Func<T1, T3> fromL, Func<T2, T3> fromR)
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
        /// Maps the right value of the given <paramref name="either"/> to a new value
        /// and returns an <see cref="Either{T1, T2}"/> which will contain either the mapped
        /// right value or the left value of the given <paramref name="either"/>.
        /// <typeparamref name="T3"/>.
        /// </summary>
        /// <typeparam name="T1">The type of left value.</typeparam>
        /// <typeparam name="T2">The type of right value.</typeparam>
        /// <typeparam name="T3">The type of the right value of the produced either.</typeparam>
        /// <param name="either">The either to map the right value to a new value.</param>
        /// <param name="map">The function mapping the right value to a new value.</param>
        /// <returns>
        /// An <see cref="Either{T1, T2}"/> where the left value will contain the left value of
        /// the given <paramref name="either"/> and the right
        /// value will be the right value of the given <paramref name="either"/> mapped to <typeparamref name="T3"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="map"/> is null.</exception>
        public static Either<T1, T3> MapRight<T1, T2, T3>(this Either<T1, T2> either, Func<T2, T3> map)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return either.MapEither(l => new Either<T1, T3>(l), r => new Either<T1, T3>(map(r)));
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

            return either.MapEither(l => defaultValueSelector(), map);
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

            return either.MapEither(map, right => defaultValueSelector());
        }

        /// <summary>
        /// Maps the left value of the given <paramref name="either"/> to a new value
        /// and returns an <see cref="Either{T1, T2}"/> which will contain either the mapped
        /// left value or the right value of the given <paramref name="either"/>.
        /// </summary>
        /// <typeparam name="T1">The type of left value.</typeparam>
        /// <typeparam name="T2">The type of right value.</typeparam>
        /// <typeparam name="T3">The type of the left value of the produced either.</typeparam>
        /// <param name="either">The either to map the left value to a new value.</param>
        /// <param name="map">The function mapping the left value to a new value.</param>
        /// <returns>
        /// An <see cref="Either{T1, T2}"/> where the left value will contain the left value of
        /// the given <paramref name="either"/> mapped to <typeparamref name="T3"/> and the right
        /// value will be the right value of the given <paramref name="either"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="map"/> is null.</exception>
        public static Either<T3, T2> MapLeft<T1, T2, T3>(this Either<T1, T2> either, Func<T1, T3> map)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return either.MapEither(l => new Either<T3, T2>(map(l)), r => new Either<T3, T2>(r));
        }

        /// <summary>
        /// Returns an <see cref="Either{T1, T2}"/> which will always represent a left value where the
        /// left value is either the left value of the given <paramref name="either"/> or the default value
        /// of <typeparamref name="T1"/> iff the <paramref name="either"/> represents a right value.
        /// </summary>
        /// <typeparam name="T1">The type of left value.</typeparam>
        /// <typeparam name="T2">The type of right value.</typeparam>
        /// <param name="either">The either where the left value will be extracted from.</param>
        /// <returns>
        /// An <see cref="Either{T1, T2}"/> which will always represent a left value where the left
        /// value is either the left value of the given <paramref name="either"/> or the default value
        /// of <typeparamref name="T1"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static Either<T1, T2> DefaultIfRight<T1, T2>(this Either<T1, T2> either) where T1 : struct
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.DefaultIfRight(default);
        }

        /// <summary>
        /// Returns an <see cref="Either{T1, T2}"/> which will always represent a left value where the
        /// left value is either the left value of the given <paramref name="either"/> or the given
        /// <paramref name="defaultValue"/> iff the <paramref name="either"/> represents a right value.
        /// </summary>
        /// <typeparam name="T1">The type of left value.</typeparam>
        /// <typeparam name="T2">The type of right value.</typeparam>
        /// <param name="either">The either where the left value will be extracted from.</param>
        /// <param name="defaultValue">The default value to use if the either represents a right value.</param>
        /// <returns>
        /// An <see cref="Either{T1, T2}"/> which will always represent a left value where the left
        /// value is either the left value of the given <paramref name="either"/> or the given <paramref name="defaultValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="defaultValue"/> are null.</exception>
        public static Either<T1, T2> DefaultIfRight<T1, T2>(this Either<T1, T2> either, T1 defaultValue)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (defaultValue == null)
            {
                throw new ArgumentNullException(nameof(defaultValue));
            }

            return either.MapLeft(() => new Either<T1, T2>(defaultValue), left => new Either<T1, T2>(left));
        }

        /// <summary>
        /// Returns an <see cref="Either{T1, T2}"/> which will always represent a right value where the
        /// right value is either the right value of the given <paramref name="either"/> or the default value
        /// of <typeparamref name="T2"/> iff the <paramref name="either"/> represents a left value.
        /// </summary>
        /// <typeparam name="T1">The type of left value.</typeparam>
        /// <typeparam name="T2">The type of right value.</typeparam>
        /// <param name="either">The either where the right value will be extracted from.</param>
        /// <returns>
        /// An <see cref="Either{T1, T2}"/> which will always represent a right value where the right
        /// value is either the right value of the given <paramref name="either"/> or the default value
        /// of <typeparamref name="T2"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static Either<T1, T2> DefaultIfLeft<T1, T2>(this Either<T1, T2> either) where T2 : struct
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.DefaultIfLeft(default(T2));
        }

        /// <summary>
        /// Returns an <see cref="Either{T1, T2}"/> which will always represent a right value where the
        /// right value is either the right value of the given <paramref name="either"/> or the given
        /// <paramref name="defaultValue"/> iff the <paramref name="either"/> represents a left value.
        /// </summary>
        /// <typeparam name="T1">The type of left value.</typeparam>
        /// <typeparam name="T2">The type of right value.</typeparam>
        /// <param name="either">The either where the left value will be extracted from.</param>
        /// <param name="defaultValue">The default value to use if the either represents a right value.</param>
        /// <returns>
        /// An <see cref="Either{T1, T2}"/> which will always represent a right value where the right
        /// value is either the right value of the given <paramref name="either"/> or the given <paramref name="defaultValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> or <paramref name="defaultValue"/> are null.</exception>
        public static Either<T1, T2> DefaultIfLeft<T1, T2>(this Either<T1, T2> either, T2 defaultValue)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            if (defaultValue == null)
            {
                throw new ArgumentNullException(nameof(defaultValue));
            }

            return either.MapRight(() => new Either<T1, T2>(defaultValue), right => new Either<T1, T2>(right));
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
        public static (IEnumerable<T1> lefts, IEnumerable<T2> rights) Partition<T1, T2>(this IEnumerable<Either<T1, T2>> eithers)
        {
            if (eithers == null)
            {
                throw new ArgumentNullException(nameof(eithers));
            }

            return eithers.Aggregate((Enumerable.Empty<T1>(), Enumerable.Empty<T2>()), (seed, either) =>
            {
                if (either.IsLeft)
                {
                    return (seed.Item1.Concat(new [] { either.Left }), seed.Item2);
                }
                
                return (seed.Item1, seed.Item2.Concat(new [] { either.Right }));                
            });
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
        public static IEnumerable<T1> GetLefts<T1, T2>(this IEnumerable<Either<T1, T2>> eithers)
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
        public static IEnumerable<T2> GetRights<T1, T2>(this IEnumerable<Either<T1, T2>> eithers)
        {
            if (eithers == null)
            {
                throw new ArgumentNullException(nameof(eithers));
            }

            return eithers.Partition().rights;
        }

        /// <summary>
        /// Returns a new <see cref="Either{T1, T2}"/> where the left value of the given
        /// <paramref name="either"/> becomes the right value,
        /// and the right value of the given <paramref name="either"/> becomes 
        /// the left value.
        /// </summary>
        /// <typeparam name="T1">The type of the left value that should become the right value.</typeparam>
        /// <typeparam name="T2">The type of the right value that should become the left value.</typeparam>
        /// <param name="either">The either to switch sides on.</param>
        /// <returns>
        /// An <see cref="Either{T1, T2}"/> where the left value will be the right
        /// value of the given <paramref name="either"/> and the right value will be
        /// the left value of the given <paramref name="either"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static Either<T2, T1> Reverse<T1, T2>(this Either<T1, T2> either)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either.MapEither(l => new Either<T2, T1>(l), r => new Either<T2, T1>(r));
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> which will either be
        /// empty or a singleton containing the left value of the given <paramref name="either"/>.
        /// </summary>
        /// <typeparam name="T1">The left type.</typeparam>
        /// <typeparam name="T2">The right type.</typeparam>
        /// <param name="either">The either to extract the left value from.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> which will contain the left value of the
        /// given <paramref name="either"/> iff it represents a left value, otherwise
        /// an empty collection will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static IEnumerable<T1> GetLeftOrEmpty<T1, T2>(this Either<T1, T2> either)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either
                .GetLeftOrNothing()
                .AsEnumerable();
        }

        /// <summary>
        /// Returns an <see cref="IEnumerable{T}"/> which will either be
        /// empty or a singleton containing the right value of the given <paramref name="either"/>.
        /// </summary>
        /// <typeparam name="T1">The left type.</typeparam>
        /// <typeparam name="T2">The right type.</typeparam>
        /// <param name="either">The either to extract the right value from.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> which will contain the right value of the
        /// given <paramref name="either"/> iff it represents a right value, otherwise
        /// an empty collection will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="either"/> is null.</exception>
        public static IEnumerable<T2> GetRightOrEmpty<T1, T2>(this Either<T1, T2> either)
        {
            if (either == null)
            {
                throw new ArgumentNullException(nameof(either));
            }

            return either
                .GetRightOrNothing()
                .AsEnumerable();
        }
    }
}
