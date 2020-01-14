﻿using System.Collections.Generic;
using ShipWorks.Data.Model.Custom;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Provides carrier accounts based on shipment type
    /// </summary>
    public interface IShippingAccountListProvider
    {
        /// <summary>
        /// Gets the available accounts.
        /// </summary>
        IEnumerable<ICarrierAccount> GetAvailableAccounts(ShipmentTypeCode shipmentTypeCode);
    }
}