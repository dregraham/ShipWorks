﻿using ShipWorks.ApplicationCore.ComponentRegistration.Ordering;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps.LabelRetrieval
{
    /// <summary>
    /// Reset return status if shipment type does not support returns
    /// </summary>
    [Order(Order.Unordered, typeof(ILabelRetrievalShipmentManipulator))]
    public class ResidentialStatusManipulator : ILabelRetrievalShipmentManipulator
    {
        readonly IShipmentTypeManager shipmentTypeManager;
        readonly IResidentialDeterminationService residentialDeterminationService;

        /// <summary>
        /// Constructor
        /// </summary>
        public ResidentialStatusManipulator(IShipmentTypeManager shipmentTypeManager,
            IResidentialDeterminationService residentialDeterminationService)
        {
            this.residentialDeterminationService = residentialDeterminationService;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Manipulate the shipment
        /// </summary>
        public ShipmentEntity Manipulate(ShipmentEntity shipment)
        {
            ShipmentType shipmentType = shipmentTypeManager.Get(shipment);

            // Determine residential status
            if (shipmentType.IsResidentialStatusRequired(shipment))
            {
                shipment.ResidentialResult = residentialDeterminationService.IsResidentialAddress(shipment);
            }

            return shipment;
        }
    }
}
