using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Ups RateHashingService
    /// </summary>
    public class AmazonRateHashingService : RateHashingService
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

                ratingField.AddShipmentField(AmazonShipmentFields.DeclaredValue);
                ratingField.AddShipmentField(AmazonShipmentFields.DeliveryExperience);
                ratingField.AddShipmentField(AmazonShipmentFields.DimsAddWeight);
                ratingField.AddShipmentField(AmazonShipmentFields.DimsHeight);
                ratingField.AddShipmentField(AmazonShipmentFields.DimsLength);
                ratingField.AddShipmentField(AmazonShipmentFields.DimsWeight);
                ratingField.AddShipmentField(AmazonShipmentFields.DimsAddWeight);

                return ratingField;
            }
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        public override string GetRatingHash(ShipmentEntity shipment)
        {
            return RatingFields.GetRatingHash(shipment);
        }
    }
}
