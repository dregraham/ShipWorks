using System.Data.Common;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Factory for creating SqlAdapters
    /// </summary>
    public interface ISqlAdapterFactory
    {
        /// <summary>
        /// Create a SqlAdapter that is not part of a transaction
        /// </summary>
        ISqlAdapter Create();

        /// <summary>
        /// Create a SqlAdapter that IS part of a transaction
        /// </summary>
        ISqlAdapter CreateTransacted();

        /// <summary>
        /// Create a SqlAdatper that uses an existing connection and transaction
        /// </summary>
        ISqlAdapter Create(DbConnection connection, DbTransaction transaction);

        /// <summary>
        /// Create a SqlAdatper that uses an existing connection
        /// </summary>
        ISqlAdapter Create(DbConnection connection);
    }
}
