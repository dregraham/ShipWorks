using Interapptive.Shared.Security;

namespace ShipWorks.Stores.Platforms.Odbc
{
    public class EncryptedOdbcDataSource : OdbcDataSource
    {
        private readonly IEncryptionProvider odbcEncryptionProvider;

        public EncryptedOdbcDataSource(IShipWorksDbProviderFactory odbcProvider, IEncryptionProviderFactory encryptionProviderFactory) : base(odbcProvider)
        {
            odbcEncryptionProvider = encryptionProviderFactory.CreateOdbcEncryptionProvider();
        }

        /// <summary>
        /// Populate the OdbcDataSource using the given encrypted json string
        /// </summary>
        public override void Restore(string encryptedJson)
        {
            base.Restore(odbcEncryptionProvider.Decrypt(encryptedJson));
        }

        /// <summary>
        /// Serialize and encrypt the OdbcDataSource
        /// </summary>
        public override string Serialize()
        {
            return odbcEncryptionProvider.Encrypt(base.Serialize());
        }
    }
}