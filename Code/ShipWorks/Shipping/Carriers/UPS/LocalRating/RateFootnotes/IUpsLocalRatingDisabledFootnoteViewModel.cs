using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    public interface IUpsLocalRatingDisabledFootnoteViewModel
    {
        UpsAccountEntity UpsAccount { get; set; }
        
        ICarrierShipmentAdapter ShipmentAdapter { get; set; }
    }
}