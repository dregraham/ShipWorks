using System.Data.Common;
using System.Transactions;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Data.Model
{
    /// <summary>
    /// Custom behavior on the data access adapter
    /// </summary>
    public partial class DataAccessAdapter
    {
        /// <summary>
        /// Get the persistence information for the given field.
        /// </summary>
        public static IFieldPersistenceInfo GetPersistenceInfo(IEntityField2 field)
        {
            using (TransactionScope suppress = new TransactionScope(TransactionScopeOption.Suppress))
            {
                using (DataAccessAdapter adapter = new DataAccessAdapter())
                {
                    return adapter.GetFieldPersistenceInfo(field);
                }
            }
        }

        /// <summary>
        /// Create a new connection using the specified connection string
        /// </summary>
        public static DbConnection CreateConnection(string connectionString)
        {
            using (DataAccessAdapter adapter = new DataAccessAdapter())
            {
                return adapter.CreateNewPhysicalConnection(connectionString);
            }
        }

        /// <summary>
        /// Get the DbProviderFactory used by LLBLGen
        /// </summary>
        public static DbProviderFactory GetDbProviderFactory()
        {
            using (DataAccessAdapter adapter = new DataAccessAdapter())
            {
                return adapter.GetDbProviderFactoryInstance();
            }
        }

        /// <summary>
        /// Create a new connection using the specified connection string
        /// </summary>
        public static DbDataAdapter CreateDataAdapter(string command, DbConnection con)
        {
            DbCommand dbCommand = con.CreateCommand();
            dbCommand.CommandText = command;
            return CreateDataAdapter(dbCommand);
        }

        /// <summary>
        /// Create a new connection using the specified command
        /// </summary>
        public static DbDataAdapter CreateDataAdapter(DbCommand cmd)
        {
            using (DataAccessAdapter adapter = new DataAccessAdapter())
            {
                DbDataAdapter dbAdapter = GetDbProviderFactory().CreateDataAdapter();
                dbAdapter.SelectCommand = cmd;
                return dbAdapter;
            }
        }
    }
}
