using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public interface IUspsRatingService
    {
        RateGroup GetRates(ShipmentEntity shipment);
        RateGroup GetRates(ShipmentEntity shipment, bool retrieveExpress1Rates);
    }
}