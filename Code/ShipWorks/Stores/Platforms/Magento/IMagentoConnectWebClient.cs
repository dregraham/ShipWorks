using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Interface for communicating with Magento Connect
    /// </summary>
    public interface IMagentoConnectWebClient
    {
        /// <summary>
        /// Get the module and capabilities information from the module
        /// </summary>
        GenericModuleResponse GetModule();
    }
}