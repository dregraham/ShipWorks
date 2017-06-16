using System;
using Autofac;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.Compatibility
{
    /// <summary>
    /// Check to see if a specific store is compatible with Magento Two Rest
    /// </summary>
    [KeyedComponent(typeof(IMagentoProbe), MagentoVersion.MagentoTwoREST)]
    public class MagentoTwoRestProbe : IMagentoProbe
    {
        private readonly Func<MagentoStoreEntity, IMagentoTwoRestClient> webClientFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="MagentoTwoRestProbe"/> class.
        /// </summary>
        public MagentoTwoRestProbe(Func<MagentoStoreEntity, IMagentoTwoRestClient> webClientFactory)
        {
            this.webClientFactory = webClientFactory;
        }

        /// <summary>
        /// Check to see if the store is compatible with Magento 1
        /// </summary>
        public GenericResult<Uri> FindCompatibleUrl(MagentoStoreEntity store)
        {
            try
            {
                IMagentoTwoRestClient client = webClientFactory(store);
                client.GetToken();
                return GenericResult.FromSuccess(new Uri(store.ModuleUrl));
            }
            catch (UriFormatException)
            {
                return GenericResult.FromError<Uri>("Url not in a valid format.");
            }
            catch (MagentoException ex)
            {
                return GenericResult.FromError<Uri>($"An error occurred while attempting to connect to Magento. {ex.Message}");
            }
        }
    }
}
