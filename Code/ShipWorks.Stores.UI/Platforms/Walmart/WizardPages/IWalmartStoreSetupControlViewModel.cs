using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.UI.Platforms.Walmart.WizardPages
{
    /// <summary>
    /// Logic for the WalmartStoreSetupControl
    /// </summary>
    public interface IWalmartStoreSetupControlViewModel
    {
        /// <summary>
        /// Client ID issued by Walmart
        /// </summary>
        string ClientID { get; set; }

        /// <summary>
        /// Client secret issued by Walmart
        /// </summary>
        string ClientSecret { get; set; }

        /// <summary>
        /// Saves the store credentials
        /// </summary>
        void Save(WalmartStoreEntity store);

        /// <summary>
        /// Loads the credentials for the given store.
        /// </summary>
        void Load(WalmartStoreEntity store);
    }
}