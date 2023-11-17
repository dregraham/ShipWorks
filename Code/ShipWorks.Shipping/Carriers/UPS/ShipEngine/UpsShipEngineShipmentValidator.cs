
using System.Collections.Generic;
using System.Linq;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;

namespace ShipWorks.Shipping.Carriers.Ups.ShipEngine
{
    /// <summary>
    /// Valiate UPS ShipEngine shipments
    /// </summary>
    public class UpsShipEngineShipmentValidator : IUpsShipmentValidator
    {
        /// <summary>
        /// Validate the given UPS ShipEngine shipment
        /// </summary>
        public Result ValidateShipment(ShipmentEntity shipment)
        {
            // Country codes
            var validCountryCodes = new List<string> 
            { 
                // USA
                "US",               

                // Puerto Rico
                "PR"
            };

            if(validCountryCodes.None(validCode => shipment.ShipCountryCode.Equals(validCode, System.StringComparison.OrdinalIgnoreCase)))
            {
                return Result.FromError("International shipments are not supported with this account.");
            } 

            if (shipment.Ups.EmailNotifySender > 0 ||
                shipment.Ups.EmailNotifyRecipient > 0 ||
                shipment.Ups.EmailNotifyOther > 0)
            {
                return Result.FromError("Quantum View Notify is not supported with this account.");
            }

            if (shipment.Ups.Packages.Any(p => p.DryIceEnabled && !(p.UpsShipment.Service == (int)UpsServiceType.UpsGround || p.UpsShipment.Service == (int)UpsServiceType.UpsGroundSaver)))
            {
                return Result.FromError("Dry ice is not supported with this service.");
            }

            if (shipment.Ups.CodEnabled)
            {
                return Result.FromError("Collect on Delivery is not supported with this account.");
            }

            if (shipment.Ups.ShipperRelease)
            {
                return Result.FromError("Shipper Release is not supported with this account.");
            }

            if (shipment.Ups.CarbonNeutral)
            {
                return Result.FromError("Carbon Neutral is not supported with this account.");
            }

            if (shipment.Ups.CommercialPaperlessInvoice)
            {
                return Result.FromError("Paperless Invoicing is not supported with this account.");
            }

            if (shipment.Ups.Packages.Any(p => p.VerbalConfirmationEnabled))
            {
                return Result.FromError("Verbal Confirmation is not supported with this account.");
            }

            if (shipment.Ups.PayorType != (int) UpsPayorType.Sender)
            {
                return Result.FromError("Third Party Billing is not supported with this account.");
            }

            if(shipment.Ups.Packages.Any(p => p.DeclaredValue > 999M))
            {
                return Result.FromError("This service is unavailable for shipments over $1000.");
            }

            return Result.FromSuccess();
        }
    }
}
