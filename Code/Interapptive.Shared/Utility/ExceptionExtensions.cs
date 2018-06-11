using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using log4net;

namespace Interapptive.Shared.Utility
{
    /// <summary>
    /// Extensions for handling exceptions
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Determines if the given exception is or has 
        /// </summary>
        /// <typeparam name="T">The type of exception to check for</typeparam>
        /// <param name="value">The exception to compare against</param>
        /// <returns>True if value is of type T or if any of the inner exceptions are of type T</returns>
        public static bool HasExceptionType<T>(this Exception value) where T : Exception => 
            value != null && 
            (value is T || value.GetAllExceptions().OfType<T>().Any());

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
        /// Checks to see if exceptions are read only database exceptions
        /// </summary>
        public static bool IsReadonlyDatabaseException(this Exception ex, ILog log)
        {
            if (ex.GetAllExceptions().OfType<SqlException>().Any(
                x => x.State == 254 && x.Message.Contains("This ShipWorks database is in read only mode.", StringComparison.InvariantCultureIgnoreCase)))
            {
                log.Error("This ShipWorks database is in read only mode", ex);
                return true;
            }
            return false;
        }
    }
}
