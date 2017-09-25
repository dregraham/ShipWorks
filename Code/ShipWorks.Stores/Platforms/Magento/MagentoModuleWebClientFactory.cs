using System;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Magento module web client factory
    /// </summary>
    [Component]
    public class MagentoModuleWebClientFactory : IMagentoModuleWebClientFactory
    {
        readonly Func<MagentoStoreEntity, IMagentoTwoWebClient> createV2WebClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public MagentoModuleWebClientFactory(Func<MagentoStoreEntity, IMagentoTwoWebClient> createV2WebClient)
        {
            this.createV2WebClient = createV2WebClient;
        }

        /// <summary>
        /// Create a web client for the given store
        /// </summary>
        public IGenericStoreWebClient Create(MagentoStoreEntity store)
        {
            MethodConditions.EnsureArgumentIsNotNull(store, nameof(store));

            switch ((MagentoVersion) store.MagentoVersion)
            {
                case MagentoVersion.PhpFile:
                    return new MagentoWebClient(store);

                case MagentoVersion.MagentoConnect:
                    // for connecting to our Magento Connect Extension via SOAP
                    return new MagentoConnectWebClient(store);

                case MagentoVersion.MagentoTwo:
                    return createV2WebClient(store);

                default:
                    throw new NotImplementedException("Magento Version not supported");
            }
        }
    }
}
