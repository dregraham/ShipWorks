﻿using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Rate
{
    /// <summary>
    /// An implementation of the CarrierRequest that issues FedEx RateRequest request types.
    /// </summary>
    public interface IFedExRateRequest
    {
        /// <summary>
        /// Submits the request to the carrier API
        /// </summary>
        GenericResult<IFedExRateResponse> Submit(IShipmentEntity shipment);

        /// <summary>
        /// Submits the request to the carrier API
        /// </summary>
        GenericResult<IFedExRateResponse> Submit(IShipmentEntity shipment, FedExRateRequestOptions options);
    }
}