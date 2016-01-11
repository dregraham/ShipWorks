using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Ups RateHashingService
    /// </summary>
    public class UpsRateHashingService : RateHashingService
    {
        /// <summary>
        /// Gets the fields used for rating a shipment.
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
                ratingField.ShipmentFields.Add(UpsShipmentFields.UpsAccountID);
                ratingField.ShipmentFields.Add(UpsShipmentFields.SaturdayDelivery);
                ratingField.ShipmentFields.Add(UpsShipmentFields.SaturdayDelivery);
                ratingField.ShipmentFields.Add(UpsShipmentFields.CodAmount);
                ratingField.ShipmentFields.Add(UpsShipmentFields.CodEnabled);
                ratingField.ShipmentFields.Add(UpsShipmentFields.CodPaymentType);
                ratingField.ShipmentFields.Add(UpsShipmentFields.Service);
                ratingField.ShipmentFields.Add(UpsShipmentFields.DeliveryConfirmation);
                ratingField.PackageFields.Add(UpsPackageFields.PackagingType);
                ratingField.PackageFields.Add(UpsPackageFields.DeclaredValue);
                ratingField.PackageFields.Add(UpsPackageFields.VerbalConfirmationEnabled);
                ratingField.PackageFields.Add(UpsPackageFields.DimsWeight);
                ratingField.PackageFields.Add(UpsPackageFields.DimsLength);
                ratingField.PackageFields.Add(UpsPackageFields.DimsHeight);
                ratingField.PackageFields.Add(UpsPackageFields.DimsWidth);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceEnabled);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceRegulationSet);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceWeight);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceEnabled);
                ratingField.PackageFields.Add(UpsPackageFields.DryIceIsForMedicalUse);

                return ratingField;
            }
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        public override string GetRatingHash(ShipmentEntity shipment)
        {
            return RatingFields.GetRatingHash(shipment, shipment.Ups.Packages);
        }
    }
}
