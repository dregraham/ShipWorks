using System;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Used to retrieve the database identifier guid from sproc GetDatabaseGuid.
    /// </summary>
    public class DatabaseIdentifier : IDatabaseIdentifier
    {
        /// <summary>
        /// Returns DatabaseGuid from database.
        /// </summary>
        public Guid Get()
        {
            try
            {
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    return SqlCommandProvider.ExecuteScalar<Guid>(con, "exec GetDatabaseGuid");
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseIdentifierException(ex);
            }
        }
    }
}