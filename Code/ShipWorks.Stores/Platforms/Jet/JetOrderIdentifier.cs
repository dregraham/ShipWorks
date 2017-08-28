using System;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Jet
{
    public class JetOrderIdentifier : OrderIdentifier
    {
        private readonly string merchantOrderId;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetOrderIdentifier(string merchantOrderId)
        {
            this.merchantOrderId = merchantOrderId;
        }

        /// <summary>
        /// Apply the order identifier values to the order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            JetOrderEntity jetOrder = order as JetOrderEntity;

            if (jetOrder == null)
            {
                throw new InvalidOperationException("A non Jet order was passed to the Jet order identifier.");
            }

            jetOrder.MerchantOrderId = merchantOrderId;
        }

        /// <summary>
        /// Apply the order identifier values to the download history entry
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraStringData1 = merchantOrderId;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            factory.JetOrder.Where(JetOrderFields.MerchantOrderId == merchantOrderId);

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        public override string ToString() => $"JetMerchantOrderId:{merchantOrderId}";
    }
}