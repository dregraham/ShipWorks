﻿using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance.InsureShip.Net
{
    /// <summary>
    /// Interface for a InsureShip Request Factory
    /// </summary>
    public interface IInsureShipRequestFactory
    {
        /// <summary>
        /// Creates the insure shipment request.
        /// </summary>
        InsureShipRequestBase CreateInsureShipmentRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate);

        /// <summary>
        /// Creates the submit claim request
        /// </summary>
        InsureShipRequestBase CreateSubmitClaimRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate);

        /// <summary>
        /// Creates the void policy request.
        /// </summary>
        InsureShipRequestBase CreateVoidPolicyRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate);

        /// <summary>
        /// Creates the claim status request.
        /// </summary>
        InsureShipRequestBase CreateClaimStatusRequest(ShipmentEntity shipmentEntity, InsureShipAffiliate insureShipAffiliate);
    }
}
