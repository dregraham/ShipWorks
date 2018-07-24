using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        public Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));

            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("API.ResponseTimeInMilliseconds");
            if (string.IsNullOrEmpty(shipment.Amazon.ShippingServiceID))
            {
                GenericResult<RateGroup> rateResult = new GenericResult<RateGroup>();
                telemetricResult.RunTimedEvent(TelemetricEventType.GetRates, () => rateResult = ratesRetriever.GetRates(shipment));
                
                if (rateResult.Success)
                {
                    string serviceId = ((AmazonRateTag) rateResult.Value?.Rates?.FirstOrDefault()?.Tag)?.ShippingServiceId ?? string.Empty;
                    shipment.Amazon.ShippingServiceID = serviceId;
                }
            }

            AmazonShipment labelResponse = null;
            telemetricResult.RunTimedEvent(TelemetricEventType.GetLabel, () => labelResponse = createShipmentRequest.Submit(shipment));
            telemetricResult.SetValue(createDownloadedLabelData(shipment, labelResponse));
            
            return Task.FromResult(telemetricResult);
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
