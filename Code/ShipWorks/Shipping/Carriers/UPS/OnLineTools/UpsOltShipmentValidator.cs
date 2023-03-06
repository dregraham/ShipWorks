using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
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
			if (shipment.ShipCountryCode == "MX" || shipment.OriginCountryCode == "MX")
			{
				if (shipment.Ups.Packages.Count > 1)
				{
					return Result.FromError("Single shipments containing multiple packages to/from Mexico are not allowed. Please create a shipment for each package.");
				}
			}

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
			(emailNotify & (int) UpsEmailNotificationType.Exception) != 0 ||
				(emailNotify & (int) UpsEmailNotificationType.Deliver) != 0;
	}
}
