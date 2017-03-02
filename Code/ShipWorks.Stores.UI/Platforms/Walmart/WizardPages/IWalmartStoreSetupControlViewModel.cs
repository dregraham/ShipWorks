using System.Security;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.UI.Platforms.Walmart.WizardPages
{
    public interface IWalmartStoreSetupControlViewModel
    {
        /// <summary>
        /// Consumer ID issued by Walmart
        /// </summary>
        string ConsumerID { get; set; }

        /// <summary>
        /// Private key issued by Walmart
        /// </summary>
        SecureString PrivateKey { get; set; }

        /// <summary>
        /// Channel Type issued by Walmart
        /// </summary>
        string ChannelType { get; set; }

        /// <summary>
        /// Loads the store credentials
        /// </summary>
        void Load(WalmartStoreEntity store);

        /// <summary>
        /// Saves the store credentials
        /// </summary>
        bool Save(WalmartStoreEntity store);
    }
}