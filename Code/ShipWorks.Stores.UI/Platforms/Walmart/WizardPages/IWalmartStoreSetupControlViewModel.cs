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
        string PrivateKey { get; set; }

        /// <summary>
        /// Channel Type issued by Walmart
        /// </summary>
        string ChannelType { get; set; }
    }
}