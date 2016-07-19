using Interapptive.Shared.Security;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using System.Security.Cryptography;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Reads the license information from the database
    /// </summary>
    public class CustomerLicenseReader : ICustomerLicenseReader
    {
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomerLicenseReader(IEncryptionProviderFactory encryptionProviderFactory)
        {
            encryptionProvider = encryptionProviderFactory.CreateLicenseEncryptionProvider();
        }

        /// <summary>
        /// Reads this License
        /// </summary>
        public string Read()
        {
            ConfigurationEntity config = ConfigurationData.Fetch();

            try
            {
                return encryptionProvider.Decrypt(config.CustomerKey);
            }
            catch (ORMEntityOutOfSyncException)
            {
                // This exception was rarely thrown by config.CustomerKey, but it did happen. Hopefully this will 
                // keep it from hapenning again... I could also putting some sort of sleep in here (a couple of seconds)
                // but we'll try this first.
                return RefreshConfigAndRetryRead();
            }
            catch (EncryptionException ex) when(ex.InnerException is CryptographicException)
            {
                // This was hapenning when the key and the databaseid didn't match. This woulde happen when switching 
                // or restoring a database and shouldn't happen.
                // I don't know if it is hapenning anymore...
                return RefreshConfigAndRetryRead();
            }
        }

        /// <summary>
        /// Retries the read.
        /// </summary>
        private string RefreshConfigAndRetryRead()
        {
            ConfigurationData.CheckForChangesNeeded();
            ConfigurationEntity config = ConfigurationData.Fetch();

            return encryptionProvider.Decrypt(config.CustomerKey);
        }
    }
}
