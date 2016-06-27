using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.BestRate.Footnote
{
    /// <summary>
    /// View model for prompting for the store address for counter rates
    /// </summary>
    public interface ICounterRatesInvalidStoreAddressFootnoteViewModel
    {
        /// <summary>
        /// Shipment adapter associated with the current rates
        /// </summary>
        ICarrierShipmentAdapter ShipmentAdapter { get; set; }
    }
}