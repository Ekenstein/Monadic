using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadic.Extensions
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
        public static T Maybe<T>(this Maybe<T> maybe, T defaultValue) => maybe.IsNothing
            ? defaultValue
            : maybe.Value;

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
        public static T Maybe<T>(this Maybe<T> maybe, Func<T> defaultValue) => maybe.IsNothing
            ? defaultValue()
            : maybe.Value;

        public static T2 FromMaybe<T1, T2>(this Maybe<T1> maybe, T2 defaultValue, Func<T1, T2> func)
        {
            return maybe.IsNothing
                ? defaultValue
                : func(maybe.Value);
        }

        public static T OrThrow<T>(this Maybe<T> maybe, Func<Exception> exception) =>
            maybe.Maybe(() => throw exception());

        public static Either<T1, T2> Either<T1, T2>(this Maybe<T1> maybe, Func<T2> defaultValue) =>
            maybe.FromMaybe(new Either<T1, T2>(defaultValue()), v => v);

        public static Either<T1, T2> Either<T1, T2>(this Maybe<T1> maybe, T2 defaultValue) =>
            maybe.FromMaybe(new Either<T1, T2>(defaultValue), v => v);

        /// <summary>
        /// Performs and returns the value produced by the given <paramref name="func"/> iff the given <paramref name="maybe"/>
        /// contains a value, otherwise Nothing will be returned.
        /// </summary>
        public static Maybe<T2> Coalesce<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> func) => maybe
            .FromMaybe(Monadic.Maybe<T2>.Nothing, func);

        public static Maybe<T2> Coalesce<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> transform) => maybe
            .Coalesce(t => Monadic.Maybe.Create(transform(t)));

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
            return maybe.FromMaybe<T, T?>(null, v => v);
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
    }
}
