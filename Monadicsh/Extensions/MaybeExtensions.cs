using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadicsh.Extensions
{
    public static class MaybeExtensions
    {
        /// <summary>
        /// Returns either the given <paramref name="defaultValue"/> if the maybe is representing
        /// nothing, or the value of the <paramref name="maybe"/>.
        /// </summary>
        /// <typeparam name="T">The type the maybe is encapsulating.</typeparam>
        /// <param name="maybe">The maybe containing either nothing or just a value.</param>
        /// <param name="defaultValue">The default value to return if the maybe is representing nothing.</param>
        /// <returns>
        /// Either the given <paramref name="defaultValue"/> if the maybe is representing
        /// nothing, or the value of the <paramref name="maybe"/>.
        /// </returns>
        public static T Or<T>(this Maybe<T> maybe, T defaultValue) => maybe.Or(() => defaultValue);

        /// <summary>
        /// Returns either the result of the given <paramref name="defaultValue"/> if the maybe is representing
        /// nothing, or the value of the <paramref name="maybe"/>.
        /// </summary>
        /// <typeparam name="T">The type the maybe is encapsulating.</typeparam>
        /// <param name="maybe">The maybe containing either nothing or just a value.</param>
        /// <param name="defaultValue">The default value to return if the maybe is representing nothing.</param>
        /// <returns>
        /// Either the result of the given <paramref name="defaultValue"/> if the maybe is representing
        /// nothing, or the value of the <paramref name="maybe"/>.
        /// </returns>
        public static T Or<T>(this Maybe<T> maybe, Func<T> defaultValue) => maybe.Map(defaultValue, v => v);

        /// <summary>
        /// Returns either the value of the maybe mapped to the new type <typeparamref name="T2"/> or
        /// the produced default value iff the maybe represents Nothing.
        /// </summary>
        /// <typeparam name="T1">The type the maybe is currently wrapping.</typeparam>
        /// <typeparam name="T2">The type that the value of the maybe should be mapped to.</typeparam>
        /// <param name="maybe">The maybe containing either a value or Nothing.</param>
        /// <param name="defaultValue">The function producing the default value if the maybe represents Nothing.</param>
        /// <param name="map">The function mapping the value of the maybe if the maybe contains a value.</param>
        /// <returns>
        /// A value of type <typeparamref name="T2"/> either mapped from the value of the maybe, or the produced
        /// default value iff the maybe represents Nothing.
        /// </returns>
        public static T2 Map<T1, T2>(this Maybe<T1> maybe, Func<T2> defaultValue, Func<T1, T2> map)
        {
            return maybe.IsNothing
                ? defaultValue()
                : map(maybe.Value);
        }

        /// <summary>
        /// Returns either the value of the maybe mapped to the new type <typeparamref name="T2"/> or
        /// the given <paramref name="defaultValue"/> iff the maybe represents Nothing.
        /// </summary>
        /// <typeparam name="T1">The type the maybe is currently wrapping.</typeparam>
        /// <typeparam name="T2">The type that the value of the maybe should be mapped to.</typeparam>
        /// <param name="maybe">The maybe containing either a value or Nothing.</param>
        /// <param name="defaultValue">The default value to return if the maybe represents Nothing.</param>
        /// <param name="map">The function mapping the value of the maybe if the maybe contains a value.</param>
        /// <returns>
        /// A value of type <typeparamref name="T2"/> either mapped from the value of the maybe, or the
        /// given <paramref name="defaultValue"/> iff the maybe represents Nothing.
        /// </returns>
        public static T2 Map<T1, T2>(this Maybe<T1> maybe, T2 defaultValue, Func<T1, T2> map) => maybe
            .Map(() => defaultValue, map);

        /// <summary>
        /// Returns the value of the given <paramref name="maybe"/> or throws
        /// the exception produced by the given func iff the maybe represents Nothing.
        /// </summary>
        /// <typeparam name="T">The type the maybe is wrapping.</typeparam>
        /// <param name="maybe">The maybe that either contains a value or nothing.</param>
        /// <param name="exception">The func producing the exception that will be thrown if the Maybe represents Nothing.</param>
        /// <returns>The value of the maybe iff the maybe contains a value, otherwise the exception produced by the func will be thrown.</returns>
        public static T OrThrow<T>(this Maybe<T> maybe, Func<Exception> exception) =>
            maybe.Or(() => throw exception());

        public static Either<T1, T2> Either<T1, T2>(this Maybe<T1> maybe, Func<T2> defaultValue) =>
            maybe.Map(() => new Either<T1, T2>(defaultValue()), v => v);

        /// <summary>
        /// Converts the given <paramref name="maybe"/> to an either that either
        /// contains the value of the maybe, or the given <paramref name="defaultValue"/> iff
        /// the maybe represents Nothing.
        /// </summary>
        /// <typeparam name="T1">The type of the left side of the either.</typeparam>
        /// <typeparam name="T2">The type of the right side of the either.</typeparam>
        /// <param name="maybe">The maybe that may contain a value.</param>
        /// <param name="defaultValue">The default value to use if the maybe is Nothing.</param>
        /// <returns>
        /// An either containing a left value if the maybe has a value, otherwise the either
        /// will contain a right value where the value is the given <paramref name="defaultValue"/>.
        /// </returns>
        public static Either<T1, T2> Either<T1, T2>(this Maybe<T1> maybe, T2 defaultValue) =>
            maybe.Either(() => defaultValue);

        /// <summary>
        /// Performs and returns the value produced by the given <paramref name="func"/> iff the given <paramref name="maybe"/>
        /// contains a value, otherwise Nothing will be returned.
        /// </summary>
        public static Maybe<T2> Coalesce<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> func) => maybe
            .Map(() => Maybe<T2>.Nothing, func);

        public static Maybe<T2> Coalesce<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func) => maybe
            .Coalesce(t => Maybe.Create(func(t)));

        /// <summary>
        /// Converts the given <paramref name="maybe"/> to a <see cref="Nullable{T}"/>.
        /// If the <paramref name="maybe"/> is representing a Nothing, null will be returned,
        /// otherwise the value of the <paramref name="maybe"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value the maybe is wrapping.</typeparam>
        /// <param name="maybe">The maybe T or nothing.</param>
        /// <returns>A <see cref="Nullable{T}"/> of <see cref="T"/>.</returns>
        public static T? ToNullable<T>(this Maybe<T> maybe) where T : struct
        {
            return maybe.Map(() => default(T?), v => v);
        }

        /// <summary>
        /// Returns a collection of type <typeparamref name="T"/> extracted from the maybes that contains
        /// a value.
        /// </summary>
        /// <typeparam name="T">The type the maybes wrap.</typeparam>
        /// <param name="maybes">The collection of maybes that will have their values extracted, if a value exists.</param>
        /// <returns>
        /// A collection of zero or more values of type <typeparamref name="T"/> that are extracted from the collection of
        /// maybes where the maybe must contain a value.
        /// </returns>
        public static IEnumerable<T> CatMaybes<T>(this IEnumerable<Maybe<T>> maybes) => maybes
            .Where(m => m.IsJust)
            .Select(m => m.Value);

        public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> maybe) => maybe
            .Coalesce(v => v);
    }
}
