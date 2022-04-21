using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Interface for an AmazonAccountSettingsViewModel
    /// </summary>
    public interface IAmazonAccountSettingsViewModel
    {
        /// <summary>
        /// Load the viewmodel from a store
        /// </summary>
        void Load(IAmazonStoreEntity amazonStoreEntity);
    }
}