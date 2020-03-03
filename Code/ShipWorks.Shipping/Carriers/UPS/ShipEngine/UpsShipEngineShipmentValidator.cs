using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void ValidateShipment(ShipmentEntity shipment)
        {
            if (shipment.ReturnShipment)
            {
                throw new ShippingException("Return shipments are not supported with this account.");
            }

            if ((shipment.Ups.EmailNotifySender & (int) UpsEmailNotificationType.None) != 0 ||
                (shipment.Ups.EmailNotifyRecipient & (int) UpsEmailNotificationType.None) != 0 ||
                (shipment.Ups.EmailNotifyOther & (int) UpsEmailNotificationType.None) != 0)
            {
                throw new ShippingException("Quantum View Notify is not supported with this account.");
            }

            if (shipment.Ups.Packages.Any(p => p.DryIceEnabled))
            {
                throw new ShippingException("Dry ice is not supported with this account.");
            }

            if (shipment.Ups.CodEnabled)
            {
                throw new ShippingException("Collect on Delivery is not supported with this account.");
            }

            if (shipment.Ups.ShipperRelease)
            {
                throw new ShippingException("Shipper Release is not supported with this account.");
            }

            if (shipment.Ups.SaturdayDelivery)
            {
                throw new ShippingException("Saturday Delivery is not supported with this account.");
            }

            if (shipment.Ups.CarbonNeutral)
            {
                throw new ShippingException("Carbon Neutral is not supported with this account.");
            }

            if (shipment.Ups.CommercialPaperlessInvoice)
            {
                throw new ShippingException("Paperless Invoicing is not supported with this account.");
            }
        }
    }
}
