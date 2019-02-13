using System;

namespace Monadicsh.Extensions
{
    /// <summary>
    /// Provides a static set of functions for <see cref="CaseInsensitive"/>.
    /// </summary>
    public static class CaseInsensitiveExtensions
    {
        /// <summary>
        /// Transforms the original string value but keeps it case insensitive.
        /// </summary>
        /// <param name="ci">The case insensitive string to transform.</param>
        /// <param name="map">The function transforming the original string to a new string.</param>
        /// <returns>
        /// A <see cref="CaseInsensitive"/> representation of the transformed string produced
        /// by the given <paramref name="map"/>.
        /// </returns>
        public static CaseInsensitive Map(this CaseInsensitive ci, Func<string, string> map)
        {
            if (map == null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return new CaseInsensitive(map(ci.Original));
        }
    }
}
