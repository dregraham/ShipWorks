using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Factory for creating SqlAdapters
    /// </summary>
    [Component]
    public class SqlAdapterFactory : ISqlAdapterFactory
    {
        /// <summary>
        /// Create a SqlAdapter that is not part of a transaction
        /// </summary>
        public ISqlAdapter Create() => SqlAdapter.Create(false);

        /// <summary>
        /// Create a SqlAdapter that uses the existing connection and transaction
        /// </summary>
        public ISqlAdapter Create(DbConnection connection, DbTransaction transaction) => new SqlAdapter(connection, transaction);

        /// <summary>
        /// Create a SqlAdapter that IS part of a transaction
        /// </summary>
        public ISqlAdapter CreateTransacted() => SqlAdapter.Create(true);

        /// <summary>
        /// Execute a block of code using a SqlAdapter that is part of a physical transaction
        /// </summary>
        public async Task<T> WithPhysicalTransactionAsync<T>(Func<ISqlAdapter, Task<T>> withAdapter, [CallerMemberName] string name = "")
        {
            using (DbConnection connection = SqlSession.Current.OpenConnection())
            {
                return await connection.WithTransactionAsync(withAdapter, name).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Create a SqlAdapter that uses the existing connection
        /// </summary>
        public ISqlAdapter Create(DbConnection connection) => new SqlAdapter(connection);
    }
}
