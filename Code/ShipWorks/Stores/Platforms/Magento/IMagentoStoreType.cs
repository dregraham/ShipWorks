namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Interface that represents the Magento Store Type
    /// </summary>
    /// <remarks>This is needed for unit testing/mocking</remarks>
    public interface IMagentoStoreType
    {
        /// <summary>
        /// Initialize a new magento store entity from an online module
        /// </summary>
        void InitializeFromOnlineModule();
    }
}