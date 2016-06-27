using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Rate hashing service for the Usps carrier
    /// </summary>
    public class UspsRateHashingService : PostalRateHashingService
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
                ratingField.AddShipmentField(UspsShipmentFields.UspsAccountID, genericAccountIdFieldName);
                ratingField.AddShipmentField(UspsShipmentFields.OriginalUspsAccountID);
                ratingField.AddShipmentField(UspsShipmentFields.RateShop);
                ratingField.AddShipmentField(PostalShipmentFields.NoPostage);

                return ratingField;
            }
        }
    }
}
