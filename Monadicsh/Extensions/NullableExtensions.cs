using System;
using System.Collections.Generic;

namespace Monadicsh.Extensions
{
    /// <summary>
    /// A static set of functions for <see cref="Nullable{T}"/>
    /// </summary>
    public static class NullableExtensions
    {
        /// <summary>
        /// Converts the given nullable <paramref name="value"/> to a <see cref="Maybe{T}"/>.
        /// If the <paramref name="value"/> is null, <see cref="Maybe{T}.Nothing"/> will be returned,
        /// otherwise <see cref="Maybe{T}.Just(T)"/>.
        /// </summary>
        /// <typeparam name="T">The type of value that the maybe should be holding.</typeparam>
        /// <param name="value">The nullable value that should be transformed into a <see cref="Maybe{T}"/>.</param>
        /// <returns>
        /// A <see cref="Maybe{T}"/> representation of the given nullable <paramref name="value"/>.
        /// If the <paramref name="value"/> is null, Nothing will be returned, otherwise Just the value.
        /// </returns>
        public static Maybe<T> AsMaybe<T>(this T? value) where T : struct => Maybe.Create(value);

        /// <summary>
        /// Converts the given nullable <paramref name="value"/> to an <see cref="IEnumerable{T}"/>.
        /// If the <paramref name="value"/> is null, an empty collection will be returned, otherwise
        /// a collection containing the given <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">The type of value that the nullable is holding.</typeparam>
        /// <param name="value">The nullable value to transform to an <see cref="IEnumerable{T}"/>.</param>
        /// <returns>
        /// An <see cref="IEnumerable{T}"/> containing zero or exactly one element which is the extracted <paramref name="value"/>.
        /// </returns>
        public static IEnumerable<T> AsEnumerable<T>(this T? value) where T : struct => value
            .AsMaybe()
            .AsEnumerable();
    }
}
