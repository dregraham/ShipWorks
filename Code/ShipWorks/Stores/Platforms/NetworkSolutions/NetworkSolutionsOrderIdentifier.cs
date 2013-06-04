using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Content;
using ShipWorks.Data.Model.EntityClasses;

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
        /// String representation
        /// </summary>
        public override string ToString()
        {
            return string.Format("NetworkSolutionsOrderID:{0}", networkSolutionsOrderID);
        }
    }
}
