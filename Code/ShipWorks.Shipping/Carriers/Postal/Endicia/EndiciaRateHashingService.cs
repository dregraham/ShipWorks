using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    public class EndiciaRateHashingService : PostalRateHashingService
    {
        /// <summary>
        /// Gets the fields used for rating a shipment.
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public override RatingFields RatingFields
        {
            get
            {
                if (ratingField != null)
                {
                    return ratingField;
                }

                ratingField = base.RatingFields;
                ratingField.AddShipmentField(EndiciaShipmentFields.EndiciaAccountID, genericAccountIdFieldName);
                ratingField.AddShipmentField(EndiciaShipmentFields.OriginalEndiciaAccountID);
                ratingField.AddShipmentField(PostalShipmentFields.SortType);
                ratingField.AddShipmentField(PostalShipmentFields.EntryFacility);

                return ratingField;
            }
        }
    }
}