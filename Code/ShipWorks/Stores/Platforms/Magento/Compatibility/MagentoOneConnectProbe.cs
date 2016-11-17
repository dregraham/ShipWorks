using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Magento.Enums;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Magento.Compatibility
{
    /// <summary>
    /// Checks if Magento is a Magento One Connect store.
    /// </summary>
    [KeyedComponent(typeof(IMagentoProbe), MagentoVersion.MagentoConnect)]
    public class MagentoOneConnectProbe: IMagentoProbe
    {
        private Func<MagentoStoreEntity, MagentoConnectWebClient> webClientFactory;

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
            try
            {
                MagentoConnectWebClient webClient = webClientFactory(store);
                GenericModuleResponse response = webClient.GetModule();
                return GenericResult.FromSuccess(new Uri(store.ModuleUrl));
            }
            catch (GenericStoreException)
            {
                return GenericResult.FromError("Exception occurred while attempting to connect to ShipWorks Module.", new Uri(store.ModuleUrl));
            }
        }
    }
}
