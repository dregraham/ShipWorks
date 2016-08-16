using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// Creates Express1 Rate Not Qualified footnote controls
    /// </summary>
    public class Express1NotQualifiedRateFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Construct a new Express1NotQualifiedRateFootnoteFactory object
        /// </summary>
        /// <param name="shipmentType">Type of shipment that instantiated this factory</param>
        public Express1NotQualifiedRateFootnoteFactory(ShipmentTypeCode shipmentTypeCode)
        {
            ShipmentTypeCode = shipmentTypeCode;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
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
        /// Create an Express1 rate not qualified control
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new Express1RateNotQualifiedFootnote();
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter) =>
            new Express1RateNotQualifiedFootnoteViewModel();
    }
}
