using ShipWorks.ApplicationCore.ComponentRegistration;

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
        /// Create a SqlAdapter that IS part of a transaction
        /// </summary>
        public ISqlAdapter CreateTransacted() => SqlAdapter.Create(true);
    }
}
