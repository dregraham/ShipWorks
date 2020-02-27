using System;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Ups Label client for Ups Online Tools
    /// </summary>
    public class UpsOltLabelClient : IUpsLabelClient
    {
        private readonly Func<UpsLabelResponse, UpsDownloadedLabelData> createDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsOltLabelClient(Func<UpsLabelResponse, UpsDownloadedLabelData> createDownloadedLabelData)
        {
            this.createDownloadedLabelData = createDownloadedLabelData;
        }

        /// <summary>
        /// Get a label for the given shipment
        /// </summary>
        public Task<TelemetricResult<IDownloadedLabelData>> GetLabel(ShipmentEntity shipment)
        {
            try
            {
                TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);

                TelemetricResult<UpsLabelResponse> telemetricUpsLabelResponse = UpsApiShipClient.ProcessShipment(shipment);

                telemetricUpsLabelResponse.CopyTo(telemetricResult);
                telemetricResult.SetValue(createDownloadedLabelData(telemetricUpsLabelResponse.Value));

                return Task.FromResult(telemetricResult);
            }
            catch (UpsApiException ex)
            {
                string message = ex.Message;

                // find the "XML document is well formed but not valid" error
                if (ex.ErrorCode == "10002" &&
                    shipment.ReturnShipment &&
                    !string.IsNullOrEmpty(ex.ErrorLocation) &&
                    string.Equals(ex.ErrorLocation, "ShipmentConfirmRequest/Shipment/Package/Description", StringComparison.OrdinalIgnoreCase))
                {
                    message = "The return shipment's Contents is required.";
                }

                throw new ShippingException(message, ex);
            }
            catch (CarrierException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}
