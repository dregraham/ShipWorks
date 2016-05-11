using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.BestRate
{
    /// <summary>
    /// BestRate RateHashingService
    /// </summary>
    public class BestRateRateHashingService : RateHashingService
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
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsAddWeight);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsHeight);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsLength);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsWidth);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.DimsWeight);
                ratingField.ShipmentFields.Add(BestRateShipmentFields.ServiceLevel);

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
