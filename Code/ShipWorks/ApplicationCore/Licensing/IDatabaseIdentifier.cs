using System;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for identifying a database
    /// </summary>
    public interface IDatabaseIdentifier
    {
        /// <summary>
        /// Gets the database identifier
        /// </summary>
        Guid Get();
    }
}