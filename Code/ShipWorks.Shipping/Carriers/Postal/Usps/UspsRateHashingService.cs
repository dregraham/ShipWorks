using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.HelperClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsRateHashingService : PostalRateHashingService
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
                ratingField.ShipmentFields.Add(UspsShipmentFields.UspsAccountID);
                ratingField.ShipmentFields.Add(UspsShipmentFields.OriginalUspsAccountID);
                ratingField.ShipmentFields.Add(UspsShipmentFields.RateShop);
                ratingField.ShipmentFields.Add(PostalShipmentFields.NoPostage);

                return ratingField;
            }
        }
    }
}
