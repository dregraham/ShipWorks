using System;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Detailed information about a single ShipWorks database
    /// </summary>
    public interface ISqlDatabaseDetail
    {
        /// <summary>
        /// GUID of the database
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Is the database an archive
        /// </summary>
        bool IsArchive { get; }

        /// <summary>
        /// The date of the last order to be downloaded into the database
        /// </summary>
        DateTime LastOrderDate { get; }

        /// <summary>
        /// The last order number to be downloaded into the database
        /// </summary>
        string LastOrderNumber { get; }

        /// <summary>
        /// The last ShipWorks user to log in to the database
        /// </summary>
        string LastUsedBy { get; }

        /// <summary>
        /// The date/time the last ShipWorks user logged in to the database
        /// </summary>
        DateTime LastUsedOn { get; }

        /// <summary>
        /// The name of the database
        /// </summary>
        string Name { get; }

        /// <summary>
        /// ShipWorks schema version of the database
        /// </summary>
        Version SchemaVersion { get; }

        /// <summary>
        /// The status of the database, as it related to ShipWorks
        /// </summary>
        SqlDatabaseStatus Status { get; }

        /// <summary>
        /// The total number of orders in the database
        /// </summary>
        int OrderCount { get;  }

        /// <summary>
        /// The oldest order downloaded into the database
        /// </summary>
        DateTime FirstOrderDate { get; }
    }
}