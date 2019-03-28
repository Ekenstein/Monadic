using System;
using System.Collections.Generic;
using System.Text;

namespace Monadicsh
{
    /// <summary>
    /// Represents an overloaded string.
    /// </summary>
    public interface IString
    {
        /// <summary>
        /// The original string value. Can be null.
        /// </summary>
        string OriginalValue { get; }

        /// <summary>
        /// The <see cref="string"/> representation of the <see cref="IString"/>.
        /// </summary>
        string GetValue();
    }
}
