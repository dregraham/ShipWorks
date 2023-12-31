﻿using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Fims
{
    /// <summary>
    /// Responsible for saving retrieved FedEx FIMS Labels to Database
    /// </summary>
    public interface IFimsLabelRepository
    {
        /// <summary>
        /// Responsible for saving retrieved FedEx FIMS Labels to Database
        /// </summary>
        void SaveLabel(IFimsShipResponse fimsShipResponse, long ownerID);

        /// <summary>
        /// If we had saved an image for this shipment previously, but the shipment errored out later (like for an MPS), then clear before
        /// we start.
        /// </summary>
        void ClearReferences(IShipmentEntity shipment);
    }
}
