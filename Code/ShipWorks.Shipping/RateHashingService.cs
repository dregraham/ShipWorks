using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping
{
    /// <summary>
    /// Base class for RateHashingService - A rate hash is a unique string for a shipments characteristics
    /// </summary>
    public abstract class RateHashingService : IRateHashingService
    {
        protected const string genericAccountIdFieldName = "AccountId";
        protected RatingFields ratingField;

        /// <summary>
        /// Fields of a shipment used to calculate rates
        /// </summary>
        public virtual RatingFields RatingFields
        {
            [NDependIgnoreLongMethod]
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = new RatingFields();
                ratingField.AddShipmentField(ShipmentFields.ShipmentType);
                ratingField.AddShipmentField(ShipmentFields.ContentWeight);
                ratingField.AddShipmentField(ShipmentFields.TotalWeight);
                ratingField.AddShipmentField(ShipmentFields.ShipmentCost);
                ratingField.AddShipmentField(ShipmentFields.CustomsValue);

                ratingField.AddShipmentField(ShipmentFields.ShipDate);
                ratingField.AddShipmentField(ShipmentFields.ShipCompany);
                ratingField.AddShipmentField(ShipmentFields.ShipStreet1);
                ratingField.AddShipmentField(ShipmentFields.ShipStreet2);
                ratingField.AddShipmentField(ShipmentFields.ShipStreet3);
                ratingField.AddShipmentField(ShipmentFields.ShipCity);
                ratingField.AddShipmentField(ShipmentFields.ShipStateProvCode);
                ratingField.AddShipmentField(ShipmentFields.ShipPostalCode);
                ratingField.AddShipmentField(ShipmentFields.ShipCountryCode);
                ratingField.AddShipmentField(ShipmentFields.ResidentialDetermination);
                ratingField.AddShipmentField(ShipmentFields.ResidentialResult);

                ratingField.AddShipmentField(ShipmentFields.OriginOriginID);
                ratingField.AddShipmentField(ShipmentFields.OriginCompany);
                ratingField.AddShipmentField(ShipmentFields.OriginStreet1);
                ratingField.AddShipmentField(ShipmentFields.OriginStreet2);
                ratingField.AddShipmentField(ShipmentFields.OriginStreet3);
                ratingField.AddShipmentField(ShipmentFields.OriginCity);
                ratingField.AddShipmentField(ShipmentFields.OriginStateProvCode);
                ratingField.AddShipmentField(ShipmentFields.OriginPostalCode);
                ratingField.AddShipmentField(ShipmentFields.OriginCountryCode);

                ratingField.AddShipmentField(ShipmentFields.ReturnShipment);
                ratingField.AddShipmentField(ShipmentFields.Insurance);
                ratingField.AddShipmentField(ShipmentFields.InsuranceProvider);

                return ratingField;
            }
        }

        /// <summary>
        /// Gets the rating hash based on the shipment's configuration.
        /// </summary>
        public virtual string GetRatingHash(ShipmentEntity shipment)
        {
            return RatingFields.GetRatingHash(shipment);
        }

        /// <summary>
        /// Is the given field a rating field
        /// </summary>
        public bool IsRatingField(string changedField) => RatingFields.FieldsContainName(changedField);
    }
}