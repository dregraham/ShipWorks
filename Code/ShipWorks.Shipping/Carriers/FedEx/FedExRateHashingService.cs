using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// FedEx RateHashingService
    /// </summary>
    public class FedExRateHashingService : RateHashingService
    {
        /// <summary>
        /// Fields of a shipment used to calculate rates
        /// </summary>
        public override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;
                ratingField.ShipmentFields.Add(FedExShipmentFields.FedExAccountID);
                ratingField.ShipmentFields.Add(FedExShipmentFields.WeightUnitType);
                ratingField.ShipmentFields.Add(FedExShipmentFields.Signature);
                ratingField.ShipmentFields.Add(FedExShipmentFields.Service);
                ratingField.ShipmentFields.Add(FedExShipmentFields.PackagingType);
                ratingField.ShipmentFields.Add(FedExShipmentFields.DropoffType);
                ratingField.ShipmentFields.Add(FedExShipmentFields.SaturdayDelivery);
                ratingField.ShipmentFields.Add(FedExShipmentFields.OriginResidentialDetermination);
                ratingField.ShipmentFields.Add(FedExShipmentFields.SmartPostHubID);
                ratingField.ShipmentFields.Add(FedExShipmentFields.SmartPostIndicia);
                ratingField.ShipmentFields.Add(FedExShipmentFields.SmartPostEndorsement);
                ratingField.ShipmentFields.Add(FedExShipmentFields.CodEnabled);
                ratingField.ShipmentFields.Add(FedExShipmentFields.NonStandardContainer);

                ratingField.PackageFields.Add(FedExPackageFields.DimsWeight);
                ratingField.PackageFields.Add(FedExPackageFields.DimsAddWeight);
                ratingField.PackageFields.Add(FedExPackageFields.DeclaredValue);
                ratingField.PackageFields.Add(FedExPackageFields.DimsLength);
                ratingField.PackageFields.Add(FedExPackageFields.DimsHeight);
                ratingField.PackageFields.Add(FedExPackageFields.DimsWidth);
                ratingField.PackageFields.Add(FedExPackageFields.ContainsAlcohol);
                ratingField.PackageFields.Add(FedExPackageFields.DryIceWeight);

                return ratingField;
            }
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        public override string GetRatingHash(ShipmentEntity shipment)
        {
            return RatingFields.GetRatingHash(shipment, shipment.FedEx.Packages);
        }
    }
}
