using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Rakuten
{
    /// <summary>
    /// Interface for RakutenStoreSetupControlViewModel
    /// </summary>
    public interface IRakutenStoreSetupControlViewModel
    {
        /// <summary>
        /// Load the store into the view model
        /// </summary>
        void Load(RakutenStoreEntity store);

        /// <summary>
        /// Save the store to the view model
        /// </summary>
        bool Save(RakutenStoreEntity store);
    }
}