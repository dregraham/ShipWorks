using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// LabelService for Ups Online Tools
    /// </summary>
    public class UpsOltLabelService : UpsLabelService
    {
        private readonly IUpsOltShipmentValidator upsOltShipmentValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="upsOltShipmentValidator"></param>
        public UpsOltLabelService(IUpsOltShipmentValidator upsOltShipmentValidator)
        {
            this.upsOltShipmentValidator = upsOltShipmentValidator;
        }

        /// <summary>
        /// Creates a label for Ups Online Tools
        /// </summary>
        public override void Create(ShipmentEntity shipment)
        {
            try
            {
                // Call the base class for setting default values as needed based on the service/package type of the shipment
                base.Create(shipment);
                
                upsOltShipmentValidator.ValidateShipment(shipment);

                UpsServicePackageTypeSetting.Validate(shipment);
                UpsApiShipClient.ProcessShipment(shipment);
            }
            catch (UpsApiException ex)
            {
                string message = ex.Message;

                // find the "XML document is well formed but not valid" error
                if (ex.ErrorCode == "10002" && shipment.ReturnShipment && !String.IsNullOrEmpty(ex.ErrorLocation))
                {
                    if (String.Compare(ex.ErrorLocation, "ShipmentConfirmRequest/Shipment/Package/Description", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        message = "The return shipment's Contents is required.";
                    }
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