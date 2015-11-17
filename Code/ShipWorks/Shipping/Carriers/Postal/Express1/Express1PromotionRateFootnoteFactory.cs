using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Creates Express1 Rate Promotion footnote controls
    /// </summary>
    public class Express1PromotionRateFootnoteFactory : IRateFootnoteFactory
    {
        private readonly IExpress1SettingsFacade express1Settings;

        /// <summary>
        /// Construct a new Express1PromotionRateFootnoteFactory object
        /// </summary>
        /// <param name="shipmentType">Type of shipment that instantiated this factory</param>
        /// <param name="express1Settings">Settings that will be used when creating the footnote control</param>
        public Express1PromotionRateFootnoteFactory(ShipmentType shipmentType, IExpress1SettingsFacade express1Settings)
        {
            ShipmentTypeCode = shipmentType.ShipmentTypeCode;
            this.express1Settings = express1Settings;
        }

        /// <summary>
        /// Gets the carrier to which this footnote is associated
        /// </summary>
        public ShipmentTypeCode ShipmentTypeCode { get; private set; }

        /// <summary>
        /// Notes that this factory should not be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return false; }
        }

        /// <summary>
        /// Create an Express1 rate promotion control
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new Express1RatePromotionFootnote(express1Settings);
        }
    }
}
