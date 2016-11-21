using System;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.Compatibility
{
    /// <summary>
    /// Check to see if a specific store is compatible with Magento Two Rest
    /// </summary>
    [KeyedComponent(typeof(IMagentoProbe), MagentoVersion.MagentoTwoREST)]
    public class MagentoTwoRestProbe : IMagentoProbe
    {
        private readonly IMagentoTwoRestClient client;
        private readonly IEncryptionProviderFactory encryptionProviderFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoTwoRestProbe(IMagentoTwoRestClient client, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.client = client;
            this.encryptionProviderFactory = encryptionProviderFactory;
        }

        /// <summary>
        /// Check to see if the store is compatible with Magento 1
        /// </summary>
        public GenericResult<Uri> FindCompatibleUrl(MagentoStoreEntity store)
        {
            try
            {
                string password =
                    encryptionProviderFactory.CreateSecureTextEncryptionProvider(store.ModuleUsername)
                        .Decrypt(store.ModulePassword);

                client.GetToken(new Uri(store.ModuleUrl), store.ModuleUsername, password);
                return GenericResult.FromSuccess(new Uri(store.ModuleUrl));
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<Uri>($"An error occurred while attempting to connect to Magento. {ex.Message}");
            }
        }
    }
}