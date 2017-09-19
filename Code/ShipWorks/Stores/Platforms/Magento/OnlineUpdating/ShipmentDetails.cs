namespace ShipWorks.Stores.Platforms.Magento.OnlineUpdating
{
    /// <summary>
    /// Details for uploading shipment information
    /// </summary>
    public class ShipmentDetails
    {
        /// <summary>
        /// Comments
        /// </summary>
        public string Comments { get; set; } = string.Empty;

        /// <summary>
        /// Should customer be emailed
        /// </summary>
        public bool EmailCustomer { get; set; }
    }
}
