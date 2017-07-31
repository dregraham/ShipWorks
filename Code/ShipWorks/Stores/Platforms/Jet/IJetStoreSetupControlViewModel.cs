using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Interface for JetStoreSetupControlViewModel
    /// </summary>
    public interface IJetStoreSetupControlViewModel
    {
        /// <summary>
        /// Load the store into the view model
        /// </summary>
        void Load(JetStoreEntity store);

        /// <summary>
        /// Save the store to the view model
        /// </summary>
        bool Save(JetStoreEntity store);
    }
}