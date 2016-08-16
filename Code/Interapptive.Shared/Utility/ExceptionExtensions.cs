using System;
using System.Linq;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extensions for handling exceptions
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Returns true if exception is in list of exceptions
        /// </summary>
        public static bool IsExceptionType(this Exception value, params Type[] exceptions)
        {
            return exceptions.Contains(value.GetType());
        }
    }
}
