using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.LocalRating.RateFootnotes
{
    /// <summary>
    /// View model for the wpf version of the UpsLocalRatingDisabledFootnote
    /// </summary>
    public interface IUpsLocalRatingDisabledFootnoteViewModel
    {
        /// <summary>
        /// Gets or sets the ups account.
        /// </summary>
        UpsAccountEntity UpsAccount { get; set; }

        /// <summary>
        /// Gets or sets the shipment adapter.
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; set; }
    }
}