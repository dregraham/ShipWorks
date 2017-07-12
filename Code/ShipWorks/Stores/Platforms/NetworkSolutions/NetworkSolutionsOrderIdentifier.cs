using System;
using System.Linq;
using SD.LLBLGen.Pro.QuerySpec;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.FactoryClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.NetworkSolutions
{
    /// <summary>
    /// Identifies a NetworkSolutions order
    /// </summary>
    public class NetworkSolutionsOrderIdentifier : OrderIdentifier
    {
        long networkSolutionsOrderID = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsOrderIdentifier(long networkSolutionsOrderID)
        {
            this.networkSolutionsOrderID = networkSolutionsOrderID;
        }

        /// <summary>
        /// Apply the identifier to the order passed in
        /// </summary>
        public override void ApplyTo(OrderEntity order)
        {
            NetworkSolutionsOrderEntity nsOrder = order as NetworkSolutionsOrderEntity;
            if (nsOrder == null)
            {
                throw new InvalidOperationException("A non NetworkSolutionsOrderEntity was passed to the NetworkSolutions order identifier.");
            }

            nsOrder.NetworkSolutionsOrderID = networkSolutionsOrderID;
        }

        /// <summary>
        /// Apply the identifier to the DownloadDetailEntity provided
        /// </summary>
        public override void ApplyTo(DownloadDetailEntity downloadDetail)
        {
            downloadDetail.ExtraBigIntData1 = networkSolutionsOrderID;
        }

        /// <summary>
        /// Create an entity query that can be used to retrieve the search record for a combined order
        /// </summary>
        public override QuerySpec CreateCombinedSearchQuery(QueryFactory factory) =>
            factory.NetworkSolutionsOrderSearch.Where(NetworkSolutionsOrderSearchFields.NetworkSolutionsOrderID == networkSolutionsOrderID);

        /// <summary>
        /// String representation
        /// </summary>
        public override string ToString() => $"NetworkSolutionsOrderID:{networkSolutionsOrderID}";
    }
}
