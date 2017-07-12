using System;
using System.Linq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.PayPal
{
    /// <summary>
    /// Order Identifier for identifying orders based on PayPal Transaction IDs
    /// </summary>
    public class PayPalOrderIdentifier : OrderIdentifier
    {
        /// <summary>
        /// PayPal's transaction identifier
        /// </summary>
        string transactionID = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public PayPalOrderIdentifier(string payPalTransactionId)
        {
            this.transactionID = payPalTransactionId;
        }

        /// <summary>
        /// Applies the PayPal transaction ID to the provided download detail
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraStringData1 = transactionID;
        }

        /// <summary>
        /// Applies the PayPal Transaction ID to the provided order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            PayPalOrderEntity paypalOrder = order as PayPalOrderEntity;

            // make sure we're dealing with a PayPal order
            if (paypalOrder == null)
            {
                throw new InvalidOperationException("A non PayPal order was passed to the PayPal Order Identifier");
            }

            paypalOrder.TransactionID = transactionID;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            factory.PayPalOrderSearch.Where(PayPalOrderSearchFields.TransactionID == transactionID);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("PayPalTransactionID:{0}", transactionID);
        }

    }
}
