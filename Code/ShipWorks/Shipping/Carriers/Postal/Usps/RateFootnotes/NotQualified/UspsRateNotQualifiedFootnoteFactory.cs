using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.RateFootnotes.NotQualified
{
    /// <summary>
    /// Creates USPS Rate Not Qualified footnote control.
    /// </summary>
    public class UspsRateNotQualifiedFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Construct a new UspsRateNotQualifiedFootnoteFactory object
        /// </summary>
        /// <param name="shipmentType">Type of shipment that instantiated this factory</param>
        public UspsRateNotQualifiedFootnoteFactory(ShipmentTypeCode shipmentTypeCode)
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
        /// Create an USPS rate not qualified control
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new UspsRateNotQualifiedFootnote();
        }

        /// <summary>
        /// Get a view model that represents this footnote
        /// </summary>
        public object CreateViewModel(ICarrierShipmentAdapter shipmentAdapter) =>
            new UspsRateNotQualifiedFootnoteViewModel();
    }
}
