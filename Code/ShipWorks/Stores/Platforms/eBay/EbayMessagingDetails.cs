using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Details about a message to be sent to an eBay user.
    /// </summary>
    public class EbayMessagingDetails
    {
        /// <summary>
        /// The type of eBay message to send
        /// </summary>
        public EbaySendMessageType MessageType { get; set; }

        /// <summary>
        /// The subject of the message to send
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// The message contents
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Whether or not to have eBay send a copy to the seller (SW user)
        /// </summary>
        public bool CopyMe { get; set; }

        /// <summary>
        /// Returns the ID of the selected order item, or 0 if All
        /// </summary>
        public long SelectedItemID { get; set; }
    }
}