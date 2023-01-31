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
        public TempIntegrationDB(string databaseName, string dataSource = @"localhost")
            : this(databaseName, dataSource, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TempIntegrationDB"/> class.
        /// It does NOT delete the existing db and it does NOT create a new db.
        /// </summary>
        public TempIntegrationDB(string databaseName, string dataSource = @"localhost", bool recreateDb = true)
            : base(databaseName, dataSource)
        {
            if (recreateDb)
            {
                this.DeleteDatabase();
                this.CreateDatabase();
            }
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
