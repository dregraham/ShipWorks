using Interapptive.Shared;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Web client for Magento, extends the generic client to add Action functionality
    /// </summary>
    public interface IMagentoWebClient : IGenericStoreWebClient
    {
        /// <summary>
        /// Executes an action on an order and returns the new order status
        /// </summary>
        string ExecuteAction(MagentoUploadAction action);
    }
}