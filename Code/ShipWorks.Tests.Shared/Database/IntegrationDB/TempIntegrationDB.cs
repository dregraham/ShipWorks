using System;

namespace ShipWorks.Tests.Shared.Database.IntegrationDB
{
    /// <summary>
    /// Temporary database to be used for integration tests
    /// </summary>
    public class TempIntegrationDB : IntegrationDB, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TempIntegrationDB"/> class.
        /// </summary>
        public TempIntegrationDB(string databaseName, string dataSource = @"(localdb)\v11.0", string username = "sa", string password = "ShipW@rks1")
            : base(databaseName, dataSource)
        {
            this.DeleteDatabase();
            this.CreateDatabase();
        }

        /// <summary>
        /// Deletes the database.
        /// </summary>
        public void Dispose()
        {
            this.DeleteDatabase();
        }
    }
}
