using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.UI.Platforms.Walmart.WizardPages
{
    /// <summary>
    /// Logic for the WalmartStoreSetupControl
    /// </summary>
    public interface IWalmartStoreSetupControlViewModel
    {
        /// <summary>
        /// Consumer ID issued by Walmart
        /// </summary>
        string ConsumerID { get; set; }

        /// <summary>
        /// Private key issued by Walmart
        /// </summary>
        string PrivateKey { get; set; }

        /// <summary>
        /// Channel Type issued by Walmart
        /// </summary>
        string ChannelType { get; set; }
        
        /// <summary>
        /// Saves the store credentials
        /// </summary>
        bool Save(WalmartStoreEntity store);
    }
}