using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// OdbcDatasource that inputs and outputs encrypted json.
    /// </summary>
    public class EncryptedOdbcDataSource : OdbcDataSource
    {
        private readonly IEncryptionProvider odbcEncryptionProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EncryptedOdbcDataSource"/> class.
        /// </summary>
        public EncryptedOdbcDataSource(IShipWorksDbProviderFactory odbcProvider, IEncryptionProviderFactory encryptionProviderFactory) : base(odbcProvider)
        {
            odbcEncryptionProvider = encryptionProviderFactory.CreateOdbcEncryptionProvider();
        }

        /// <summary>
        /// Populate the OdbcDataSource using the given encrypted json string
        /// </summary>
        public override void Restore(string encryptedJson)
        {
            try
            {
                base.Restore(odbcEncryptionProvider.Decrypt(encryptedJson));
            }
            catch (EncryptionException ex)
            {
                throw new ShipWorksOdbcException("Failed to restore data source", ex);
            }
        }

        /// <summary>
        /// Serialize and encrypt the OdbcDataSource
        /// </summary>
        public override string Serialize()
        {
            try
            {
                return odbcEncryptionProvider.Encrypt(base.Serialize());
            }
            catch (EncryptionException ex)
            {
                throw new ShipWorksOdbcException("Failed to restore data source", ex);
            }
        }
    }
}