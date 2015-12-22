using System;
using System.Collections.Generic;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Equality comparer that ignores case
    /// </summary>
    public class CaseInsensitiveEqualityComparer : IEqualityComparer<string>
    {
        /// <summary>
        /// Tests equality between two strings
        /// </summary>
        public bool Equals(string x, string y) =>
            string.Equals(x, y, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// Get the hash code of a string
        /// </summary>
        public int GetHashCode(string obj) =>
            obj.ToUpper().GetHashCode();
    }
}
