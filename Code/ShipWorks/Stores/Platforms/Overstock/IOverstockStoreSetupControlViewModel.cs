using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Interface for OverstockStoreSetupControlViewModel
    /// </summary>
    public interface IOverstockStoreSetupControlViewModel
    {
        /// <summary>
        /// Load the store into the view model
        /// </summary>
        void Load(OverstockStoreEntity store);

        /// <summary>
        /// Save the store to the view model
        /// </summary>
        bool Save(OverstockStoreEntity store);
    }
}