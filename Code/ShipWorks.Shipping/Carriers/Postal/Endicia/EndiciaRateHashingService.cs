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
                ratingField.ShipmentFields.Add(EndiciaShipmentFields.EndiciaAccountID);
                ratingField.ShipmentFields.Add(EndiciaShipmentFields.OriginalEndiciaAccountID);
                ratingField.ShipmentFields.Add(PostalShipmentFields.SortType);
                ratingField.ShipmentFields.Add(PostalShipmentFields.EntryFacility);

                return ratingField;
            }
        }
    }
}