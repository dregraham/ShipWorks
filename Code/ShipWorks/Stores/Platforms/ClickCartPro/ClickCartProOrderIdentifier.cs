using System;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.ClickCartPro
{
    /// <summary>
    /// Configures order prototypes to locate orders
    /// </summary>
    public class ClickCartProOrderIdentifier : OrderIdentifier
    {
        string clickCartProOrderId = "";

        /// <summary>
        /// Constructor
        /// </summary>
        public ClickCartProOrderIdentifier(string clickCartProOrderId)
        {
            this.clickCartProOrderId = clickCartProOrderId;
        }

        /// <summary>
        /// Apply the prototype to the DownloadDetail
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraStringData1 = clickCartProOrderId;
        }

        /// <summary>
        /// Apply the Click Cart Pro order id to the order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            ClickCartProOrderEntity ccpOrder = order as ClickCartProOrderEntity;
            if (ccpOrder == null)
            {
                throw new InvalidOperationException("A non Click Cart Pro order was passed to the ClickCartProOrderIdentifier");
            }

            ccpOrder.ClickCartProOrderID = clickCartProOrderId;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            CreateCombinedSearchQueryInternal(factory,
                factory.ClickCartProOrderSearch,
                ClickCartProOrderSearchFields.OriginalOrderID,
                ClickCartProOrderSearchFields.ClickCartProOrderID == clickCartProOrderId);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("ClickCartProOrderID:{0}", clickCartProOrderId);
        }

        /// <summary>
        /// Value to use when auditing
        /// </summary>
        public override string AuditValue => clickCartProOrderId;
    }
}
