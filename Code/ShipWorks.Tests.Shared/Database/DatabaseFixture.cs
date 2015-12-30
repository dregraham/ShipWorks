using System;
using System.Data.SqlClient;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using ShipWorks.ApplicationCore.ExecutionMode;
using ShipWorks.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Tests.Shared.EntityBuilders;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Fixture that will create a database that can be used to test against
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        private DataContext currentContext;
        readonly ShipWorksLocalDb db;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseFixture()
        {
            db = new ShipWorksLocalDb("ShipWorks");

            using (SqlConnection conn = db.Open())
            {
                using (new ExistingConnectionScope(conn))
                {
                    SqlUtility.EnableClr(conn);

                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = @"ALTER DATABASE ShipWorks
  SET CHANGE_TRACKING = ON
  (CHANGE_RETENTION = 1 DAYS, AUTO_CLEANUP = ON)";
                        command.ExecuteNonQuery();
                    }

                    ShipWorksDatabaseUtility.CreateSchemaAndData(() => db.Open(), inTransaction => new SqlAdapter(db.Open()));

                    using (SqlAdapter sqlAdapter = new SqlAdapter(conn))
                    {
                        Create.Entity<UserEntity>().Save(sqlAdapter);
                        Create.Entity<ComputerEntity>().Save(sqlAdapter);
                    }
                }
            }

            DataProvider.InitializeForApplication(new TestExecutionMode());
        }

        /// <summary>
        /// Create a data context for use in a test
        /// </summary>
        /// <remarks>When this context is disposed, everything created inside it is rolled back.  Further,
        /// calling this method again will dispose the previous context.  This is because when a test results
        /// in an exception, the context may not be disposed properly in the test itself.</remarks>
        public DataContext CreateDataContext(AutoMock mock)
        {
            currentContext?.Dispose();
            currentContext = new DataContext(db.Open(), mock);
            return currentContext;
        }

        /// <summary>
        /// Drop the database
        /// </summary>
        public void Dispose()
        {
            currentContext?.Dispose();
            db.Dispose();
        }
    }
}
