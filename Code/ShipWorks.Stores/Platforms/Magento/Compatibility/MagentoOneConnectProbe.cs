using System;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;

namespace ShipWorks.Stores.Platforms.Magento.Compatibility
{
    /// <summary>
    /// Checks if Magento is a Magento One Connect store.
    /// </summary>
    [KeyedComponent(typeof(IMagentoProbe), MagentoVersion.MagentoConnect)]
    public class MagentoOneConnectProbe: IMagentoProbe
    {
        private readonly Func<MagentoStoreEntity, MagentoConnectWebClient> webClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoOneConnectProbe(Func<MagentoStoreEntity, MagentoConnectWebClient> webClientFactory)
        {
            this.webClientFactory = webClientFactory;
        }

        /// <summary>
        /// Probe the given Magento store to see if it is compatible
        /// </summary>
        public GenericResult<Uri> FindCompatibleUrl(MagentoStoreEntity store)
        {
            store.ModuleUrl = store.ModuleUrl.TrimEnd('/');

            // First try just the given url
            GenericResult<Uri> result = ProbeUrl(store);

            if (!result.Success && store.ModuleUrl.Contains("index.php", StringComparison.InvariantCultureIgnoreCase))
            {
                store.ModuleUrl = store.ModuleUrl.TrimEnd('/').Replace("/index.php", "");
                result = ProbeUrl(store);
            }

            return result;
        }

        /// <summary>
        /// Probe the given url to see if it is a ShipWorks Magento Module
        /// </summary>
        private GenericResult<Uri> ProbeUrl(MagentoStoreEntity store)
        {
            try
            {
                MagentoConnectWebClient webClient = webClientFactory(store);
                webClient.GetModule();

                return GenericResult.FromSuccess(new Uri(store.ModuleUrl));
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<Uri>($"Exception occurred while attempting to connect to ShipWorks Module. {ex.Message}");
            }
        }
    }
}
