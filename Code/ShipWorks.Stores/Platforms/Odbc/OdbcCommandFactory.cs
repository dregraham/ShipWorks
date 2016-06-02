using Interapptive.Shared.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Creates an OdbcCommand for a store
    /// </summary>
    public class OdbcCommandFactory
    {
        private readonly IOdbcFieldMap odbcFieldMap;
        private readonly IOdbcDataSource dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCommandFactory"/> class.
        /// </summary>
        public OdbcCommandFactory(
            IOdbcFieldMap odbcFieldMap,
            IOdbcDataSource dataSource)
        {
            this.odbcFieldMap = odbcFieldMap;
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Creates the download command.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store)
        {
            odbcFieldMap.Load(store.Map);
            dataSource.Restore(store.ConnectionString);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource);
        }
    }
}
