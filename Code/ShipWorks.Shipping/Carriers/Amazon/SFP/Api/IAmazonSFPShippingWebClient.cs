﻿using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon.Mws;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Api
{
    /// <summary>
    /// Amazon shipping api client
    /// </summary>
    public interface IAmazonSFPShippingWebClient
    {
        /// <summary>
        /// Validate the given credentials
        /// </summary>
        AmazonValidateCredentialsResponse ValidateCredentials(IAmazonMwsWebClientSettings mwsSettings);

        /// <summary>
        /// Gets the rates.
        /// </summary>
        GetEligibleShippingServicesResponse GetRates(ShipmentRequestDetails requestDetails, AmazonSFPShipmentEntity shipment);

        /// <summary>
        /// Create a shipment
        /// </summary>
        AmazonShipment CreateShipment(ShipmentRequestDetails requestDetails, AmazonSFPShipmentEntity shipment, TelemetricResult<IDownloadedLabelData> telemetricResult);

        /// <summary>
        /// Voids the shipment
        /// </summary>
        CancelShipmentResponse CancelShipment(AmazonSFPShipmentEntity amazonShipment);
    }
}
