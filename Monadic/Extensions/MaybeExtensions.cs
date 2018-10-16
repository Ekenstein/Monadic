using System;
using System.Collections.Generic;
using System.Linq;

namespace Monadic.Extensions
{
    public static class MaybeExtensions
    {
        public static T Maybe<T>(this Maybe<T> maybe, T defaultValue) => maybe.IsNothing
            ? defaultValue
            : maybe.Value;

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

        public static Maybe<T2> Coalesce<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> func)
        {
            return maybe.IsNothing ? Monadic.Maybe<T2>.Nothing : func(maybe.Value);
        }

        /// <summary>
        /// Converts the given <paramref name="maybe"/> to a <see cref="Nullable{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value the maybe is wrapping.</typeparam>
        /// <param name="maybe">The maybe T or nothing.</param>
        /// <returns>A <see cref="Nullable{T}"/> of <see cref="T"/>.</returns>
        public static T? ToNullable<T>(this Maybe<T> maybe) where T : struct
        {
            return maybe.FromMaybe<T, T?>(null, v => v);
        }

        public static IEnumerable<T> CatMaybes<T>(this IEnumerable<Maybe<T>> maybes) => maybes
            .Where(m => m.IsJust)
            .Select(m => m.Value);
    }
}
