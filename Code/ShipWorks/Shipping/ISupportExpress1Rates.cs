using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping
{
    public interface ISupportExpress1Rates
    {
        RateGroup GetRates(ShipmentEntity shipment, bool shouldRetrieveExpress1Rates);
    }
}