using System.Reflection;

namespace ShipWorks.Stores.Platforms.Ebay.Warehouse
{
    /// <summary>
    /// Ebay Warehouse Item
    /// </summary>
    [Obfuscation]
    public class EbayWarehouseItem
    {
        /// <summary>
        /// The EbayItemID of the EbayOrderItem
        /// </summary>
        public long EbayItemID { get; set; }

        /// <summary>
        /// The EbayTransactionID of the EbayOrderItem
        /// </summary>
        public long EbayTransactionID { get; set; }

        /// <summary>
        /// The SellingManagerRecord of the EbayOrderItem
        /// </summary>
        public int SellingManagerRecord { get; set; }

        /// <summary>
        /// The PaymentStatus of the EbayOrderItem
        /// </summary>
        public int PaymentStatus { get; set; }

        /// <summary>
        /// The PaymentMethod of the EbayOrderItem
        /// </summary>
        public int PaymentMethod { get; set; }

        /// <summary>
        /// The CompleteStatus of the EbayOrderItem
        /// </summary>
        public int CompleteStatus { get; set; }

        /// <summary>
        /// The FeedbackLeftType of the EbayOrderItem
        /// </summary>
        public int FeedbackLeftType { get; set; }

        /// <summary>
        /// The FeedbackLeftComments of the EbayOrderItem
        /// </summary>
        public string FeedbackLeftComments { get; set; }

        /// <summary>
        /// The FeedbackReceivedType of the EbayOrderItem
        /// </summary>
        public int FeedbackReceivedType { get; set; }

        /// <summary>
        /// The FeedbackReceivedComments of the EbayOrderItem
        /// </summary>
        public string FeedbackReceivedComments { get; set; }

        /// <summary>
        /// The MyEbayPaid of the EbayOrderItem
        /// </summary>
        public bool MyEbayPaid { get; set; }

        /// <summary>
        /// The MyEbayShipped of the EbayOrderItem
        /// </summary>
        public bool MyEbayShipped { get; set; }
    }
}
