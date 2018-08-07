using System;
using ShipWorks.Data.Connection;
using ShipWorks.Startup;
using ShipWorks.Tests.Shared.Database;
using Xunit;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Data.Common;

namespace ShipWorks.Core.Tests.Integration.Data
{
    [Collection("Database collection")]
    [Trait("Category", "ContinuousIntegration")]
    public class EntitySeedTest
    {
        private readonly DataContext context;

        public EntitySeedTest(DatabaseFixture db)
        {
            context = db.CreateDataContext(x => ContainerInitializer.Initialize(x));
        }

        [Fact]
        public void NoDuplicateIdentitySeedsExist()
        {
            List<(int Seed, string TableName)> identitySeeds = new List<(int Seed, string TableName)>();

            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                DbCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $@"
                    SELECT
                        CONVERT(INT, IDENT_SEED(TABLE_SCHEMA + '.' + TABLE_NAME)) AS Seed,
	                    TABLE_NAME AS [Table]
                    FROM INFORMATION_SCHEMA.TABLES
                    WHERE OBJECTPROPERTY(OBJECT_ID(TABLE_SCHEMA + '.' + TABLE_NAME), 'TableHasIdentity') = 1
                      AND IDENT_SEED(TABLE_SCHEMA + '.' + TABLE_NAME) != 1
                    ORDER BY IDENT_SEED(TABLE_SCHEMA + '.' + TABLE_NAME)";

                // See if there is a count to take
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        identitySeeds.Add((reader.GetInt32(0), reader.GetString(1)));
                    }
                }
            }

            var dups = identitySeeds.GroupBy(s => s.Seed)
                .Where(g => g.Count() > 1);

            Assert.False(dups.Any(), string.Join(", ", dups.Select(g => $"{String.Join(",", g)}")));
        }
    }
}
