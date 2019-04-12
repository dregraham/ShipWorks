using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Amazon.SWA
{
    /// <summary>
    /// AmazonSWA RateHashingService
    /// </summary>
    [KeyedComponent(typeof(IRateHashingService), ShipmentTypeCode.AmazonSWA)]
    public class AmazonSWARateHashingService : RateHashingService
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

                ratingField.AddShipmentField(AmazonSWAShipmentFields.DimsWeight);
                ratingField.AddShipmentField(AmazonSWAShipmentFields.DimsHeight);
                ratingField.AddShipmentField(AmazonSWAShipmentFields.DimsLength);
                ratingField.AddShipmentField(AmazonSWAShipmentFields.DimsWeight);
                ratingField.AddShipmentField(AmazonSWAShipmentFields.DimsAddWeight);

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
