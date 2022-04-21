using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Rate hashing service implementation for DHL eCommerce
    /// </summary>
    [KeyedComponent(typeof(IRateHashingService), ShipmentTypeCode.DhlEcommerce)]
    public class DhlEcommerceRateHashingService : RateHashingService
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
                ratingField.AddShipmentField(DhlEcommerceShipmentFields.DhlEcommerceAccountID, genericAccountIdFieldName);
                ratingField.AddShipmentField(DhlEcommerceShipmentFields.SaturdayDelivery);
                ratingField.AddShipmentField(DhlEcommerceShipmentFields.DeliveredDutyPaid);
                ratingField.AddShipmentField(DhlEcommerceShipmentFields.NonMachinable);
                ratingField.AddShipmentField(DhlEcommerceShipmentFields.Service);
                ratingField.AddShipmentField(DhlEcommerceShipmentFields.ResidentialDelivery);

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
