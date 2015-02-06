using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// Factory that creates CounterRatesInvalidStoreAddressFootnoteControls
    /// </summary>
    public class CounterRatesInvalidStoreAddressFootnoteFactory : IRateFootnoteFactory
    {
        /// <summary>
        /// Construct a new CounterRatesInvalidStoreAddressFootnoteControl object
        /// </summary>
        public CounterRatesInvalidStoreAddressFootnoteFactory(ShipmentType shipmentType)
        {
            ShipmentType = shipmentType;
        }

        /// <summary>
        /// Gets and sets the shipment type
        /// </summary>
        public ShipmentType ShipmentType { get; private set; }

        /// <summary>
        /// Notes that this factory should be used in BestRate
        /// </summary>
        public bool AllowedForBestRate
        {
            get { return true; }
        }

        /// <summary>
        /// Create a new CounterRatesInvalidStoreAddressFootnoteControl
        /// </summary>
        public RateFootnoteControl CreateFootnote(FootnoteParameters parameters)
        {
            return new CounterRatesInvalidStoreAddressFootnoteControl(parameters);
        }
    }
}
