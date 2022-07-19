using System.Linq;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Platform;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Manage label through Amazon
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.AmazonSFP)]
    public class AmazonSFPLabelService : ILabelService
    {
        private readonly IRatesRetriever ratesRetriever;
        private readonly IAmazonSfpLabelClient sfpLabelClient;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPLabelService(
            IRatesRetriever ratesRetriever, 
            IAmazonSfpLabelClient sfpLabelClient)
        {
            this.ratesRetriever = ratesRetriever;
            this.sfpLabelClient = sfpLabelClient;
        }

        /// <summary>
        /// Create the label
        /// </summary>
        public async Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
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

            var response = await sfpLabelClient.Create(shipment).ConfigureAwait(false);

            return response;
        }

        /// <summary>
        /// Void the Shipment
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            sfpLabelClient.Void(shipment);
        }
    }
}
