using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Manage label through Amazon
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.Amazon)]
    public class AmazonLabelService : ILabelService
    {
        private readonly IAmazonCreateShipmentRequest createShipmentRequest;
        private readonly IAmazonCancelShipmentRequest cancelShipmentRequest;
        private readonly Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData> createDownloadedLabelData;
        private readonly IRatesRetriever ratesRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonLabelService(IAmazonCreateShipmentRequest createShipmentRequest,
            IAmazonCancelShipmentRequest cancelShipmentRequest,
            Func<ShipmentEntity, AmazonShipment, AmazonDownloadedLabelData> createDownloadedLabelData,
            IRatesRetriever ratesRetriever)
        {
            this.createShipmentRequest = createShipmentRequest;
            this.cancelShipmentRequest = cancelShipmentRequest;
            this.createDownloadedLabelData = createDownloadedLabelData;
            this.ratesRetriever = ratesRetriever;
        }

        /// <summary>
        /// Create the label
        /// </summary>
        public IDownloadedLabelData Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            if (string.IsNullOrEmpty(shipment.Amazon.ShippingServiceID))
            {
                GenericResult<RateGroup> rateResult = ratesRetriever.GetRates(shipment);
                
                if (rateResult.Success)
                {
                    string serviceId = ((AmazonRateTag) rateResult.Value?.Rates?.FirstOrDefault()?.Tag)?.ShippingServiceId ?? string.Empty;
                    shipment.Amazon.ShippingServiceID = serviceId;
                }
            }

            AmazonShipment labelResponse = createShipmentRequest.Submit(shipment);

            return createDownloadedLabelData(shipment, labelResponse);
        }

        /// <summary>
        /// Void the Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            cancelShipmentRequest.Submit(shipment);
        }
    }
}
