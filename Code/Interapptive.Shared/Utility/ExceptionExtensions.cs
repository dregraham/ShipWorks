using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Return a list of all exceptions/inner exceptions down the line
        /// </summary>
        public static IEnumerable<Exception> GetAllExceptions(this Exception ex)
        {
            Exception currentEx = ex;
            yield return currentEx;
            while (currentEx.InnerException != null)
            {
                currentEx = currentEx.InnerException;
                yield return currentEx;
            }
        }

        /// <summary>
        /// Find the deepest inner exceptions message
        /// </summary>
        public static Exception GetInnermostException(this Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }

            return ex;
        }
    }
}
