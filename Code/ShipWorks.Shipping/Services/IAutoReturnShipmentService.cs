﻿using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    public interface IAutoReturnShipmentService
    {
        /// <summary>
        /// An exception thrown while trying to apply a profile to the return shipment
        /// </summary>
        ShippingException ApplyProfileException { get; }

        /// <summary>
        /// Creates a new auto return shipments
        /// </summary>
        ShipmentEntity CreateReturn(ShipmentEntity shipment);
    }
}
