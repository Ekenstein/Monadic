using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Extensions
{
    /// <summary>
    /// Provides a static set of extension methods for the <see cref="Maybe{T}"/> type.
    /// </summary>
    public static class MaybeExtensions
    {
        /// <summary>
        /// Returns the value of the given <paramref name="maybe"/> if the maybe contains a value. Otherwise the given
        /// <paramref name="defaultValue"/> will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that either contains a value or nothing.</param>
        /// <param name="defaultValue">The default value to return if the maybe is representing nothing.</param>
        /// <returns>
        /// The value of the given <paramref name="maybe"/> iff the maybe contains a value. Otherwise the given
        /// <paramref name="defaultValue"/> will be returned.
        /// </returns>
        public static T Or<T>(this Maybe<T> maybe, T defaultValue) => maybe.Or(() => defaultValue);

        /// <summary>
        /// Returns the value of the given <paramref name="maybe"/> if the maybe contains a value. Otherwise the value
        /// produced by the given <paramref name="defaultValueSelector"/> will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that either contains a value or nothing.</param>
        /// <param name="defaultValueSelector">The function producing the default value if the maybe is representing nothing.</param>
        /// <returns>
        /// The value of the given <paramref name="maybe"/> iff the maybe contains a value. Otherwise the value
        /// produced by the given <paramref name="defaultValueSelector"/> will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="defaultValueSelector"/> is null.</exception>
        public static T Or<T>(this Maybe<T> maybe, Func<T> defaultValueSelector) => maybe.Map(defaultValueSelector, v => v);

        /// <summary>
        /// Returns the value of the given <paramref name="maybe"/> or null if the maybe
        /// is representing Nothing.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that should have its value extracted from it.</param>
        /// <returns>
        /// The value of the given <paramref name="maybe"/> or null if the maybe is representing Nothing.
        /// </returns>
        public static T OrNull<T>(this Maybe<T> maybe) where T : class => maybe
            .Or(() => default(T));

        /// <summary>
        /// Returns either the value of the maybe mapped to the new type <typeparamref name="T2"/> or
        /// the produced default value iff the maybe represents Nothing.
        /// </summary>
        /// <typeparam name="T1">The type of the value that the maybe is currently holding.</typeparam>
        /// <typeparam name="T2">The type that the value of the maybe will be mapped to.</typeparam>
        /// <param name="maybe">The maybe that either contains a value or nothing.</param>
        /// <param name="defaultValueSelector">The function producing the default value if the maybe represents Nothing.</param>
        /// <param name="map">The function mapping the value of the maybe if the maybe contains a value.</param>
        /// <returns>
        /// A value of type <typeparamref name="T2"/> either mapped from the value of the maybe, or the value produced
        /// by the given <paramref name="defaultValueSelector"/> iff the maybe represents Nothing.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="defaultValueSelector"/> or <paramref name="map"/> is null.</exception>
        public static T2 Map<T1, T2>(this Maybe<T1> maybe, Func<T2> defaultValueSelector, Func<T1, T2> map)
        {
            if (defaultValueSelector == null)
            {
                throw new ArgumentNullException(nameof(defaultValueSelector));
            }

            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return maybe.IsNothing
                ? defaultValueSelector()
                : map(maybe.Value);
        }

        /// <summary>
        /// Returns either the value of the maybe mapped to the new type <typeparamref name="T2"/> or
        /// the given <paramref name="defaultValue"/> iff the maybe represents Nothing.
        /// </summary>
        /// <typeparam name="T1">The type of the value that the maybe is currently holding.</typeparam>
        /// <typeparam name="T2">The type that the value of the maybe will be mapped to.</typeparam>
        /// <param name="maybe">The maybe that either contains a value or nothing.</param>
        /// <param name="defaultValue">The default value to return if the maybe represents Nothing.</param>
        /// <param name="map">The function mapping the value of the maybe if the maybe contains a value.</param>
        /// <returns>
        /// A value of type <typeparamref name="T2"/> either mapped from the value of the maybe, or the
        /// given <paramref name="defaultValue"/> iff the maybe represents Nothing.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="map"/> is null.</exception>
        public static T2 Map<T1, T2>(this Maybe<T1> maybe, T2 defaultValue, Func<T1, T2> map) => maybe
            .Map(() => defaultValue, map);

        /// <summary>
        /// Returns the value of the given <paramref name="maybe"/> or throws
        /// the exception produced by the given <paramref name="exceptionSelector"/> 
        /// iff the maybe represents Nothing.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that either contains a value or nothing.</param>
        /// <param name="exceptionSelector">The function producing the exception that will be thrown if the Maybe represents Nothing.</param>
        /// <returns>
        /// The value of the maybe iff the maybe contains a value, otherwise the exception produced 
        /// by the <paramref name="exceptionSelector"/> will be thrown.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="exceptionSelector"/> is null.</exception>
        public static T OrThrow<T>(this Maybe<T> maybe, Func<Exception> exceptionSelector)
        {
            if (exceptionSelector == null)
            {
                throw new ArgumentNullException(nameof(exceptionSelector));
            }

            return maybe.Or(() => throw exceptionSelector());
        }

        /// <summary>
        /// Returns an <see cref="Monadicsh.Either{T1, T2}"/> representation of the given <paramref name="maybe"/>.
        /// If the maybe is representing Nothing, the left side will be populated with the value produced with the given <paramref name="defaultValueSelector"/>,
        /// otherwise the right side will be populated by the value of the maybe.
        /// </summary>
        /// <typeparam name="T1">The type of the value that the maybe is holding and the type the left side of the either will have.</typeparam>
        /// <typeparam name="T2">The type of the value that the right side of the either will have.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <param name="defaultValueSelector">The function producing the default value of type <typeparamref name="T2"/> iff the maybe represents nothing.</param>
        /// <returns>
        /// An <see cref="Monadicsh.Either{T1, T2}"/> representation of the given <paramref name="maybe"/>.
        /// If the maybe is representing Nothing, the left side will be populated with the value produced by the given <paramref name="defaultValueSelector"/>,
        /// otherwise the right side will be populated by the value of the maybe.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="defaultValueSelector"/> is null or the value produced by the value selector is null.</exception>
        public static Either<T1, T2> ToEither<T1, T2>(this Maybe<T2> maybe, Func<T1> defaultValueSelector)
        {
            if (defaultValueSelector == null)
            {
                throw new ArgumentNullException(nameof(defaultValueSelector));
            }

            return maybe.Map(() => new Either<T1, T2>(defaultValueSelector()), v => v);
        }

        /// <summary>
        /// Returns an <see cref="Monadicsh.Either{T1, T2}"/> representation of the given <paramref name="maybe"/>.
        /// If the maybe is representing Nothing, the left side will be populated by the given <paramref name="defaultValue"/>,
        /// otherwise the right side will be populated by the value of the maybe.
        /// </summary>
        /// <typeparam name="T1">The type of the value that the maybe is holding and the type the left side of the either will have.</typeparam>
        /// <typeparam name="T2">The type of the value that the right side of the either will have.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <param name="defaultValue">The default value to use iff the maybe represents nothing.</param>
        /// <returns>
        /// An <see cref="Monadicsh.Either{T1, T2}"/> representation of the given <paramref name="maybe"/>.
        /// If the maybe is representing Nothing, the left side will be populated by the given <paramref name="defaultValue"/>,
        /// otherwise the right side will be populated by the value of the maybe.
        /// </returns>
        public static Either<T1, T2> ToEither<T1, T2>(this Maybe<T2> maybe, T1 defaultValue) =>
            maybe.ToEither(() => defaultValue);

        /// <summary>
        /// Tests the given <paramref name="maybe"/> for Nothing before performing
        /// the given <paramref name="valueSelector"/>. If the given <paramref name="maybe"/> is Nothing,
        /// Nothing will be returned, otherwise the <paramref name="valueSelector"/> will be performed and
        /// returned. Similar to the null-conditional operator (?.).
        /// </summary>
        /// <typeparam name="T1">The type of the value that the maybe is holding.</typeparam>
        /// <typeparam name="T2">The type of the value that the resulting Maybe will be holding.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <param name="valueSelector">The function to be performed and iff the given maybe contains a value.</param>
        /// <returns>
        /// Either a Maybe representing Nothing or the maybe returned by the given <paramref name="valueSelector"/>
        /// produced with the value of the given <paramref name="maybe"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="valueSelector"/> is null.</exception>
        public static Maybe<T2> Coalesce<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> valueSelector)
        {
            if (valueSelector == null)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }

            return maybe.Map(() => Maybe<T2>.Nothing, valueSelector);
        }

        /// <summary>
        /// Tests the given <paramref name="maybe"/> for Nothing before performing
        /// the given <paramref name="valueSelector"/>. If the given <paramref name="maybe"/> is Nothing,
        /// Nothing will be returned, otherwise the <paramref name="valueSelector"/> will be performed and
        /// returned. Similar to the null-conditional operator (?.).
        /// </summary>
        /// <typeparam name="T1">The type of the value that the maybe is holding.</typeparam>
        /// <typeparam name="T2">The type of the value that the resulting Maybe will be holding.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <param name="valueSelector">The function to be performed and iff the given maybe contains a value.</param>
        /// <returns>
        /// Either a Maybe representing Nothing or a maybe that is created with the returned value of the <paramref name="valueSelector"/>
        /// produced with the value of the given <paramref name="maybe"/>.
        /// If the returned value of the given <paramref name="valueSelector"/> is null, Nothing will be returned, otherwise Just.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="valueSelector"/> is null.</exception>
        public static Maybe<T2> Coalesce<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> valueSelector) => maybe
            .Coalesce(t => Maybe.Create(valueSelector(t)));

        /// <summary>
        /// Returns a <see cref="Nullable{T}"/> representation of the given <paramref name="maybe"/>
        /// where the value will be null if the given <paramref name="maybe"/> is Nothing, otherwise
        /// a non-null value will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <returns>
        /// A <see cref="Nullable{T}"/> of type <typeparamref name="T"/> which will be null
        /// if the given <paramref name="maybe"/> is Nothing, otherwise a non-null value.
        /// </returns>
        public static T? ToNullable<T>(this Maybe<T> maybe) where T : struct
        {
            return maybe.Map(() => default(T?), v => v);
        }

        /// <summary>
        /// Returns a collection of type <typeparamref name="T"/> extracted from the maybes that contains
        /// a value.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybes">The collection of maybes that will have their values extracted, if a value exists.</param>
        /// <returns>
        /// A collection of zero or more values of type <typeparamref name="T"/> that are extracted from the collection of
        /// maybes where the maybe must contain a value.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="maybes"/> is null.</exception>
        public static IEnumerable<T> Just<T>(this IEnumerable<Maybe<T>> maybes) => maybes
            .Where(m => m.IsJust)
            .Select(m => m.Value);

        /// <summary>
        /// Checks whether the given <paramref name="value"/> is equal to the value of the <paramref name="maybe"/>.
        /// The equality check will be performed with the standard equality comparer for the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <param name="value">The value to compare with the value of the maybe.</param>
        /// <returns>
        /// True if the given <paramref name="value"/> is equal to the value of the <paramref name="maybe"/>. Otherwise
        /// false if either the maybe contains Nothing or that the value of the maybe isn't equal
        /// to the given <paramref name="value"/>.
        /// </returns>
        public static bool Is<T>(this Maybe<T> maybe, T value) => maybe
            .Is(value, EqualityComparer<T>.Default);

        /// <summary>
        /// Checks whether the given <paramref name="value"/> is equal to the value of the maybe.
        /// The equality check will be performed with the given <paramref name="equalityComparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <param name="value">The value to compare with the value of the maybe.</param>
        /// <param name="equalityComparer">The equality comparer to compare the values with.</param>
        /// <returns>
        /// True if the given <paramref name="value"/> is equal to the value of the <paramref name="maybe"/>
        /// determined by the given <paramref name="equalityComparer"/>. Otherwise false
        /// if either the maybe contains nothing or that the value of maybe isn't equal to the given
        /// <paramref name="value"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="equalityComparer"/> is null.</exception>
        public static bool Is<T>(this Maybe<T> maybe, T value, IEqualityComparer<T> equalityComparer) => maybe
            .Is(() => value, equalityComparer);

        /// <summary>
        /// Checks whether produced value of the given <paramref name="valueSelector"/> is equal
        /// to the value of the maybe. The equality check will be performed with the standard
        /// equality comparer for the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <param name="valueSelector">The function producing the value to compare with.</param>
        /// <returns>
        /// True if the value produced by the given <paramref name="valueSelector"/> is equal to the value of the
        /// <paramref name="maybe"/>. Otherwise false if either the maybe contains nothing or that the value of 
        /// the maybe isn't equal to the produced value.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="valueSelector"/> is null.</exception>
        public static bool Is<T>(this Maybe<T> maybe, Func<T> valueSelector) => maybe
            .Is(valueSelector, EqualityComparer<T>.Default);

        /// <summary>
        /// Checks whether produced value of the given <paramref name="valueSelector"/> is equal
        /// to the value of the maybe. The equality check will be performed with the 
        /// given <paramref name="equalityComparer"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that contains either a value or nothing.</param>
        /// <param name="valueSelector">The function producing the value to compare with.</param>
        /// <param name="equalityComparer">The equality comparer to compare values with.</param>
        /// <returns>
        /// True if the value produced by the given <paramref name="valueSelector"/> is equal to the value of the
        /// <paramref name="maybe"/>. Otherwise false if either the maybe contains nothing or that the value of 
        /// the maybe isn't equal to the produced value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If either the given <paramref name="valueSelector"/> is null or if the given <paramref name="equalityComparer"/>
        /// is null.
        /// </exception>
        public static bool Is<T>(this Maybe<T> maybe, Func<T> valueSelector, IEqualityComparer<T> equalityComparer)
        {
            if (valueSelector == null)
            {
                throw new ArgumentNullException(nameof(valueSelector));
            }

            if (equalityComparer == null)
            {
                throw new ArgumentNullException(nameof(equalityComparer));
            }

            return maybe.Map(() => false, v => equalityComparer.Equals(v, valueSelector()));
        }

        /// <summary>
        /// Flattens the given <paramref name="maybe"/>. If the given <paramref name="maybe"/> is Nothing,
        /// Nothing will be returned, otherwise the inner <see cref="Maybe{T}"/> will be returned.
        /// </summary>
        /// <typeparam name="T">The type of the value that the inner maybe is holding.</typeparam>
        /// <param name="maybe">The maybe that is holding either Nothing or another <see cref="Maybe{T}"/>.</param>
        /// <returns>
        /// Either Nothing if the given <paramref name="maybe"/> is Nothing, or the inner <paramref name="maybe"/>.
        /// </returns>
        public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> maybe) => maybe.Coalesce(v => v);

        /// <summary>
        /// Returns a <see cref="Result{T}"/> which is successful if the <paramref name="maybe"/>
        /// represents a value, otherwise the result will be unsuccessful and will contain the errors
        /// produced by the given <paramref name="errorSelector"/>.
        /// A successful result will contain the value of the given <paramref name="maybe"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe to create a <see cref="Result{T}"/> representation of.</param>
        /// <param name="errorSelector">The function producing errors if the maybe represents Nothing.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> which is successful if the maybe represents a value, otherwise
        /// the result will be unsuccessful and will contain the errors produced by the given <paramref name="errorSelector"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="errorSelector"/> is null.</exception>
        public static Result<T> ToResult<T>(this Maybe<T> maybe, Func<IEnumerable<Error>> errorSelector)
        {
            if (errorSelector == null)
            {
                throw new ArgumentNullException(nameof(errorSelector));
            }

            return maybe.Map(() => Result<T>.Failed(errorSelector()?.ToArray()), Result.Create);
        }

        /// <summary>
        /// Returns a <see cref="Result{T}"/> which is successful if the <paramref name="maybe"/>
        /// represents a value, otherwise the result will be unsuccessful and will contain the given <paramref name="errors"/>.
        /// A successful result will contain the value of the given <paramref name="maybe"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value that the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe to create a <see cref="Result{T}"/> representation of.</param>
        /// <param name="errors">The errors describing the unsuccessful result if the maybe represents Nothing.</param>
        /// <returns>
        /// A <see cref="Result{T}"/> which is successful if the maybe represents a value, otherwise
        /// the result will be unsuccessful and will contain the given <paramref name="errors"/>.
        /// </returns>
        public static Result<T> ToResult<T>(this Maybe<T> maybe, params Error[] errors) => maybe
            .ToResult(() => errors);

        /// <summary>
        /// Checks whether the value of the given <paramref name="maybe"/> is valid or not by using
        /// the given <paramref name="predicate"/>.
        /// If the value is valid, the <paramref name="maybe"/> will be returned, otherwise Nothing.
        /// </summary>
        /// <typeparam name="T">The type of the value the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe to check whether its value is valid or not.</param>
        /// <param name="predicate">Function that takes the value of the maybe and returns a flag indicating if the value is valid or not.</param>
        /// <returns>
        /// Nothing if the <paramref name="maybe"/> is either Nothing or that the value isn't valid, otherwise
        /// the given <paramref name="maybe"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="predicate"/> is null.</exception>
        public static Maybe<T> Guard<T>(this Maybe<T> maybe, Func<T, bool> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            return maybe.Coalesce(v => predicate(v) ? maybe : Maybe<T>.Nothing);
        }

        /// <summary>
        /// Checks whether the value of the given <paramref name="maybe"/> is equal to the
        /// <paramref name="expectedValue"/>. If it is, the <paramref name="maybe"/>
        /// will be returned, otherwise Nothing. The default equality comparer for <typeparamref name="T"/>
        /// will be used.
        /// </summary>
        /// <typeparam name="T">The type of the value the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe to check whether its valid is equal to the expected value or not.</param>
        /// <param name="expectedValue">The value that is expected to be equal to the value of the maybe.</param>
        /// <returns>
        /// Nothing if the <paramref name="maybe"/> is either Nothing or that the value isn't equal
        /// to the <paramref name="expectedValue"/>. Otherwise the given <paramref name="maybe"/> will
        /// be returned.
        /// </returns>
        public static Maybe<T> Guard<T>(this Maybe<T> maybe, T expectedValue)
        {
            return maybe.Guard(expectedValue, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Checks whether the value of the given <paramref name="maybe"/> is equal to the
        /// <paramref name="expectedValue"/>. If it is, the <paramref name="maybe"/>
        /// will be returned, otherwise Nothing. The given <paramref name="equalityComparer"/>
        /// will be used to check for equality.
        /// </summary>
        /// <typeparam name="T">The type of the value the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe to check whether its valid is equal to the expected value or not.</param>
        /// <param name="expectedValue">The value that is expected to be equal to the value of the maybe.</param>
        /// <param name="equalityComparer">The equality comparer to use to compare the expected value and the value of the maybe with.</param>
        /// <returns>
        /// Nothing if the <paramref name="maybe"/> is either Nothing or that the value isn't equal
        /// to the <paramref name="expectedValue"/>. Otherwise the given <paramref name="maybe"/> will
        /// be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="equalityComparer"/> is null.</exception>
        public static Maybe<T> Guard<T>(this Maybe<T> maybe, T expectedValue, IEqualityComparer<T> equalityComparer)
        {
            if (equalityComparer == null)
            {
                throw new ArgumentNullException(nameof(equalityComparer));
            }

            return maybe.Guard(() => expectedValue, equalityComparer);
        }

        /// <summary>
        /// Checks whether the value of the given <paramref name="maybe"/> is equal to the
        /// value produced by the given <paramref name="expectedValueSelector"/>. If it is, the <paramref name="maybe"/>
        /// will be returned, otherwise Nothing. The default equality comparer for <typeparamref name="T"/>
        /// will be used.
        /// </summary>
        /// <typeparam name="T">The type of the value the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe to check whether its valid is equal to the expected value or not.</param>
        /// <param name="expectedValueSelector">The function producing the value that is expected to be equal to the value of the maybe.</param>
        /// <returns>
        /// Nothing if the <paramref name="maybe"/> is either Nothing or that the value isn't equal
        /// to the value produced by the given <paramref name="expectedValueSelector"/>.
        /// Otherwise the given <paramref name="maybe"/> will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">If the given <paramref name="expectedValueSelector"/> is null.</exception>
        public static Maybe<T> Guard<T>(this Maybe<T> maybe, Func<T> expectedValueSelector)
        {
            if (expectedValueSelector == null)
            {
                throw new ArgumentNullException(nameof(expectedValueSelector));
            }

            return maybe.Guard(expectedValueSelector, EqualityComparer<T>.Default);
        }

        /// <summary>
        /// Checks whether the value of the given <paramref name="maybe"/> is equal to the
        /// value produced by the given <paramref name="expectedValueSelector"/>. If it is, the <paramref name="maybe"/>
        /// will be returned, otherwise Nothing. The given <paramref name="equalityComparer"/> will
        /// be used to check for equality between the values.
        /// </summary>
        /// <typeparam name="T">The type of the value the maybe is holding.</typeparam>
        /// <param name="maybe">The maybe to check whether its valid is equal to the expected value or not.</param>
        /// <param name="expectedValueSelector">The function producing the value that is expected to be equal to the value of the maybe.</param>
        /// <param name="equalityComparer">The equality comparer to use to compare the expected value and the value of the maybe with.</param>
        /// <returns>
        /// Nothing if the <paramref name="maybe"/> is either Nothing or that the value isn't equal
        /// to the value produced by the given <paramref name="expectedValueSelector"/>.
        /// Otherwise the given <paramref name="maybe"/> will be returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// If the given <paramref name="expectedValueSelector"/> or <paramref name="equalityComparer"/> are null.
        /// </exception>
        public static Maybe<T> Guard<T>(this Maybe<T> maybe, Func<T> expectedValueSelector, IEqualityComparer<T> equalityComparer)
        {
            if (expectedValueSelector == null)
            {
                throw new ArgumentNullException(nameof(expectedValueSelector));
            }

            if (equalityComparer == null)
            {
                throw new ArgumentNullException(nameof(equalityComparer));
            }

            return maybe.Guard(v => equalityComparer.Equals(v, expectedValueSelector()));
        }
    }
}
