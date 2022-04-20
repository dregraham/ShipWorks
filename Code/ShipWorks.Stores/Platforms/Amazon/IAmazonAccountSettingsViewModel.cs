using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Interface for an AmazonAccountSettingsViewModel
    /// </summary>
    public interface IAmazonAccountSettingsViewModel
    {
        void Load(IAmazonStoreEntity amazonStoreEntity);
    }
}