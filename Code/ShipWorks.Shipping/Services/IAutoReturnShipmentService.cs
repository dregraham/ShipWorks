using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    public interface IAutoReturnShipmentService
    {
        /// <summary>
        /// Creates an auto return shipment
        /// </summary>
        ShipmentEntity CreateReturnShipment(ShipmentEntity shipment);

        /// <summary>
        /// Applies the return profile
        /// </summary>
        void ApplyReturnProfile(ref ShipmentEntity shipment);

        /// <summary>
        /// Gets the new auto return shipments
        /// </summary>
        /// <returns>
        /// A tuple that is of the original shipment and the
        /// new return shipment.
        /// </returns>
        List<Tuple<ShipmentEntity, ShipmentEntity>> GetShipments();
    }
}
