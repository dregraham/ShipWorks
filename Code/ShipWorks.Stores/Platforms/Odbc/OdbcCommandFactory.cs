using Interapptive.Shared.Security;
using ShipWorks.ApplicationCore.Security;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Creates an OdbcCommand for a store
    /// </summary>
    public class OdbcCommandFactory
    {
        private readonly IEncryptionProviderFactory encryptionProviderFactory;
        private readonly IOdbcFieldMap odbcFieldMap;
        private readonly IOdbcDataSource dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCommandFactory"/> class.
        /// </summary>
        public OdbcCommandFactory(
            IEncryptionProviderFactory encryptionProviderFactory,
            IOdbcFieldMap odbcFieldMap,
            IOdbcDataSource dataSource)
        {
            this.encryptionProviderFactory = encryptionProviderFactory;
            this.odbcFieldMap = odbcFieldMap;
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Creates the download command.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store)
        {
            string serializedMap = encryptionProviderFactory.CreateOdbcEncryptionProvider().Decrypt(store.Map);
            odbcFieldMap.Load(serializedMap);
            dataSource.Restore(store.ConnectionString);

            return new OdbcDownloadCommand(odbcFieldMap, dataSource);
        }
    }
}
