using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Interapptive.Shared.Data;
using ShipWorks.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Used to retrieve the database identifier guid from sproc GetDatabaseGuid.
    /// </summary>
    public class DatabaseIdentifier : IDatabaseIdentifier
    {
        /// <summary>
        /// Returns DatabaseGuid of database.
        /// </summary>
        public Guid Get()
        {
            return SqlSession.Current.DatabaseIdentifier;
        }
    }
}