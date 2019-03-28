using System;
using System.Collections.Generic;
using System.Text;

namespace Monadicsh.Extensions
{
    /// <summary>
    /// Provides a static set of extensions for types that inherits <see cref="IString"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Maps the string representation of the given instance of <see cref="IString"/> to a new instance
        /// of the same type.
        /// </summary>
        public static T Map<T>(this IString s, Func<string, T> map) where T : IString
        {
            if (s == null)
            {
                throw new ArgumentNullException(nameof(s));
            }

            return map(s.GetValue());
        }
    }
}
