using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Magento module web client factory
    /// </summary>
    public interface IMagentoModuleWebClientFactory
    {
        /// <summary>
        /// Create a web client for the given store
        /// </summary>
        IGenericStoreWebClient Create(MagentoStoreEntity store);
    }
}