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

                ratingField.ShipmentFields.Add(AmazonShipmentFields.DeclaredValue);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DeliveryExperience);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsAddWeight);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsHeight);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsLength);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsWeight);
                ratingField.ShipmentFields.Add(AmazonShipmentFields.DimsAddWeight);

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
