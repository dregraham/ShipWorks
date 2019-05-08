using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services
{
    public class AutoReturnShipmentService
    {
        private IEnumerable<ShipmentEntity> shipments;
        private List<Tuple<ShipmentEntity, ShipmentEntity>> shipmentsWithReturns;

        /// <summary>
        /// Constructor
        /// </summary>
        public AutoReturnShipmentService(IEnumerable<ShipmentEntity> shipments)
        {
            this.shipments = shipments;
        }

        /// <summary>
        /// Creates an auto return shipment
        /// </summary>
        private ShipmentEntity CreateReturnShipment(ShipmentEntity shipment)
        {
            // Copy auto return shipment logic here
            throw new NotImplementedException();
        }

        /// <summary>
        /// Applies the return profile
        /// </summary>
        private void ApplyReturnProfile(ref ShipmentEntity shipment)
        {
            // Copy auto return profile logic here
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the new auto return shipments
        /// </summary>
        /// <returns>
        /// A tuple that is of the original shipment and the
        /// new return shipment.
        /// </returns>
        public List<Tuple<ShipmentEntity, ShipmentEntity>> GetShipments()
        {
            foreach (ShipmentEntity shipment in shipments)
            {
                if (shipment.IncludeReturn)
                {
                    // Create new auto return shipment
                    var newReturnShipment = CreateReturnShipment(shipment);

                    if (shipment.ApplyReturnProfile)
                    {
                        // Apply return profile
                        ApplyReturnProfile(ref newReturnShipment);
                    }
                    shipmentsWithReturns.Add(new Tuple<ShipmentEntity, ShipmentEntity>(shipment, newReturnShipment));
                }
                else
                {
                    shipmentsWithReturns.Add(new Tuple<ShipmentEntity, ShipmentEntity>(shipment, null));
                }
            }

            return shipmentsWithReturns;
        }
    }
}