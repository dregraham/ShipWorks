using System;
using System.Data.SqlClient;
using Autofac.Extras.Moq;
using ShipWorks.Data.Connection;

namespace ShipWorks.Shipping.Tests.Integration
{
    /// <summary>
    /// Context that can be used to run a data driven test
    /// </summary>
    public class DataContext : IDisposable
    {
        private readonly SqlConnection connection;
        private readonly SqlTransaction transaction;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataContext(SqlConnection connection, AutoMock mock)
        {
            this.connection = connection;
            transaction = connection.BeginTransaction();

            mock.Provide<Func<bool, SqlAdapter>>(x => new SqlAdapter(connection, transaction));
        }

        /// <summary>
        /// Create a SQL adapter that is hooked into the current test transaction
        /// </summary>
        public SqlAdapter CreateSqlAdapter() => new SqlAdapter(connection, transaction);

        /// <summary>
        /// Dispose of the context
        /// </summary>
        public void Dispose()
        {
            transaction.Dispose();
            connection.Dispose();
        }
    }
}
