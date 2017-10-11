using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Rate hashing service implementation for DHL Express
    /// </summary>
    [KeyedComponent(typeof(IRateHashingService), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressRateHashingService : RateHashingService
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
                ratingField.AddShipmentField(DhlExpressShipmentFields.DhlExpressAccountID, genericAccountIdFieldName);
                ratingField.AddShipmentField(DhlExpressShipmentFields.SaturdayDelivery);
                ratingField.AddShipmentField(DhlExpressShipmentFields.DeliveredDutyPaid);
                ratingField.AddShipmentField(DhlExpressShipmentFields.NonMachinable);
                ratingField.AddShipmentField(DhlExpressShipmentFields.Service);

                ratingField.PackageFields.Add(DhlExpressPackageFields.DeclaredValue);
                ratingField.PackageFields.Add(DhlExpressPackageFields.DimsAddWeight);
                ratingField.PackageFields.Add(DhlExpressPackageFields.DimsWeight);
                ratingField.PackageFields.Add(DhlExpressPackageFields.DimsLength);
                ratingField.PackageFields.Add(DhlExpressPackageFields.DimsHeight);
                ratingField.PackageFields.Add(DhlExpressPackageFields.DimsWidth);

                return ratingField;
            }
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        public override string GetRatingHash(ShipmentEntity shipment)
        {
            return RatingFields.GetRatingHash(shipment, shipment.DhlExpress.Packages);
        }
    }
}
