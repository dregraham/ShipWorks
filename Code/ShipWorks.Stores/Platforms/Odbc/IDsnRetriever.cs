using System;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Retrieves DSN names
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IDsnRetriever : IDisposable
    {
        /// <summary>
        /// Gets the name of the next DSN - return null of no next DSN
        /// </summary>
        string GetNextDsnName();
    }
}