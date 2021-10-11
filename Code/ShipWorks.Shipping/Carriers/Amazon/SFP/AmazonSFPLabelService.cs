using System;
using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Manage label through Amazon
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.AmazonSFP)]
    public class AmazonSFPLabelService : ILabelService
    {
        private readonly IAmazonSFPCreateShipmentRequest createShipmentRequest;
        private readonly IAmazonSFPCancelShipmentRequest cancelShipmentRequest;
        private readonly Func<ShipmentEntity, AmazonShipment, AmazonSFPDownloadedLabelData> createDownloadedLabelData;
        private readonly IRatesRetriever ratesRetriever;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPLabelService(IAmazonSFPCreateShipmentRequest createShipmentRequest,
            IAmazonSFPCancelShipmentRequest cancelShipmentRequest,
            Func<ShipmentEntity, AmazonShipment, AmazonSFPDownloadedLabelData> createDownloadedLabelData,
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

            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);
            if (string.IsNullOrEmpty(shipment.AmazonSFP.ShippingServiceID))
            {
                GenericResult<RateGroup> rateResult = new GenericResult<RateGroup>();
                telemetricResult.RunTimedEvent(TelemetricEventType.GetRates, () => rateResult = ratesRetriever.GetRates(shipment));

                if (rateResult.Success)
                {
                    string serviceId = ((AmazonRateTag) rateResult.Value?.Rates?.FirstOrDefault()?.Tag)?.ShippingServiceId;                    
                    shipment.AmazonSFP.ShippingServiceID = serviceId;
                }
            }

            AmazonShipment labelResponse = null;
            labelResponse = createShipmentRequest.Submit(shipment, telemetricResult);
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
