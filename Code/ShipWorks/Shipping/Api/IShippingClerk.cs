﻿using System.Collections.Generic;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.Api
{
    /// <summary>
    /// An interface intended to act as a facade for carrying out shipping related tasks supported by ShipWorks
    /// (shipping a package, voiding a shipment, tracking a package, etc.) for a specific carrier and managing
    /// any ancillary details associated with those transactions such as authentication/handshakes, saving labels,
    /// exception handling, etc. The metaphor here is that of a clerk that a customer would interact with if he/she
    /// went to a carrier's physical store to accomplish these tasks.
    /// </summary>
    public interface IShippingClerk
    {
        /// <summary>
        /// Sends the shipment entity to the carrier so a shipment is created  in the carrier's system,
        /// and the resulting data (label, tracking info, etc.) is saved and/or updated on the shipment
        /// entity accordingly.
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        TelemetricResult<GenericResult<IEnumerable<IFedExShipResponse>>> Ship(ShipmentEntity shipmentEntity);

        /// <summary>
        /// Void/Cancel/Delete a shipment
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        void Void(ShipmentEntity shipmentEntity);

        /// <summary>
        /// Gets the shipping rates from the carrier for the given shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A RateGroup containing the rates received from the carrier.</returns>
        RateGroup GetRates(ShipmentEntity shipment, ICertificateInspector certificateInspector);

        /// <summary>
        /// Track a shipment
        /// </summary>
        /// <param name="shipmentEntity">The shipment entity.</param>
        TrackingResult Track(ShipmentEntity shipmentEntity);
    }
}
