namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Interface to expose the Shipping UI
    /// </summary>
    public interface IGetAmazonCarrierCredentialsDialog
    {

        /// <summary>
        /// Exposes the Windows Close method
        /// </summary>
        void Close();
        
        /// <summary>
        /// Exposes the Windows ShowDialog method
        /// </summary>
        void ShowDialog();
    }
}
