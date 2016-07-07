﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ShipWorks.Core.Messaging.Messages.Shipping;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Interface for loading shipments
    /// </summary>
    public interface IShipmentLoader
    {
        /// <summary>
        /// Load an order selection
        /// </summary>
        Task<LoadedOrderSelection> Load(long orderID);

        /// <summary>
        /// Load an order selection
        /// </summary>
        Task<IEnumerable<LoadedOrderSelection>> Load(long[] orderIDList);
    }
}
