using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Validates OLT shipment.
    /// </summary>
    public class UpsOltShipmentValidator
    {
        /// <summary>
        /// Validates the shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void ValidateShipment(ShipmentEntity shipment)
        {
            if (!UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
            {
                return;
            }

            HasValidQvnMIOptions(shipment.Ups.EmailNotifyOther);
            HasValidQvnMIOptions(shipment.Ups.EmailNotifyRecipient);
            HasValidQvnMIOptions(shipment.Ups.EmailNotifySender);
        }

        /// <summary>
        /// Determines whether the Qvn Options are valid for an MI Shipment. If not, throws Shipping Exception.
        /// </summary>
        /// <param name="emailNotify">The email notify.</param>
        private void HasValidQvnMIOptions(int emailNotify)
        {
            if ((emailNotify & (int)FedExEmailNotificationType.Exception) != 0 ||
                (emailNotify & (int)FedExEmailNotificationType.Deliver) != 0)
            {
                throw new ShippingException("Exception and Delivery are not valid notification options for Mail Innovations shipments.");
            }
        }
    }
}
