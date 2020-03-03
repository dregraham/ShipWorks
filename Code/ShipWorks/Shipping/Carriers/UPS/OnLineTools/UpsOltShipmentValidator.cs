using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools
{
    /// <summary>
    /// Validates OLT shipment.
    /// </summary>
    public class UpsOltShipmentValidator : IUpsShipmentValidator
    {
        /// <summary>
        /// Validates the shipment.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public Result ValidateShipment(ShipmentEntity shipment)
        {
            if (!UpsUtility.IsUpsMiService((UpsServiceType) shipment.Ups.Service))
            {
                return Result.FromSuccess();
            }

            if (HasValidQvnMIOptions(shipment.Ups.EmailNotifyOther) || 
                HasValidQvnMIOptions(shipment.Ups.EmailNotifyRecipient) ||
                HasValidQvnMIOptions(shipment.Ups.EmailNotifySender))
            {
                return Result.FromError("Exception and Delivery are not valid notification options for Mail Innovations shipments.");
            }

            return Result.FromSuccess();
        }

        /// <summary>
        /// Determines whether the Qvn Options are valid for an MI Shipment.
        /// </summary>
        /// <param name="emailNotify">The email notify.</param>
        private bool HasValidQvnMIOptions(int emailNotify) =>
            !((emailNotify & (int) FedExEmailNotificationType.Exception) != 0 ||
                (emailNotify & (int) FedExEmailNotificationType.Deliver) != 0);
    }
}
