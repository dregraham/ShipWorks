using ShipWorks.Shipping.Editing;

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
        public Express1NotQualifiedRateFootnoteFactory(ShipmentType shipmentType)
        {
            ShipmentType = shipmentType;
        }

        /// <summary>
        /// Gets the corresponding shipment type for the factory.
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Create an Express1 rate not qualified control
        /// </summary>
        /// <returns></returns>
        public RateFootnoteControl CreateFootnote()
        {
            return new Express1RateNotQualifiedFootnote();
        }
    }
}
