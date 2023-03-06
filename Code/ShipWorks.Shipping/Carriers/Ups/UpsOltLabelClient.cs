using System;
using System.Threading.Tasks;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// Ups Label client for Ups Online Tools
    /// </summary>
    [Component(RegistrationType.Self)]
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
				if (ex.ErrorCode == "121984" &&
					(shipment.ShipCountryCode == "MX" || shipment.OriginCountryCode == "MX") &&
					!string.IsNullOrEmpty(ex.ErrorLocation) &&
					string.Equals(ex.ErrorLocation, "ShipmentConfirmRequest/Shipment/Package/Description", StringComparison.OrdinalIgnoreCase))
				{
					message = "Shipments to Mexico must include a Merchandise Description. Please enter a description into the Customs tab > Description field.";
				}

throw new ShippingException(message, ex);
            }
            catch (CarrierException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Void the given shipment
        /// </summary>
        public void VoidLabel(ShipmentEntity shipment)
        {
            try
            {
                if (!UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
                {
                    UpsApiVoidClient.VoidShipment(shipment);
                }
            }
            catch (UpsException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }
        }
    }
}
