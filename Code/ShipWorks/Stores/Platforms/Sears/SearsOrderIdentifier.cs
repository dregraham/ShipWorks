using System;
using System.Linq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Sears
{
    /// <summary>
    /// Class for uniquely identifying a sears order
    /// </summary>
    public class SearsOrderIdentifier : OrderIdentifier
    {
        long confirmationNumber;
        string poNumber;

        /// <summary>
        /// Constructor
        /// </summary>
        public SearsOrderIdentifier(long confirmationNumber, string poNumber)
        {
            this.confirmationNumber = confirmationNumber;
            this.poNumber = poNumber;
        }

        /// <summary>
        /// Apply the unique order information to the order
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            SearsOrderEntity searsOrder = order as SearsOrderEntity;

            if (searsOrder == null)
            {
                throw new InvalidOperationException("A non Sears order was passed to the Sears order identifier.");
            }

            searsOrder.OrderNumber = confirmationNumber;
            searsOrder.PoNumber = poNumber;
        }

        /// <summary>
        /// Apply the unique order information to the download detail
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.OrderNumber = confirmationNumber;
            downloadDetail.ExtraStringData1 = poNumber;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            factory.SearsOrderSearch
                .Where(SearsOrderSearchFields.PoNumber == poNumber)
                .AndWhere(SearsOrderSearchFields.OrderID == confirmationNumber);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() => $"ConfirmationNumber:{confirmationNumber};PoNumber:{poNumber}";
    }
}
