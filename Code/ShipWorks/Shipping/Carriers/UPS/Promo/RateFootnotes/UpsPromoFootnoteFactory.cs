using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Api;
using ShipWorks.Shipping.Carriers.UPS.Promo.API;
using ShipWorks.Shipping.Editing.Rating;

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

        /// <summary>
        /// Creates a UpsPromoFootnote
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UpsPromoFootnote(promo);
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
