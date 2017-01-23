using System.Collections.Generic;
using System.Linq;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Char type extension methods
    /// </summary>
    public static class CharExtensions
    {
        /// <summary>
        /// Returns a string representation of the given chars
        /// </summary>
        public static string CreateString(this IEnumerable<char> value) => new string(value.ToArray());
    }
}