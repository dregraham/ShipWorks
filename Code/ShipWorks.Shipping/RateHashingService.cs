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
                ratingField.ShipmentFields.Add(ShipmentFields.ShipmentType);
                ratingField.ShipmentFields.Add(ShipmentFields.ContentWeight);
                ratingField.ShipmentFields.Add(ShipmentFields.TotalWeight);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipmentCost);
                ratingField.ShipmentFields.Add(ShipmentFields.CustomsValue);

                ratingField.ShipmentFields.Add(ShipmentFields.ShipDate);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipCompany);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipStreet1);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipStreet2);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipStreet3);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipCity);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipStateProvCode);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipPostalCode);
                ratingField.ShipmentFields.Add(ShipmentFields.ShipCountryCode);
                ratingField.ShipmentFields.Add(ShipmentFields.ResidentialDetermination);
                ratingField.ShipmentFields.Add(ShipmentFields.ResidentialResult);

                ratingField.ShipmentFields.Add(ShipmentFields.OriginOriginID);
                ratingField.ShipmentFields.Add(ShipmentFields.OriginCompany);
                ratingField.ShipmentFields.Add(ShipmentFields.OriginStreet1);
                ratingField.ShipmentFields.Add(ShipmentFields.OriginStreet2);
                ratingField.ShipmentFields.Add(ShipmentFields.OriginStreet3);
                ratingField.ShipmentFields.Add(ShipmentFields.OriginCity);
                ratingField.ShipmentFields.Add(ShipmentFields.OriginStateProvCode);
                ratingField.ShipmentFields.Add(ShipmentFields.OriginPostalCode);
                ratingField.ShipmentFields.Add(ShipmentFields.OriginCountryCode);

                ratingField.ShipmentFields.Add(ShipmentFields.ReturnShipment);
                ratingField.ShipmentFields.Add(ShipmentFields.Insurance);
                ratingField.ShipmentFields.Add(ShipmentFields.InsuranceProvider);

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