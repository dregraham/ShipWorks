using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    public class OnTracRateHashingService : RateHashingService
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
                ratingField.ShipmentFields.Add(OnTracShipmentFields.OnTracAccountID);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.CodAmount);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.CodType);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.SaturdayDelivery);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DeclaredValue);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.PackagingType);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsAddWeight);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsHeight);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsLength);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsWidth);
                ratingField.ShipmentFields.Add(OnTracShipmentFields.DimsWeight);

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
