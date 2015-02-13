using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.Promotion
{
    /// <summary>
    /// An IRateFootnoteFactory for creating USPS promotion footnotes.
    /// </summary>
    public class UspsRatePromotionFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UspsRatePromotionFootnoteFactory" /> class.
        /// </summary>
        /// <param name="shipmentType">Type of the shipment.</param>
        /// <param name="shipment">The shipment.</param>
        /// <param name="showSingleAccountDialog">if set to <c>true</c> [show single account dialog].</param>
        public UspsRatePromotionFootnoteFactory(ShipmentType shipmentType, ShipmentEntity shipment, bool showSingleAccountDialog)
        {
            ShipmentType = shipmentType;
            Shipment = shipment;
            ShowSingleAccountDialog = showSingleAccountDialog;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Gets or sets the shipment.
        /// </summary>
        private ShipmentEntity Shipment { get; set; }

        /// <summary>
        /// Notes that this factory should not be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether to [show single account dialog].
        /// </summary>
        public bool ShowSingleAccountDialog { get; private set; }

        /// <summary>
        /// Creates a footnote control.
        /// </summary>
        /// <param name="parameters">Parameters that allow footnotes to interact with the rates grid</param>
        /// <returns>A instance of a RateFoonoteControl.</returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UspsRatePromotionFootnote(Shipment, ShowSingleAccountDialog);
        }
    }
}
