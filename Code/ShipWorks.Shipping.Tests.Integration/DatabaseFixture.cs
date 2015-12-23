using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Data;
using ShipWorks.Data.Administration;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Tests.Integration;
using SQL.LocalDB.Test;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration
{
    public class ShipWorksLocalDb : TempLocalDb
    {
        public ShipWorksLocalDb(string databaseName) : base(databaseName)
        {

        }

        public override string ConnectionString =>
            base.ConnectionString +
                    ";Connect Timeout=10;Application Name=ShipWorks;Workstation ID=0000100001;Transaction Binding=\"Explicit Unbind\"";
    }

    /// <summary>
    /// Fixture that will create a database that can be used to test against
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
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
                        EntityBuilder.Create<UserEntity>().WithDefaults().Save(sqlAdapter);
                        EntityBuilder.Create<ComputerEntity>().WithDefaults().Save(sqlAdapter);
                    }
                }

            }
        }

        internal DataContext CreateDataContext(AutoMock mock)
        {
            return new DataContext(db.Open(), mock);
        }

        private IEnumerable<string> CreateSchemaLines(StreamReader reader)
        {
            return reader.ReadToEnd()
                .Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !x.StartsWith("PRINT"))
                .Where(x => !string.IsNullOrWhiteSpace(x));
        }

        public void Dispose() => db.Dispose();
    }

    [CollectionDefinition("Database collection")]
    public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
