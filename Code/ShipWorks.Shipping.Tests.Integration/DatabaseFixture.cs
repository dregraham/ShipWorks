using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac.Extras.Moq;
using SQL.LocalDB.Test;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration
{
    /// <summary>
    /// Fixture that will create a database that can be used to test against
    /// </summary>
    public class DatabaseFixture : IDisposable
    {
        readonly TempLocalDb db;

        /// <summary>
        /// Constructor
        /// </summary>
        public DatabaseFixture()
        {
            db = new TempLocalDb("ShipWorks");

            using (SqlConnection conn = db.Open())
            {
                using (SqlCommand command = conn.CreateCommand())
                {
                    command.CommandText = @"ALTER DATABASE ShipWorks
  SET CHANGE_TRACKING = ON
  (CHANGE_RETENTION = 1 DAYS, AUTO_CLEANUP = ON)";
                    command.ExecuteNonQuery();

                    using (Stream stream = Assembly.Load("ShipWorks.Core")
                        .GetManifestResourceStream("ShipWorks.Data.Administration.Scripts.Installation.CreateSchema.sql"))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            foreach (string line in CreateSchemaLines(reader))
                            {
                                command.CommandText = line;
                                command.ExecuteNonQuery();
                            }
                        }
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
