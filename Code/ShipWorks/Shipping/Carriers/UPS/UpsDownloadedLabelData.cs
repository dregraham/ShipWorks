using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{    /// <summary>
     /// Label data that has been downloaded from UPS
     /// </summary>
    [Component(RegistrationType.Self)]

    public class UpsDownloadedLabelData : IDownloadedLabelData
    {
        private readonly UpsLabelResponse upsLabelResponse;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsDownloadedLabelData(UpsLabelResponse upsLabelResponse)
        {
            this.upsLabelResponse = upsLabelResponse;
        }

        /// <summary>
        /// Save label data to the database and/or disk
        /// </summary>
        public void Save()
        {
            try
            {
                UpsApiShipClient.ProcessShipAccept(upsLabelResponse);
            }
            catch (UpsApiException ex)
            {
                string message = ex.Message;

                // find the "XML document is well formed but not valid" error
                if (ex.ErrorCode == "10002" &&
                    upsLabelResponse.Shipment.ReturnShipment &&
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
