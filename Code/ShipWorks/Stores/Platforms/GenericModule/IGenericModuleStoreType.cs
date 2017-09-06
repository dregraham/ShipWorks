namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Interface that represents the GenericModuleStoreType
    /// </summary>
    /// <remarks>This is needed for unit testing/mocking</remarks>
    public interface IGenericModuleStoreType
    {
        /// <summary>
        /// Initialize a new magento store entity from an online module
        /// </summary>
        void InitializeFromOnlineModule();

        /// <summary>
        /// Creates an instance of the downloader. To be overridden by derived stores if necessary.
        /// </summary>
        IGenericStoreWebClient CreateWebClient();
    }
}