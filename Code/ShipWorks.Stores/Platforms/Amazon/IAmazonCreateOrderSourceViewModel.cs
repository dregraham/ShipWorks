using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Amazon
{
    /// <summary>
    /// Viewmodel to set up an Amazon Order Source
    /// </summary>
    public interface IAmazonCreateOrderSourceViewModel
    {
        /// <summary>
        /// Load the store
        /// </summary>
        void Load(AmazonStoreEntity store);
        
        /// <summary>
        /// Save the store
        /// </summary>
        void Save(AmazonStoreEntity store);
    }
}