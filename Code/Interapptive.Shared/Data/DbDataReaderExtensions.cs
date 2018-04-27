using System;
using System.Data.Common;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Extension methods for a DbDatareader
    /// </summary>
    public static class DbDataReaderExtensions
    {
        /// <summary>
        /// Get a date time or null
        /// </summary>
        public static DateTime? GetNullableDateTime(this DbDataReader reader, int ordinal) =>
            reader.IsDBNull(ordinal) ? (DateTime?) null : reader.GetDateTime(ordinal);
    }
}
