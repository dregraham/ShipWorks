using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    /// <summary>
    /// An IRateFootnoteFactory for creating UPS promotion footnotes.
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Editing.Rating.IRateFootnoteFactory" />
    public class UpsPromoFootnoteFactory : IRateFootnoteFactory
    {
        private readonly IUpsPromo promo;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoFootnoteFactory(IUpsPromo promo, UpsAccountEntity account)
        {
            this.promo = promo;
            ShipmentTypeCode = account.ShipmentType;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; set; }

        /// <summary>
        /// Creates a UpsPromoFootnote
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UpsPromoFootnote(promo);
        }

        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter)
        {
            //todo: Resolve view model for footnote control
            return null;
        }

        /// <summary>
        /// Not for Best Rate
        /// </summary>
        public bool AllowedForBestRate => false;
    }
}
