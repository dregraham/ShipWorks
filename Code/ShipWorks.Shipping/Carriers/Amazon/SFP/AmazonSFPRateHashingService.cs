using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Ups RateHashingService
    /// </summary>
    public class AmazonSFPRateHashingService : RateHashingService
    {
        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        protected override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;

                ratingField.AddShipmentField(AmazonSFPShipmentFields.DeclaredValue);
                ratingField.AddShipmentField(AmazonSFPShipmentFields.DeliveryExperience);
                ratingField.AddShipmentField(AmazonSFPShipmentFields.DimsAddWeight);
                ratingField.AddShipmentField(AmazonSFPShipmentFields.DimsHeight);
                ratingField.AddShipmentField(AmazonSFPShipmentFields.DimsLength);
                ratingField.AddShipmentField(AmazonSFPShipmentFields.DimsWeight);
                ratingField.AddShipmentField(AmazonSFPShipmentFields.DimsAddWeight);

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
