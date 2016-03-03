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
                ratingField.AddShipmentField(PostalShipmentFields.PackagingType);
                ratingField.AddShipmentField(PostalShipmentFields.DimsHeight);
                ratingField.AddShipmentField(PostalShipmentFields.DimsLength);
                ratingField.AddShipmentField(PostalShipmentFields.DimsWidth);
                ratingField.AddShipmentField(PostalShipmentFields.DimsAddWeight);
                ratingField.AddShipmentField(PostalShipmentFields.DimsWeight);
                ratingField.AddShipmentField(PostalShipmentFields.NonMachinable);
                ratingField.AddShipmentField(PostalShipmentFields.NonRectangular);
                ratingField.AddShipmentField(PostalShipmentFields.InsuranceValue);

                return ratingField;
            }
        }
    }
}
