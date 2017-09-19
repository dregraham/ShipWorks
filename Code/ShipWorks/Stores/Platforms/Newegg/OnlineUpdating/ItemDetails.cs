namespace ShipWorks.Stores.Platforms.Newegg.OnlineUpdating
{
    /// <summary>
    /// Item details for uploading Newegg shipment data
    /// </summary>
    public class ItemDetails
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ItemDetails(string sellerPartNumber, double quantity)
        {
            SellerPartNumber = sellerPartNumber;
            Quantity = quantity;
        }

        /// <summary>
        /// Seller part number
        /// </summary>
        public string SellerPartNumber { get; }

        /// <summary>
        /// Quantity
        /// </summary>
        public double Quantity { get; }
    }
}