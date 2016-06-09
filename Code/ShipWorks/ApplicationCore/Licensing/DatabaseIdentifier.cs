using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Connection;
using System;
using System.Data.SqlClient;
using System.Transactions;

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
                using (new TransactionScope(TransactionScopeOption.Suppress))
                using (SqlAdapter sqlAdapter = new SqlAdapter())
                {
                    RetrievalQuery getDatabaseGuidQuery = new RetrievalQuery(new SqlCommand("exec GetDatabaseGuid"));
                    return (Guid) sqlAdapter.ExecuteScalarQuery(getDatabaseGuidQuery);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseIdentifierException(ex);
            }
        }
    }
}