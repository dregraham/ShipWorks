using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// Rate hashing service for OnTrac
    /// </summary>
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
                ratingField.AddShipmentField(OnTracShipmentFields.OnTracAccountID, genericAccountIdFieldName);
                ratingField.AddShipmentField(OnTracShipmentFields.CodAmount);
                ratingField.AddShipmentField(OnTracShipmentFields.CodType);
                ratingField.AddShipmentField(OnTracShipmentFields.SaturdayDelivery);
                ratingField.AddShipmentField(OnTracShipmentFields.DeclaredValue);
                ratingField.AddShipmentField(OnTracShipmentFields.PackagingType);
                ratingField.AddShipmentField(OnTracShipmentFields.DimsAddWeight);
                ratingField.AddShipmentField(OnTracShipmentFields.DimsHeight);
                ratingField.AddShipmentField(OnTracShipmentFields.DimsLength);
                ratingField.AddShipmentField(OnTracShipmentFields.DimsWidth);
                ratingField.AddShipmentField(OnTracShipmentFields.DimsWeight);
                ratingField.AddShipmentField(OnTracShipmentFields.DimsAddWeight);

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
