using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using log4net;
using Interapptive.Shared.Data;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Utility class for dealing with identity columns and their seeds
    /// </summary>
    public static class SqlTableSeedManager
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlTableSeedManager));

        /// <summary>
        /// Get the current collection of seeds from identity columns
        /// </summary>
        public static List<SqlTableSeed> GetSeeds(SqlConnection con)
        {
            List<SqlTableSeed> seeds = new List<SqlTableSeed>();

            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = @"
                select OBJECT_ID(t.name, 'TABLE') as object_id, t.name as table_name, c.seed_value, c.last_value
                  from sys.tables t inner join sys.identity_columns c on c.object_id = t.object_id";

            using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    object lastValue = reader.GetValue(3);

                    seeds.Add(new SqlTableSeed(
                        reader.GetInt32(0),
                        reader.GetString(1),
                        Convert.ToInt64(reader.GetValue(2)),
                        (lastValue is DBNull) ? (long?) null : Convert.ToInt64(lastValue)));
                }
            }

            return seeds;
        }
    }
}
