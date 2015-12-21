using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Postal
{
    /// <summary>
    /// Rate hashing for the postal carrier
    /// </summary>
    public class PostalRateHashingService : RateHashingService
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
                ratingField.ShipmentFields.Add(PostalShipmentFields.PackagingType);
                ratingField.ShipmentFields.Add(PostalShipmentFields.DimsHeight);
                ratingField.ShipmentFields.Add(PostalShipmentFields.DimsLength);
                ratingField.ShipmentFields.Add(PostalShipmentFields.DimsWidth);
                ratingField.ShipmentFields.Add(PostalShipmentFields.DimsAddWeight);
                ratingField.ShipmentFields.Add(PostalShipmentFields.DimsWeight);
                ratingField.ShipmentFields.Add(PostalShipmentFields.NonMachinable);
                ratingField.ShipmentFields.Add(PostalShipmentFields.NonRectangular);
                ratingField.ShipmentFields.Add(PostalShipmentFields.InsuranceValue);

                return ratingField;
            }
        }
    }
}
