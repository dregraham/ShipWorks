using Interapptive.Shared.Security;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using System;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// Creates an OdbcCommand for a store
    /// </summary>
    public class OdbcCommandFactory
    {
        private readonly IOdbcFieldMap odbcFieldMap;
        private readonly IOdbcDataSource dataSource;
        private readonly Func<Type, ILog> logFactory;
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcCommandFactory"/> class.
        /// </summary>
        public OdbcCommandFactory(IOdbcFieldMap odbcFieldMap, IOdbcDataSource dataSource, Func<Type, ILog> logFactory, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.odbcFieldMap = odbcFieldMap;
            this.dataSource = dataSource;
            this.logFactory = logFactory;
            encryptionProvider = encryptionProviderFactory.CreateOdbcEncryptionProvider();
        }

        /// <summary>
        /// Creates the download command.
        /// </summary>
        public IOdbcCommand CreateDownloadCommand(OdbcStoreEntity store)
        {
            odbcFieldMap.Load(store.Map);
            dataSource.Restore(encryptionProvider.Decrypt(store.ConnectionString));

            return new OdbcDownloadCommand(odbcFieldMap, dataSource, logFactory(typeof(OdbcDownloadCommand)));
        }
    }
}
