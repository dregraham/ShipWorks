﻿using ShipWorks.Stores.Platforms.Odbc.DataAccess;

namespace ShipWorks.Stores.Platforms.Odbc.Download
{
    /// <summary>
    /// Generates a context specific download query
    /// </summary>
    public interface IOdbcDownloadQuery
    {
        /// <summary>
        /// Generates the Sql to download orders.
        /// </summary>
        string GenerateSql();

        /// <summary>
        /// Adds Command Text to the given sql command
        /// </summary>
        void ConfigureCommand(IShipWorksOdbcCommand command);
    }
}
