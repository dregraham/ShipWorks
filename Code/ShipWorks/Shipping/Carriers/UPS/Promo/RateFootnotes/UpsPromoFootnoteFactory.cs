using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.UPS.Promo.RateFootnotes
{
    /// <summary>
    /// An IRateFootnoteFactory for creating USPS promotion footnotes.
    /// </summary>
    public class UpsPromoFootnoteFactory : IRateFootnoteFactory
    {
        private readonly IUpsPromo promo;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsPromoFootnoteFactory(IUpsPromo promo)
        {
            this.promo = promo;
        }

        // todo: get code through constructor
        public ShipmentTypeCode ShipmentTypeCode { get; set; }

        /// <summary>
        /// Creates a UpsPromoFootnote
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Unused but required by Interface
        /// </summary>
        public ShipmentType ShipmentType { get; }
    }
}
