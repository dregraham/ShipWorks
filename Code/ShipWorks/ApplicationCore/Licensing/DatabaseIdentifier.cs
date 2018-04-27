using System;
using System.Data.Common;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Data;
using ShipWorks.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Used to retrieve the database identifier guid from sproc GetDatabaseGuid.
    /// </summary>
    [Component(SingleInstance = true)]
    public class DatabaseIdentifier : IDatabaseIdentifier
    {
        private Guid databaseId = Guid.Empty;

        /// <summary>
        /// Returns DatabaseGuid of database.
        /// </summary>
        public Guid Get()
        {
            if (databaseId == Guid.Empty)
            {
                databaseId = GetDatabaseId();
            }

            return databaseId;
        }

        /// <summary>
        /// Returns DatabaseGuid of database.
        /// </summary>
        private static Guid GetDatabaseId()
        {
            try
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    return DbCommandProvider.ExecuteScalar<Guid>(con, "exec GetDatabaseGuid");
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseIdentifierException(ex);
            }
        }

        /// <summary>
        /// Reset the cached databaseId
        /// </summary>
        public void Reset()
        {
            databaseId = Guid.Empty;
        }
    }
}