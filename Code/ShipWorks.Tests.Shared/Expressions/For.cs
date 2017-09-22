using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Tests.Shared.Expressions
{
    /// <summary>
    /// Test helpers for doing type checking, etc.
    /// </summary>
    /// <remarks>
    /// The name is to help the operations read a bit cleaner
    /// </remarks>
    public static class For
    {
        /// <summary>
        /// Get a class helper that we can use for testing various things
        /// </summary>
        public static ClassHelper<T> Class<T>() =>
            new ClassHelper<T>();
    }
}
