using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Manual
{
    /// <summary>
    /// Interface for ManualStoreControlViewModel
    /// </summary>
    public interface IManualStoreSetupControlViewModel
    {
        /// <summary>
        /// Load the store into the view model 
        /// </summary>
        void Load(StoreEntity store);

        /// <summary>
        /// Save the store to the view model 
        /// </summary>
        bool Save(StoreEntity store);
    }
}
