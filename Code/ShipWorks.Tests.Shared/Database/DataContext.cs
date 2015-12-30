using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Transactions;
using Autofac.Builder;
using Autofac.Extras.Moq;
using ShipWorks.Data.Connection;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Context that can be used to run a data driven test
    /// </summary>
    public class DataContext : IDisposable
    {
        private readonly SqlConnection connection;
        private readonly TransactionScope transactionScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataContext(SqlConnection connection, AutoMock mock)
        {
            this.connection = connection;
            connection.StateChange += StateChanged;
            transactionScope = new TransactionScope();
            mock.Provide<Func<bool, SqlAdapter>>(x => CreateSqlAdapter());
            mock.Container.ComponentRegistry.Register(RegistrationBuilder.ForDelegate((c, p) => connection).ExternallyOwned().CreateRegistration());
        }

        /// <summary>
        /// Create a SQL adapter that is hooked into the current test transaction
        /// </summary>
        public SqlAdapter CreateSqlAdapter() => new SqlAdapter(connection);

        /// <summary>
        /// Listen for when the connection state changes
        /// </summary>
        /// <remarks>This is useful for debugging</remarks>
        private void StateChanged(object sender, StateChangeEventArgs e)
        {
            Debug.WriteLine($"State changed to {e.CurrentState}");
        }

        /// <summary>
        /// Dispose of the context
        /// </summary>
        public void Dispose()
        {
            connection.StateChange -= StateChanged;
            transactionScope.Dispose();
            connection.Dispose();
        }
    }
}
