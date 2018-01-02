using System;
using System.Data.Common;
using Interapptive.Shared.Data;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Tests.Integration.Helpers
{
    /// <summary>
    /// Helper methods for database
    /// </summary>
    public static class DbHelpers
    {
        /// <summary>
        /// Test a query by passing in SQL, configuring its parameters, and then testing the results in the reader
        /// </summary>
        /// <param name="sql">Sql to test</param>
        /// <param name="configureParameters">Action that configures the command's parameters</param>
        /// <param name="performTests">Action that performs the tests on a data reader</param>
        public static void TestQuery(string sql, Action<DbCommand> configureParameters, Action<DbDataReader> performTests)
        {
            using (DbConnection con = SqlSession.Current.OpenConnection())
            {
                using (DbCommand command = DbCommandProvider.Create(con, sql))
                {
                    configureParameters(command);

                    using (var reader = command.ExecuteReader())
                    {
                        performTests(reader);
                    }
                }
            }
        }
    }
}
