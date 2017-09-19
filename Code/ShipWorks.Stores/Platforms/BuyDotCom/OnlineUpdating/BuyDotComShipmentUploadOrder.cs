using System.Collections.Generic;
using Interapptive.Shared.Collections;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.BuyDotCom.OnlineUpdating
{
    /// <summary>
    /// Order data needed for uploading shipment details
    /// </summary>
    public class BuyDotComShipmentUploadOrder
    {
        public BuyDotComShipmentUploadOrder(string orderNumberComplete, bool isManual, IEnumerable<IBuyDotComOrderItemEntity> items)
        {
            OrderNumberComplete = orderNumberComplete;
            IsManual = isManual;
            Items = items.ToReadOnly();
        }

        /// <summary>
        /// Order number
        /// </summary>
        public string OrderNumberComplete { get; }

        /// <summary>
        /// Is the order manual
        /// </summary>
        public bool IsManual { get; }

        /// <summary>
        /// Items associated with the order
        /// </summary>
        public IEnumerable<IBuyDotComOrderItemEntity> Items { get; }
    }
}